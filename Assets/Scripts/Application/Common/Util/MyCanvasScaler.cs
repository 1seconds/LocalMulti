using UnityEngine;

[ExecuteInEditMode]
public class MyCanvasScaler : MonoBehaviour {
    [SerializeField] private Canvas canvas; 
    [SerializeField] private RectTransform defaultUI;
    [SerializeField] private float width = 720; 
    [SerializeField] private float minHeight = 1280; 
    [SerializeField] private float maxHeight = 1400;

    private Vector2 size;

    private void Awake() {
        Adjust();
    } 

    private void Start() {
        if (Application.isPlaying) {
            this.RunNextFrame(() => Adjust());
        }
    }

    private void Adjust() {
        if (canvas == null || defaultUI == null) {
            return;
        }

        float scaler = Screen.width / width;
        scaler = Mathf.Min(scaler, Screen.height / minHeight);
        canvas.scaleFactor = scaler;

        float height = width * (Screen.height / (float)Screen.width);
        if (height < minHeight) {
            height = minHeight;
        }

        height = Mathf.Min(maxHeight, height);
        size = new Vector2(width, height);
        defaultUI.anchoredPosition = Vector3.zero;
        defaultUI.sizeDelta = size;

        AdjustOffset(size, scaler);
    }

    private void AdjustOffset(Vector2 size, float scaler) {
#if UNITY_IOS
        return;
#endif
        float height = width * (Screen.height / (float)Screen.width);
        if (height < minHeight) {
            height = minHeight;
        }

        var cutouts = Screen.cutouts;
        var maxHole = 0f;
        foreach (var cutout in cutouts) {
            var hole = cutout.height / scaler;
            maxHole = Mathf.Max(hole, maxHole);
        }

        var available = height - maxHole;
        var diff = Mathf.Max(0, size.y - available);
        var offset = Vector2.zero;

        if (diff > 0) {
            size.y -= diff;
            offset.y -= diff * 0.5f;

            defaultUI.sizeDelta = size;
            defaultUI.anchoredPosition = offset;
        }
    }

    public Vector2 GetSize() {
        if (size.x <= 0) {
            Adjust();
        }

        return size;
    } 

    #if UNITY_EDITOR
    private void Update() {
        Adjust();
    }
    #endif
}
