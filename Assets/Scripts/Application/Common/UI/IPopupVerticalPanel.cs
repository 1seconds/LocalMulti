using System.Collections;
using UnityEngine;

public class IPopupVerticalPanel : MonoBehaviour, IPopupPanel, BlackPanelObserver {
    private Coroutine movingRoutine;
    private RectTransform rt;
    private CanvasGroup canvas;
    [SerializeField] private Vector2 hidingPos = new Vector2(0, -1500f);
    
    private void Awake() {
        rt = GetComponent<RectTransform>();
        canvas = GetComponent<CanvasGroup>();
    }

    public virtual void Display() {
        gameObject.SetActive(true);
        Service.scene.AddPopup(this);
        rt.anchoredPosition = hidingPos;
        StartMoving(Vector3.zero, false);
        Service.scene.ShowBlackPanel(this);
    }
    
    private void OnDisable() {
        Service.scene.RemovePopup(this);
        Service.scene.HideBlackPanel(this);
    }
    
    public void OnClickBlackPanel() {
        Hide();
    }

    public void OnAndroidBack() {
        Hide();
    }

    public float QueryBlackPanelAlpha() {
        return 0.7f;
    }

    public CanvasGroup QueryBlackTargetPanel() {
        return canvas;
    }
    
    public virtual void Hide() {
        Service.scene.HideBlackPanel(this);
        StartMoving(hidingPos, true);
    }
    
    private void StartMoving(Vector2 movingPos, bool bHide) {
        if (movingRoutine != null) {
            StopCoroutine(movingRoutine);
        }
        movingRoutine = Service.scene.Run(MovingRoutine(movingPos, bHide));
    }
    
    private IEnumerator MovingRoutine(Vector3 to, bool hide) {
        yield return rt.MoveTo(EaseType.easeOutQuad, 0.25f, to);
        if (hide) {
            this.gameObject.SetActive(false);
        }
    }
}