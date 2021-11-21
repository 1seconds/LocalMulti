using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class SceneControllerBase : MonoBehaviour, SceneController {
    [SerializeField] private MyCanvasScaler canvasScaler;
    
    public abstract SceneType sceneType { get; }
    public Canvas canvas { get; private set; }
    public abstract GameObject defaultUI { get; }
    public abstract GameObject alertLayer { get; }
    public abstract GameObject waitingLayer { get; }
    public abstract GameObject pushLayer { get; }
    public HashSet<IPopupPanel> popups { get; private set; }
    public CommonAlertBox currentCommonAlert { get; set; }

    private static SceneType returnScene = SceneType.Intro;
    private static Stack<SceneType> sceneHistory = new Stack<SceneType>();
    private static SceneTransitionEffect sceneTransitionEffect;

    private CommonToastMessage lastToastMessage;
    private int waitingPanelRefCount;
    private Dictionary<BlackPanelObserver, GoItemRef<BlackPanel>> blackPanelMap;
    private CoroutineOnce co;
    private WaitingPanel lastWaiting;

    protected bool InitController(bool forceStart = false) {
        co = new CoroutineOnce(this);
        Service.scene = this;
        blackPanelMap = new Dictionary<BlackPanelObserver, GoItemRef<BlackPanel>>();
        popups = new HashSet<IPopupPanel>();
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex != 0 && Service.ready == false && !forceStart) {
            returnScene = this.sceneType;
            SceneManager.LoadScene(0);
            return false;
        }

        canvas = GetComponent<Canvas>();
        if (sceneTransitionEffect != null) {
            sceneTransitionEffect.Hide().RunAsync();
        }
        
        return true;
    }

    protected void InitEventSystemDPI() {
        int defaultValue = EventSystem.current.pixelDragThreshold;        
        EventSystem.current.pixelDragThreshold = 
            Mathf.Max(
                defaultValue , 
                (int) (defaultValue * Screen.dpi / 160f));        
    }

    public void CloseController() {
        Service.scene = null;
    }

    public virtual void OnSceneChanged(SceneType toScene) {
    }

    public async Task SwitchScene(SceneType scene, SceneTransitionEffect effect = null) {
        Time.timeScale = 1;
        OnSceneChanged(scene);
        
        sceneHistory.Push(scene);
        sceneTransitionEffect = effect;
        if (sceneTransitionEffect != null) {
            await sceneTransitionEffect.Display();
        }
        
        SceneManager.LoadScene("Empty");
    }

    public static SceneType GetNextScene() {
        if (sceneHistory.Count == 0) {
            return SceneType.Intro;
        }
        
        return sceneHistory.Peek();
    }

    public async Task SwitchPrevScene(SceneTransitionEffect effect = null) {
        if (sceneHistory.Count > 0) {
            sceneHistory.Pop();
            sceneTransitionEffect = effect;
            if (sceneTransitionEffect != null) {
                await sceneTransitionEffect.Display();
            }

            SceneManager.LoadScene("Empty");
        }
    }

    public async Task SwitchSceneWithHistoryClear(SceneType scene, SceneTransitionEffect effect = null) {
        sceneHistory.Clear();
        sceneTransitionEffect = effect;
        if (sceneTransitionEffect != null) {
            await sceneTransitionEffect.Display();
        }

        SceneManager.LoadScene("Empty");
    }

    public Coroutine Run(IEnumerator iterationResult) {
        if (this.gameObject.activeSelf) {
            return StartCoroutine(iterationResult);
        }
        return null;
    }

    public void Stop(Coroutine coroutine) {
        this.StopCoroutineSafe(coroutine);
    }

    public void RunAfter(float delay, Action action) {
        if (delay <= 0) {
            action?.Invoke();
            return;
        }
        
        StartCoroutine(RunAfterRoutine(delay, action));
    }

    private IEnumerator RunAfterRoutine(float delay, Action action) {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public void ShowBlackPanel(BlackPanelObserver ob) {
        if (blackPanelMap.ContainsKey(ob)) {
            return;
        }
        
        var path = "Prefabs/UI/Common/BlackPanel";
        var blackPanel = Service.goPooler.Get<BlackPanel>(path, Service.scene.defaultUI);
        
        blackPanel.Display(ob);
        blackPanelMap.Add(ob, new GoItemRef<BlackPanel>(blackPanel));
    }

    public void HideBlackPanel(BlackPanelObserver ob) {
        GoItemRef<BlackPanel> blackPanel; 
        blackPanelMap.TryGetValue(ob, out blackPanel);

        if (blackPanel != null && blackPanel.Get() != null) {
            blackPanel.Get().Hide();
        }

        blackPanelMap.Remove(ob);
    }    

    public IEnumerator ShowAlert(string message, AlertBoxType type, AlertBoxOutResult result) {
        while (currentCommonAlert != null) {
            yield return new WaitForSeconds(0.1f);
        }

        if (Service.scene == null) {
            result.value = AlertBoxResult.None;
            yield break;
        }

        CommonAlertBox commonAlert = this.InstantiateUI<CommonAlertBox>("Common/AlertBox", pushLayer);
        if (commonAlert != null) {
            yield return commonAlert.Show(message, type, result);
        }
    }

    public IEnumerator ShowAlertKey(string key, AlertBoxType type, AlertBoxOutResult result) {
        string message = Service.sb.Get(key);
        yield return ShowAlert(message, type, result);
    }

    public async Task<AlertBoxResult> ShowAlert(string message, AlertBoxType type) {
        var result = new AlertBoxOutResult();
        await ShowAlert(message, type, result);
        return result.value;
    }

    public async Task<AlertBoxResult> ShowAlertKey(string key, AlertBoxType type) {
        string message = Service.sb.Get(key);
        return await ShowAlert(message, type);
    }

    public void ShowPushAlert(string title, string body) {
        var path = "Prefabs/UI/Common/PushAlert";
        var alert = Service.goPooler.Get<PushAlert>(path, pushLayer);
        if (alert!= null) {
            alert.Display(title, body);
        }
    }

    public void ShowToastAlert(string text) {
        Color bgColor = Color.yellow;
        Color txtColor = Color.black;

        if (lastToastMessage != null) {
            lastToastMessage.Hide();
        }

        var path = "Prefabs/UI/Common/CommonToastAlert";
        var message = Service.goPooler.Get<CommonToastMessage>(path, alertLayer);
        if (message != null) {
            message.Show(text, bgColor, txtColor);
            lastToastMessage = message;
        }
    }

    public void ShowToastAlertKey(string key, params object[] values) {
        ShowToastAlert(Service.sb.Get(key, values));
    }
    
    public void FitScreen() {
        var width = 1080f;
        var height = Screen.height * width / Screen.width;
        var scaler = GetComponent<CanvasScaler>();
        scaler.referenceResolution = new Vector2(width, height);
    }
    
    public virtual void ShowWaitingPanel() {
        waitingPanelRefCount++;
        if (lastWaiting != null) {
            if (!lastWaiting.gameObject.activeSelf) {
                lastWaiting.Show();
            }
            return;
        }
        
        GameObject anchor = waitingLayer;
        string path = "Common/WaitingPanel";
        lastWaiting = this.InstantiateUI<WaitingPanel>(path, anchor);
        lastWaiting.gameObject.name = "Waiting";
        lastWaiting.Show();
    }

    public virtual void HideWaitingPanel() {
        waitingPanelRefCount--;
        if (waitingPanelRefCount < 0) {
            waitingPanelRefCount = 0;
        }
        
        if (lastWaiting != null && waitingPanelRefCount == 0) {
            lastWaiting.Hide();
            //lastWaiting = null;
        }
    }
    
    public async Task<T> Api<T>(Func<Task<T>> method) {
        while (true) {
            Service.scene.ShowWaitingPanel();
            var res = await method();
			
            // dummy test
            await new WaitForSeconds(0.5f);
            Service.scene.HideWaitingPanel();
			
            if (res == null) {
                var key = "common.loading.failed";
                var result = await Service.scene.ShowAlertKey(key, AlertBoxType.Retry);
                if (result == AlertBoxResult.Ok) {
                    continue;
                } 
            }

            return res;
        }
    }
    
    public async Task<bool> Api(Func<Task<bool>> method) {
        while (true) {
            Service.scene.ShowWaitingPanel();
            var res = await method();
			
            // dummy test
            await new WaitForSeconds(0.5f);
            Service.scene.HideWaitingPanel();
			
            if (res == false) {
                var key = "common.loading.failed";
                var result = await Service.scene.ShowAlertKey(key, AlertBoxType.Retry);
                if (result == AlertBoxResult.Ok) {
                    continue;
                } 
            }

            return res;
        }
    }

    public void RunOnce(IEnumerator method) {
        co.Run(method);
    }
    
    public void Stop(IEnumerator method) {
        co.Stop(method);
    }

    public Vector2 GetSceneSize() {
        return canvasScaler.GetSize();
    }

    public GoItemRef<PointingLabel> ShowPointingLabel(string message, float duration, RectTransform target, Vector2 offset, PointingDirection dir) {
        var path = "Prefabs/UI/Common/PointingLabel";
        var label = Service.goPooler.Get<PointingLabel>(path, Service.scene.defaultUI);
        if (label == null) {
            Debug.LogError("Can not found label");
            return null;
        }

        label.Display(message, duration, target, offset, dir);
        return new GoItemRef<PointingLabel>(label);
    }

    public bool HasPopup() {
        return popups.Count > 0;
    }

    public void AddPopup(IPopupPanel panel) {
        popups.Add(panel);
    }

    public void HideAllPopups() {
        var popupList = popups.ToList();
        foreach (var popup in popupList) {
            popup.Hide();
        }
        popups.Clear();
    }

    public void RemovePopup(IPopupPanel panel) {
        popups.Remove(panel);
    }
}