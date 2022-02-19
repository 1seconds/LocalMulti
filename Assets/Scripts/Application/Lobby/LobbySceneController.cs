using System.Collections.Generic;
using UnityEngine;
using System;

public class LobbySceneController : SceneControllerBase {

    [SerializeField] private GameObject mainAnchor;

    public override SceneType sceneType => SceneType.Intro;
    public override GameObject defaultUI => mainAnchor;

    private void Awake() {
        if (InitController() == false) {
            return;
        }
    }
}
