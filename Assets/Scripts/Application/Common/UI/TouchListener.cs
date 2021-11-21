using System;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void EventTouch(TouchInfo info);

public class TouchInfo {
    public Vector3 worldPos { get; set; }
    public Vector2 screenPos { get; set; }
}

public class DragHandler {
    public Action<TouchInfo> onDrag { get; }
    public Action<TouchInfo> onEndDrag { get; }

    public DragHandler(Action<TouchInfo> onDrag, Action<TouchInfo> onEndDrag) {
        this.onDrag = onDrag;
        this.onEndDrag = onEndDrag;
    }
}

public class TouchListener : MonoBehaviour {
    [SerializeField] private EventTrigger eventTrigger;

    public event EventTouch onPointerDown;
    public event EventTouch onPointerUp;
    public event EventTouch onBeginDrag;

    private DragHandler dragHandler;
    private TouchInfo _touchInfo;
    
    private void Start() {
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener((data) => {
            OnBeginDrag((PointerEventData)data);
        });

        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((data) => {
            OnDrag((PointerEventData)data);
        });

        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener((data) => {
            OnEndDrag((PointerEventData)data);
        });
        
        EventTrigger.Entry down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener((data) => {
            OnPointerDown((PointerEventData)data);
        });
        
        EventTrigger.Entry up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener((data) => {
            OnPointerUp((PointerEventData)data);
        });
        
        eventTrigger.triggers.Add(beginDrag);
        eventTrigger.triggers.Add(drag);
        eventTrigger.triggers.Add(endDrag);
        eventTrigger.triggers.Add(down);
        eventTrigger.triggers.Add(up);
    }

    public bool CaptureDragEvent(Action<TouchInfo> onDrag, Action<TouchInfo> onEndDrag) {
        if (dragHandler == null) {
            dragHandler = new DragHandler(onDrag, onEndDrag);
            return true;
        }

        return false;
    }
    
    public void OnBeginDrag(PointerEventData data) {
        _touchInfo = new TouchInfo();
        _touchInfo.worldPos = GetWorldPosition(data);
        _touchInfo.screenPos = data.position;
        onBeginDrag?.Invoke(_touchInfo);
    }

    public void OnDrag(PointerEventData data) {
        _touchInfo.worldPos = GetWorldPosition(data);
        _touchInfo.screenPos = data.position;
        dragHandler?.onDrag?.Invoke(_touchInfo);
    }

    public void OnEndDrag(PointerEventData data) {
        _touchInfo.worldPos = GetWorldPosition(data);
        _touchInfo.screenPos = data.position;
        dragHandler?.onEndDrag?.Invoke(_touchInfo);
        dragHandler = null;
    }
    
    public void OnPointerDown(PointerEventData data) {
        var info = new TouchInfo();
        info.worldPos = GetWorldPosition(data);
        info.screenPos = data.position;
        onPointerDown?.Invoke(info);
    }
    
    public void OnPointerUp(PointerEventData data) {
        var info = new TouchInfo();
        info.worldPos = GetWorldPosition(data);
        info.screenPos = data.position;
        onPointerUp?.Invoke(info);
    }
    
    private Vector3 GetWorldPosition(PointerEventData data) {
        return data.pointerCurrentRaycast.worldPosition;
    }
}