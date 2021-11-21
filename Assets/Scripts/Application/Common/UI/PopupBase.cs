using UnityEngine;
using UnityEngine.UI;

public class PopupBase : PopupPanel, BlackPanelObserver {
    [SerializeField] private CanvasGroup panel;
    
    protected virtual void OnDisable() {
        Service.scene.HideBlackPanel(this);
    }

    public virtual void Display() {
        this.gameObject.SetActive(true);
        panel.alpha = 0;
        Service.scene.ShowBlackPanel(this);
        Show();
        OnDisplay();
        RefreshUI();
    }

    protected virtual void OnDisplay() {
    }

    protected virtual void RefreshUI() {
    }

    public virtual void OnClickBlackPanel() {
        Hide();
    }

    public virtual void OnAndroidBack() {
        Hide();
    }

    public float QueryBlackPanelAlpha() {
        return 0.8f;
    }

    public CanvasGroup QueryBlackTargetPanel() {
        return panel;
    }
}