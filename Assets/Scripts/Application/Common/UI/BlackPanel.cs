using UnityEngine;
using System.Collections;

public interface BlackPanelObserver {
    void OnClickBlackPanel();
    void OnAndroidBack();
    float QueryBlackPanelAlpha();
    CanvasGroup QueryBlackTargetPanel();
}

public class BlackPanel : GoItem {
    [SerializeField] private StaticBluredScreen bluredScreen;
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private float showDuration = 0.15f;

    private Coroutine alphaRoutine;
    private Bec checker = new Bec();
    private BlackPanelObserver observer;

    public void Display(BlackPanelObserver ob) {
        this.observer = ob;
        this.StopCoroutineSafe(alphaRoutine);
        if (gameObject.activeSelf == false) {
            gameObject.SetActive(true);
        }

        UpdatePosition(ob);
        if (ob != null) {
            var c = 1 - ob.QueryBlackPanelAlpha();
            bluredScreen.color = new Color(c, c, c);
        }
        
        bluredScreen.Capture();
        alphaRoutine = Service.scene.Run(panel.AlphaTo(EaseType.easeInOutQuad, showDuration, 1f));
    }

    public void Hide() {
        Service.scene.Stop(alphaRoutine);
        alphaRoutine = Service.scene.Run(HideRoutine());
    }

    public void OnClickClose() {
        if (checker.CanEnter("Close", 0.5f) == false) {
            return;
        }

        if (observer != null) {
            observer.OnClickBlackPanel();
        }        
    }

    private IEnumerator HideRoutine() {
        yield return panel.AlphaTo(EaseType.easeInOutQuad, 0.15f, 0f);
        bluredScreen.Release();
        Service.goPooler.Return(this);
    }

    private void UpdatePosition(BlackPanelObserver ob) {
        if (ob == null) {
            return;
        }
        
        var targetPanel = ob.QueryBlackTargetPanel();
        var parent = targetPanel.transform.parent;
        var index = targetPanel.transform.GetSiblingIndex();

        if (transform.parent != parent) {
            transform.SetParent(parent);
        }

        if (transform.GetSiblingIndex() < index) {
            index--;
        }

        transform.SetSiblingIndex(index);
        SetPosition();
    }

    private void SetPosition() {
        var parent = transform.parent;
        var pos = Vector2.zero;
        while (parent != null) {
            if (parent.parent == null) {
                break;
            }
            
            pos += parent.GetComponent<RectTransform>().anchoredPosition;
            parent = parent.parent;
        }

        this.transform.localScale = Vector3.one;
        this.GetComponent<RectTransform>().anchoredPosition = -pos;
    }

    private void Update() {
        if (observer != null && Input.GetKey(KeyCode.Escape)) {
            observer.OnAndroidBack();
        }
    }
}
