using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroLogo : MonoBehaviour {
    [SerializeField] private CanvasGroup canvas;

    public IEnumerator Display(float playTime = 1.5f) {
        this.gameObject.SetActive(true);
        canvas.alpha = 0;
        Service.sound.PlayEffect("ready", 0f, 0f);
        
        yield return canvas.AlphaTo(EaseType.easeOutQuad, 0.5f, 1f);
        yield return new WaitForSeconds(playTime - 0.6f);
        yield return canvas.AlphaTo(EaseType.easeOutQuad, 0.1f, 0f);
        this.gameObject.SetActive(false);
    }
}