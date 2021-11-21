using System.Collections;
using UnityEngine;

public delegate void OnBottomPanelEvent(float height);

public class BottomPanel : MonoBehaviour {
    [SerializeField] private RectTransform anchor;
    [SerializeField] private float height = 100;

    public bool isShown { get; private set; }
    public event OnBottomPanelEvent onMoving;

    private void Awake() {
        HideImmediately();
    }

    private void HideImmediately() {
        var to = new Vector3(0, -height - 150, 0);
        anchor.anchoredPosition = to;
        anchor.gameObject.SetActive(false);
        isShown = false;
    }

    public void Show() {
        anchor.gameObject.SetActive(true);
        isShown = true;
    }

    public void Hide(bool easing) {
        isShown = false;
        if (easing == false || !this.gameObject.activeInHierarchy) {
            HideImmediately();
        } else {
            StartCoroutine(HideRoutine());
        }
    }

    private IEnumerator ShowRoutine() {
        var to = Vector3.zero;
        Vector3 from = anchor.anchoredPosition;
        var easeType = EaseType.easeOutQuad;
        var duration = 0.3f;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            var progress = ease.Run();
            var pt = Vector3.Lerp(from, to, progress);
            anchor.anchoredPosition = pt;
            onMoving?.Invoke(height + pt.y);
            yield return null;
        }
    }

    private IEnumerator HideRoutine() {
        var to = new Vector3(0, -height - 150, 0);
        Vector3 from = anchor.anchoredPosition;
        var easeType = EaseType.easeOutQuad;
        var duration = 0.3f;

        var ease = new EaseRunner(easeType, duration);
        while (ease.IsPlaying()) {
            var progress = ease.Run();
            var pt = Vector3.Lerp(from, to, progress);
            anchor.anchoredPosition = pt;
            onMoving?.Invoke(height + pt.y);
            yield return null;
        }
    
        anchor.gameObject.SetActive(false);
    }
}
