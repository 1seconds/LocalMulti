using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PointingDirection {
    UpSide,
    DownSide,
    LeftSide
}

public class PointingLabel : GoItem {
    [SerializeField] private Text message;
    [SerializeField] private TextOutFitter textFitter;
    [SerializeField] private CanvasGroup panel;

    [Header ("PointingDir")]
    [SerializeField] private GameObject upsidePointing;
    [SerializeField] private GameObject downsidePointing;
    [SerializeField] private GameObject leftsidePointing;

    private bool display = false; 
    private RectTransform rect;
    private RectTransform target;
    private Vector2 offset;
    private PointingDirection dir;

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

    public async void Display(string text, float duration, RectTransform target, Vector2 offset, PointingDirection dir) {
        display = true; 
        this.gameObject.SetActive(true);
        this.target = target;
        this.offset = offset;
        this.dir = dir;
        SetParent(target);

        panel.alpha = 0;
        message.text = text;
        
        await new WaitForEndOfFrame();
        textFitter.RefreshSize();
        await new WaitForEndOfFrame();
        
        panel.alpha = 1;

        ArrangePosition();
        if (duration > 0) {
            Service.scene.Run(AutoHide(duration));
        }        
    }

    public void ArrangePosition() {
        var textSize = textFitter.GetSize();
        var targetSize = target.sizeDelta;
        var screenSize = Service.scene.GetSceneSize();
        Vector2 position = target.rect.center;
        position += offset;
        var rectTrans = textFitter.gameObject.GetComponent<RectTransform>();

        switch(dir) {
            case PointingDirection.UpSide:
                upsidePointing.SetActive(true);
                downsidePointing.SetActive(false);
                leftsidePointing.SetActive(false);
                rectTrans.pivot = new Vector2(rectTrans.pivot.x, 1);
                position.y -= targetSize.y * 0.5f + 16;
                break;
            case PointingDirection.DownSide:
                upsidePointing.SetActive(false);
                downsidePointing.SetActive(true);
                leftsidePointing.SetActive(false);
                rectTrans.pivot = new Vector2(rectTrans.pivot.x, 0);
                position.y += targetSize.y * 0.5f + 16;
                break;
            case PointingDirection.LeftSide:
                upsidePointing.SetActive(false);
                downsidePointing.SetActive(false);
                leftsidePointing.SetActive(true);
                rectTrans.pivot = new Vector2(rectTrans.pivot.x, 0.5f);
                position.x -= targetSize.x * 0.5f + 16;
                break;
        }

        var defaultUI = Service.scene.defaultUI.GetComponent<RectTransform>();
        var worldPos = target.localToWorldMatrix.MultiplyPoint(position);
        var pt = defaultUI.ToLocalPosition(worldPos);
        var textPos = Vector2.zero;

        switch(dir) {
            case PointingDirection.UpSide:
                textPos = new Vector2(0, -14);
                break;
            case PointingDirection.DownSide:
                textPos = new Vector2(0, 14);
                break;
            case PointingDirection.LeftSide:
                textPos = new Vector2(50, 0);
                break;
        }
        
        float lhs = pt.x - textSize.x / 2;
        float rhs = pt.x + textSize.x / 2;
        float leftMin = -screenSize.x * 0.5f + 32;
        float rightMax = screenSize.x * 0.5f - 32;

        float screenOffset = 0;
        if (lhs < leftMin) {
            screenOffset = Mathf.Abs(lhs - leftMin);
        }
        if (rhs > rightMax) {
            screenOffset = -Mathf.Abs(rhs - rightMax);
        }

        float minOffset = Mathf.Max(0, textSize.x * 0.5f - 40);
        if (screenOffset > minOffset) {
            screenOffset = minOffset;
        } else if (screenOffset < -minOffset) {
            screenOffset = -minOffset;
        }

        textPos.x += screenOffset; 
        rect.anchorMax = target.anchorMax;
        rect.anchorMin = target.anchorMin;
        rect.pivot = target.pivot;
        rect.anchoredPosition = position + target.anchoredPosition;

        textFitter.SetPosition(textPos);
        Transform trans;

        switch(dir) {
            case PointingDirection.UpSide:
                trans = upsidePointing.transform;
                trans.localPosition = new Vector3(trans.localPosition.x, 13, trans.localPosition.z);
                break;
            case PointingDirection.DownSide:
                trans = downsidePointing.transform;
                trans.localPosition = new Vector3(trans.localPosition.x, -13, trans.localPosition.z);
                break;
        }
    }

    public void Hide() {
        Service.goPooler.Return(this);
    }
    
    private void SetParent(RectTransform target) {
        var parent = target.transform.parent;
        var index = target.transform.GetSiblingIndex();

        if (transform.parent != parent) {
            transform.SetParent(parent);
        }

        transform.localScale = Vector3.one;
        transform.SetSiblingIndex(index + 1);
    }

    private IEnumerator AutoHide(float duration) {
        float past = 0;
        while (display) {
            past += Time.deltaTime;
            if (past > duration) {
                break;
            }
            yield return null;
        }
        Hide();
    }
}

