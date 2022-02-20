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

    public void OnClickSelectUnit() {
        
    }

    public void OnClickBook() {
        
    }

    public void OnClickMessage() {
        
    }

    public void OnClickShop() {

    }

    public void OnClickStart() {
        Service.scene.SwitchScene(SceneType.Stage);
    }
    
    //forTest
    public void OnClickInitStage() {
        
    }

    //forTest
    public void OnClickSelectStageStart() {
        Service.scene.SwitchScene(SceneType.Stage);
    }
}
