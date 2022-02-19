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