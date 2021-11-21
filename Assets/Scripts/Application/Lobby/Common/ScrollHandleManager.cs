using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnClickHandle();

public class ScrollHandleManager : MonoBehaviour {
    public static ScrollHandleManager instance;
    public event OnClickHandle onClickHandle;

    private void Awake() {
        instance = this;
    }

    private void OnDestroy() {
        instance = null;
    }

    public void OnClickHandle() {
        onClickHandle?.Invoke();
    }

    public void ScrollToTop(ScrollRect scrollRect) {
        var content = scrollRect.content;
        var from = content.anchoredPosition;
        Service.scene.RunOnce(this.Ease(EaseType.easeOutQuad, 0.33f, (value) => {
            content.anchoredPosition = Vector2.Lerp(from, Vector2.zero, value);
        }));
    }
}
