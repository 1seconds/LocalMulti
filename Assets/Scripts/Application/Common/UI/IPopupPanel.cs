using System;
using UnityEngine;
using System.Collections;

public interface IPopupPanel {
    void Hide();
}

public class PopupPanel : GoItem, IPopupPanel {
    private PopupPanelEaser easer = new PopupPanelEaserAlpha();
    public bool isShown { get; private set;  }

    public virtual void Show() {
        easer.Show(this);
        isShown = true;
        Service.scene.AddPopup(this);
    }

    public virtual void Hide() {
        Service.scene.RemovePopup(this);
        easer.Hide(this);
        isShown = false;
    }

    public void SetEaser(PopupPanelEaser newEaser) {
        easer = newEaser;
    }
}

public interface PopupPanelEaser {
    void Show(MonoBehaviour target);
    void Hide(MonoBehaviour target);
}

public class PopupPanelEaserAlpha : PopupPanelEaser {
    private Coroutine popupRoutine;
    private CanvasGroup canvasGroup;
    private float showDuration = 0.1f;
    private float hideDuration = 0.05f;

    public PopupPanelEaserAlpha() {
    }

    public PopupPanelEaserAlpha(CanvasGroup canvasGroup) {
        this.canvasGroup = canvasGroup;
    }

    public PopupPanelEaserAlpha(float showDuration, float hideDuration) {
        this.showDuration = showDuration;
        this.hideDuration = hideDuration;
    }

    public void Show(MonoBehaviour target) {
        if (canvasGroup == null) {
            canvasGroup = target.GetComponent<CanvasGroup>();
        }
        target.StopCoroutineSafe(popupRoutine);
        popupRoutine = target.StartCoroutine(ShowRoutine());
    }

    public void Hide(MonoBehaviour target) {
        if (canvasGroup == null) {
            canvasGroup = target.GetComponent<CanvasGroup>();
        }

        target.StopCoroutineSafe(popupRoutine);
        popupRoutine = target.StartCoroutine(HideRoutine());
    }

    private IEnumerator ShowRoutine() {
        canvasGroup.alpha = 0;
        yield return canvasGroup.AlphaTo(EaseType.easeOutQuad, showDuration, 1f);
    }

    private IEnumerator HideRoutine() {
        yield return canvasGroup.AlphaTo(EaseType.easeOutQuad, hideDuration, 0f);
        canvasGroup.gameObject.SetActive(false);
    }
}


public class PopupPanelEaserBottomUp : PopupPanelEaser {
    private Coroutine popupRoutine;
    private RectTransform anchor;
    private float height = 1440;
    private float duration = 0.3f;
    private EaseType easeType;
    
    public Action onHideDone;

    public PopupPanelEaserBottomUp(RectTransform anchor, float height, float duration, EaseType easeType) {
        this.easeType = easeType;
        this.height = height;
        this.duration = duration;
        this.anchor = anchor;
    }

    public void Show(MonoBehaviour target) {
        target.StopCoroutineSafe(popupRoutine);
        popupRoutine = target.StartCoroutine(ShowRoutine());
    }

    public void Hide(MonoBehaviour target) {
        target.StopCoroutineSafe(popupRoutine);
        popupRoutine = target.StartCoroutine(HideRoutine());
    }

    private IEnumerator ShowRoutine() {
        anchor.anchoredPosition = new Vector3(0, -height, 0);
        yield return anchor.MoveTo(easeType, duration, Vector3.zero);
    }

    private IEnumerator HideRoutine() {
        yield return anchor.MoveTo(easeType, duration, new Vector3(0, -height, 0));
        onHideDone?.Invoke();
    }
}

public class PopupPanelEaserLeftRight : PopupPanelEaser {
    private Coroutine popupRoutine;
    private CanvasGroup canvasGroup;
    private float width = 720;

    public PopupPanelEaserLeftRight(float width) {
        this.width = width;
    }

    public void Show(MonoBehaviour target) {
        canvasGroup = target.GetComponent<CanvasGroup>();
        target.StopCoroutineSafe(popupRoutine);
        popupRoutine = target.StartCoroutine(ShowRoutine());
    }

    public void Hide(MonoBehaviour target) {
        canvasGroup = target.GetComponent<CanvasGroup>();
        target.StopCoroutineSafe(popupRoutine);
        popupRoutine = target.StartCoroutine(HideRoutine());
    }

    private IEnumerator ShowRoutine() {
        canvasGroup.transform.localPosition = new Vector3(-width, 0, 0);
        yield return canvasGroup.gameObject.MoveTo(EaseType.easeOutQuad, 0.3f, Vector3.zero);
    }

    private IEnumerator HideRoutine() {
        yield return canvasGroup.gameObject.MoveTo(EaseType.easeOutQuad, 0.3f, new Vector3(-width, 0, 0));
        canvasGroup.gameObject.SetActive(false);
    }
}
