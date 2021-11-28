using System.Collections.Generic;
using UnityEngine;
using System;

public class LobbySceneController : SceneControllerBase {

    [SerializeField] private GameObject mainAnchor;
    [SerializeField] private GameObject alertAnchor;
    [SerializeField] private GameObject waitingAnchor;
    [SerializeField] private GameObject pushAnchor;

    public override SceneType sceneType => SceneType.Intro;
    public override GameObject defaultUI => mainAnchor;
    public override GameObject alertLayer => alertAnchor;
    public override GameObject waitingLayer => waitingAnchor;
    public override GameObject pushLayer => pushAnchor;

    private void Awake() {
        if (InitController() == false) {
            return;
        }
    }

    public void Start() {
    }
}
