using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public delegate void BodyScrollEvent(int index);

public class BodyScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] private ManualScrollH scrollH;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private int tabSize = 5;

    public BodyScrollEvent onStartMoveTo;
    public BodyScrollEvent onBeginDrag;
    public BodyScrollEvent onDrag;
    public BodyScrollEvent onEndDrag;

    private int dragStartingIndex = 0;
    private Coroutine moveRoutine = null;
    private Vector2 velocity;
    private Vector2 lastPos;

    public void EnableScroll(bool enabled) {
        this.enabled = enabled;
        scrollH.enabled = enabled;
        scrollRect.enabled = enabled;
    }

    public void ScrollTo(int tabIndex, float duration = 0.4f) {
        var toPos = -tabIndex * 1280f;
        float fromRate = scrollH.GetScrollRate();
        float toRate = Mathf.Abs(toPos) / scrollH.GetScrollWidth();

        onStartMoveTo?.Invoke(tabIndex);
        if (duration > 0f) {
            Service.scene.Stop(moveRoutine);
            moveRoutine = Service.scene.Run(MoveTo(duration, fromRate, toRate, tabIndex));
        } else {
            scrollH.SetScroll(toRate);
            onEndDrag?.Invoke(tabIndex);
        }
    }

    public void SetTabSize(int size) {
        tabSize = size;
    }

    public void OnBeginDrag(PointerEventData data) {
        if (!scrollRect.enabled) {
            return;
        }

        Service.scene.Stop(moveRoutine);
        var scroll = scrollH.GetScroll();
        dragStartingIndex = Mathf.Abs(Mathf.RoundToInt(scroll / 1280f));
        onBeginDrag?.Invoke(dragStartingIndex);
        lastPos = data.position;
    }

    public void OnDrag(PointerEventData data) {
        if (!scrollRect.enabled) {
            return;
        }

        velocity = (data.position - lastPos) / Time.deltaTime;
        lastPos = data.position;

        FireDragEvent();
    }

    private void FireDragEvent() {
        var scroll = scrollH.GetScroll();
        var index = Mathf.Abs(Mathf.FloorToInt(scroll / 1280f));
        onDrag?.Invoke(index);
    }

    public void OnEndDrag(PointerEventData data) {
        if (!scrollRect.enabled) {
            return;
        }

        var scroll = scrollH.GetScroll();
        var width = scrollH.GetScrollWidth();
        var toScroll = scroll + velocity.x;
        toScroll = Mathf.Min(0, toScroll);
        toScroll = Mathf.Max(-width, toScroll);

        var tabIndex = Mathf.Abs(Mathf.RoundToInt(toScroll / 1280f));
        if (tabIndex < dragStartingIndex) {
            tabIndex = Mathf.Max(0, dragStartingIndex - 1);
        } else if (tabIndex > dragStartingIndex) {
            tabIndex = Mathf.Min(dragStartingIndex + 1, tabSize - 1);
        }

        var toPos = -tabIndex * 1280f;
        toPos = Mathf.Min(0, toPos);
        toPos = Mathf.Max(-width, toPos);

        float fromRate = scrollH.GetScrollRate();
        float toRate = Mathf.Abs(toPos) / width;

        var vx = Mathf.Abs(velocity.x);
        vx = Mathf.Min(1280f, vx);
        var durationScaler = 1 - (vx / 1280f * 0.5f);

        float distance = Mathf.Abs(scroll - toPos);
        var duration = distance / 1280f * 0.7f * durationScaler;

        Service.scene.Stop(moveRoutine);
        onStartMoveTo?.Invoke(tabIndex);
        moveRoutine = Service.scene.Run(MoveTo(duration, fromRate, toRate, tabIndex));
        scrollRect.velocity = Vector2.zero;
        scrollRect.elasticity = 0.1f;
        scrollRect.inertia = false;
    }

    private IEnumerator MoveTo(float duration, float fromRate, float toRate, int index) {
        yield return this.Ease(EaseType.easeOutQuad, duration, (value) => {
            scrollH.SetScroll(Mathf.Lerp(fromRate, toRate, value));
            FireDragEvent();
        });

        onEndDrag?.Invoke(index);
        scrollRect.elasticity = 0.1f;
        scrollRect.inertia = true;
    }
}
