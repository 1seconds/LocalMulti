using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextOutFitterLimit : MonoBehaviour {
    [SerializeField] private Text target;
    [SerializeField] private Vector2 padding;
    [SerializeField] private float minWidth = 80f;
    [SerializeField] private float maxWidth = 360f;

    public RectTransform rect { get; private set; }
    private Box box;

    private void Awake() {
        rect = GetComponent<RectTransform>();
        var parent = transform.parent;
        while (parent != null) {
            box = transform.parent.GetComponent<Box>();
            if (box != null) {
                break;
            }

            parent = parent.parent;
        }

        target.horizontalOverflow = HorizontalWrapMode.Wrap;
        target.verticalOverflow = VerticalWrapMode.Overflow;
    }

    public Vector2 SetText(string text) {
        target.text = text;
        return RefreshSize();
    }

    public Vector2 GetPadding() {
        return padding;
    }

    public void SetColor(Color color) {
        target.color = color;
    }

    public Vector2 RefreshSize() {
        if (target == null || rect == null) {
            return Vector2.zero;
        }

        target.rectTransform.sizeDelta = new Vector2(720, 400);
        var width = Mathf.Max(minWidth, target.preferredWidth);
        var height = target.preferredHeight;
        
        if (maxWidth < width) {
            target.rectTransform.sizeDelta = new Vector2(maxWidth, height);
            width = maxWidth;
            height = target.preferredHeight;
        }

        var res = new Vector2(width + 0.001f, height + 0.001f);
        target.rectTransform.sizeDelta = res;
        rect.sizeDelta = new Vector2(width + padding.x, height + padding.y);

        if (box != null) {
            box.Arrange();
        }

        return res;
    }

    public Vector2 GetSize() {
        return rect.sizeDelta;
    }

    public void SetPosition(Vector2 pos) {
        rect.anchoredPosition = pos;
    }
    
#if UNITY_EDITOR
    private void Update() {
        if (!Application.isPlaying) {
            RefreshSize();
        }
    }
#endif
}