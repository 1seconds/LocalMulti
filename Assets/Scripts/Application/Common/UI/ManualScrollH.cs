using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManualScrollH : MonoBehaviour {
    [SerializeField] private ScrollRect scrollRect;

    private RectTransform content => scrollRect.content;
    private RectTransform viewPort => scrollRect.viewport;
    private Coroutine moveRoutine;

    public enum AutoMoveState {
        None,
        Left,
        Right
    }

    public AutoMoveState moveState { get; private set;}

    public float GetScrollRate() {
        var width = GetScrollWidth();
        var x = -content.anchoredPosition.x;
        return x / width;
    }

    public float GetScroll() {
        return content.anchoredPosition.x;
    }

    public void SetScroll(float rate) {
        var width = GetScrollWidth();
        content.anchoredPosition = new Vector3(-width * rate, 0);
    }

    public void SetScrollDelta(Vector2 delta) {
        content.anchoredPosition += delta;
    }

    public void AutoScrollTo(float to, float duration) {
        var hC = content.sizeDelta.x;
        var hV = viewPort.rect.width;
        var width = GetScrollWidth();

        var x = to - hV * 0.5f;
        x = Mathf.Max(0, x);
        x = Mathf.Min(width, x);

        var toPos = new Vector2(x, 0);
        StartCoroutine(content.MoveTo(EaseType.easeOutQuad, duration, toPos));
    }

    public float GetViewportWidth() {
        return viewPort.rect.width;
    }

    public float GetContentWidth() {
        return content.rect.width;
    }

    public float GetScrollWidth() {
        return Mathf.Max(content.sizeDelta.x - viewPort.rect.width, 0);
    }

    public void StartAutoMoveLeft(float speed) {
        if (moveState == AutoMoveState.Right) {
            return;
        }

        StopAutoMove();
        moveState = AutoMoveState.Right;
        moveRoutine = Service.scene.Run(AutoMove(speed));
    }

    public void StartAutoMoveRight(float speed) {
        if (moveState == AutoMoveState.Left) {
            return;
        }

        StopAutoMove();
        moveState = AutoMoveState.Left;
        moveRoutine = Service.scene.Run(AutoMove(speed));
    }

    public void StopAutoMove() {
        if (moveState == AutoMoveState.None) {
            return;
        }

        moveState = AutoMoveState.None;
        Service.scene.Stop(moveRoutine);
        moveRoutine = null;
    }

    private IEnumerator AutoMove(float speed) {
        while (moveState != AutoMoveState.None) {
            var dt = Time.deltaTime;
            Vector2 pt;
            if (moveState == AutoMoveState.Left) {
                pt = content.anchoredPosition;
                pt.x -= speed * dt;
                if (pt.x > 0) {
                    pt.x = 0;
                }
            } else {
                pt = content.anchoredPosition;
                pt.x += speed * dt;
                var width = GetScrollWidth();
                if (pt.x < -width) {
                    pt.x = -width;
                }
            }

            content.anchoredPosition = pt;
            yield return null;
        }
    }
}