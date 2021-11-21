using System.Threading.Tasks;
using UnityEngine;

public class SceneTransitionEffect : MonoBehaviour, BlackPanelObserver {
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private StaticBluredScreen bluredScreen;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public async Task Display() {
        gameObject.SetActive(true);
        await new WaitForEndOfFrame();

        canvas.alpha = 0;
        bluredScreen.color = new Color(0.5f, 0.5f, 0.5f);
        bluredScreen.Capture();
        await canvas.AlphaTo(EaseType.easeInQuad, 0.4f, 1);
        await new WaitForSeconds(0.2f);
    }

    public async Task Hide() {
        await canvas.AlphaTo(EaseType.easeInQuad, 0.4f, 0);
        bluredScreen.Release();
        Destroy(gameObject);
    }

    public void OnClickBlackPanel() {
    }

    public void OnAndroidBack() {
    }

    public float QueryBlackPanelAlpha() {
        return 0.5f;
    }

    public CanvasGroup QueryBlackTargetPanel() {
        return canvas;
    }
}