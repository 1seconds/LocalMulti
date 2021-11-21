using UnityEngine;

public class TabBase : MonoBehaviour {
    [SerializeField] private string key = "";
    [SerializeField] public int index = 0;
    [SerializeField] private Canvas canvas;

    public bool? isShown { get; private set; }

    public virtual void Show(bool show) {
        if (isShown == show) {
            return;
        }

        isShown = show;
        if (isShown == true) {
            canvas.enabled = true;
            OnShow();
        } else {
            canvas.enabled = false;
            OnHide();
        }
    }

    public virtual void OnShowDone(bool show) {
    }

    public string GetKey() {
        return key;
    }

    protected virtual void OnShow() {
    }

    protected virtual void OnHide() {
    }
}