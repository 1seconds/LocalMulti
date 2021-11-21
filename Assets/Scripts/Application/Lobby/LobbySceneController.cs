using System.Collections.Generic;
using UnityEngine;
using System;

public class LobbySceneController : SceneControllerBase {

    [SerializeField] private GameObject mainAnchor;
    [SerializeField] private GameObject alertAnchor;
    [SerializeField] private GameObject waitingAnchor;
    [SerializeField] private GameObject pushAnchor;
    [SerializeField] private BodyScroll _bodyScroll;
    [SerializeField] private List<TabButton> tabButtons;
    [SerializeField] private List<TabBase> tabs;

    public override SceneType sceneType => SceneType.Intro;
    public override GameObject defaultUI => mainAnchor;
    public override GameObject alertLayer => alertAnchor;
    public override GameObject waitingLayer => waitingAnchor;
    public override GameObject pushLayer => pushAnchor;
    public string currentTab { get; private set; }
    public BodyScroll bodyScroll => _bodyScroll;
    
    private void Awake() {
        if (InitController() == false) {
            return;
        }

        bodyScroll.onStartMoveTo += OnStartMoveTo;
        bodyScroll.onDrag += OnDrag;
        bodyScroll.onEndDrag += OnEndDrag;
    }

    public void Start() {
        ShowTab("Training", false);
    }

    public void ShowTab(string key, bool easing) {
        currentTab = key;
        for (int index = 0; index < tabButtons.Count; index++) {
            if (tabButtons[index].GetKey() == key) {
                currentTab = key;
                bodyScroll.ScrollTo(index, easing ? 0.4f : 0);
                break;
            }
        }
    }
    
    public void OnClickTabButton(string key) {
        ShowTab(key, true);
    }

    private void OnStartMoveTo(int no) {
        for (int index = 0; index < tabs.Count; index++) {
            var button = tabButtons[index];
            button.Show(index == no);
        }
    }

    private void OnDrag(int no) {
        foreach (var tab in tabs) {
            var diff = tab.index - no;
            tab.Show(-1 == diff || diff == 0);
        }
    }

    private void OnEndDrag(int no) {
        for (int index = 0; index < tabs.Count; index++) {
            var tab = tabs[index];

            if (no == index) {
                currentTab = tab.GetKey();
                tab.Show(true);
                tab.OnShowDone(true);
            } else {
                tab.Show(false);
                tab.OnShowDone(false);
            }   
        }
    }
}
