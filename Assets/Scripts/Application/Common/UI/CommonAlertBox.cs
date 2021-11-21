using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum AlertBoxType {
    Ok = 0,
    OkCancel,
    YesNo,
    Retry,
    Purchase,
    Stake,
    Update,
}

public enum AlertBoxResult {
    None = 0,
    Ok,
    Cancel,
    Yes,
    No,
    Shop,
}

public class AlertBoxOutResult : OutResult<AlertBoxResult> {
}

public class CommonAlertBox : MonoBehaviour, BlackPanelObserver {
    [SerializeField] private Text textLabel;
    
    private AlertBoxResult result = AlertBoxResult.None;
    private DescendantMap descendant;
    private CanvasGroup panel;
    private RectTransform rectTransform;

    private void Awake() {
        descendant = new DescendantMap(this);
        panel = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnDestroy() {
        Service.scene.currentCommonAlert = null;
    }

    public void OnClickOk() {
        Service.sound.PlayEffect("click");
        result = AlertBoxResult.Ok;
    }

    public void OnClickCancel() {
        Service.sound.PlayEffect("click");
        result = AlertBoxResult.Cancel;
    }

    public void OnClickYes() {
        Service.sound.PlayEffect("click");
        result = AlertBoxResult.Yes;
    }

    public void OnClickNo() {
        Service.sound.PlayEffect("click");
        result = AlertBoxResult.No;
    }

    public void OnClickRetry() {
        Service.sound.PlayEffect("click");
        result = AlertBoxResult.Ok;
    }

    public void OnClickShop() {
        Service.sound.PlayEffect("click");
        result = AlertBoxResult.Shop;
        //#TODO Show ShopPanel
    }

    public void OnClickUpdate() {
        Service.sound.PlayEffect("click");
        Service.OpenURL(Service.config.GetDownloadUrl());
        result = AlertBoxResult.Cancel;
    }

    private void OnDisable() {
        Service.scene.currentCommonAlert = null;
        Service.scene.HideBlackPanel(this);
    }

    private void FitSize() {
        var height = textLabel.preferredHeight;
        var sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = Mathf.Max(100, height) + 200;
        rectTransform.sizeDelta = sizeDelta;
    }

    public IEnumerator Show(string message, AlertBoxType type, AlertBoxOutResult outResult) {
        this.gameObject.SetActive(true);
        Service.scene.currentCommonAlert = this;
        textLabel.text = message;
        descendant.Get("Type"+type).SetActive(true);
        FitSize();

        panel.alpha = 0;
        panel.transform.localScale = Vector3.one * 0.95f;
        yield return null;
        Service.scene.ShowBlackPanel(this);
        StartCoroutine(panel.AlphaTo(EaseType.easeOutQuad, 0.1f, 1f));
        yield return panel.gameObject.ScaleTo(EaseType.easeOutQuad, 0.2f, Vector3.one);
        
        while (this.result == AlertBoxResult.None) {
            yield return null;
        }

        if (outResult != null) {
            outResult.value = this.result;
        }

        yield return panel.gameObject.ScaleTo(EaseType.easeInQuad, 0.1f, Vector3.one * 0.95f);
        Service.scene.currentCommonAlert = null;
        Destroy(this.gameObject);
    }

    // BlackPanelObserver interface implementation
    //-------------------------------------------------------------------------
    public void OnClickBlackPanel() {
        result = AlertBoxResult.Cancel;
    }

    public void OnAndroidBack() {
        result = AlertBoxResult.Cancel;
    }

    public CanvasGroup QueryBlackTargetPanel() {
        return panel;
    }

    public float QueryBlackPanelAlpha() {
        return 0.8f;
    }
}
