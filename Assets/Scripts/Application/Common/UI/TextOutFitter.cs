using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextOutFitter : MonoBehaviour {
    [SerializeField] private Text target;
    [SerializeField] private Vector2 padding;

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
        target.horizontalOverflow = HorizontalWrapMode.Overflow;
        target.verticalOverflow = VerticalWrapMode.Overflow;
    }

    public Vector2 SetText(string text) {
        target.text = text;
        return RefreshSize();
    }

    public void SetColor(Color color) {
        target.color = color;
    }

    public Vector2 RefreshSize() {
        if (target == null || rect == null) {
            return Vector2.zero;
        }

        target.rectTransform.sizeDelta = new Vector2(720, 400);
        var width = target.preferredWidth;
        var height = target.preferredHeight;
        var res = new Vector2(width + padding.x, height + padding.y);;
        rect.sizeDelta = res;
        target.rectTransform.sizeDelta = new Vector2(width + 0.001f, height);
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