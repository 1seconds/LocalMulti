using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkPanel : MonoBehaviour {
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Image background;

    public IEnumerator Blink(float duration, Color color) {
        gameObject.SetActive(true);
        background.color = color;
        canvas.alpha = 1;
        yield return new WaitForSeconds(duration);
        yield return canvas.AlphaTo(EaseType.easeOutQuad, 0.4f, 0);
        gameObject.SetActive(false);
    }

    public void Begin(Color color, float alpha) {
        gameObject.SetActive(true);
        background.color = color;
        canvas.alpha = alpha;
    }

    public IEnumerator Fade(EaseType easeType, float duration, float to) {
        var from = canvas.alpha;
        yield return this.Ease(easeType, duration, (value) => {
            canvas.alpha = Mathf.Lerp(from, to, value);
        });
    }

    public void End() {
        this.gameObject.SetActive(false);
    }
}