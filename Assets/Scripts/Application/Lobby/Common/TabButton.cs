using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour {
    [SerializeField] private string key = "";
    [SerializeField] private Text title;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inActiveColor;

    private bool initialized;
    private bool isOn;

    private Coroutine animRoutine;

    protected virtual void OnShow(bool onOff) {
    }
	
    public void Show(bool onOff) {
        if (isOn == onOff && initialized) {
            return;
        }

        OnShow(onOff);
        initialized = true;
        title.color = onOff ? activeColor : inActiveColor;
        isOn = onOff;
    }

    private string GetSkinName(bool isOn) {
        var animName = "action";
        if (GetKey() == "Training") {
            animName += isOn ? "1_a" : "1_b";
        } else if (GetKey() == "All") {
            animName += isOn ? "2_a" : "2_b";
        } else if (GetKey() == "Score") {
            animName += isOn ? "3_a" : "3_b";
        } else if (GetKey() == "Dr") {
            animName += isOn ? "4_a" : "4_b";
        }
        return animName;
    }

    public string GetKey() {
        return key;
    }
}