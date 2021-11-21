using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBox : MonoBehaviour {
    public void Display() {
        
    }

    public void OnClickStageScene() {
        Service.scene.SwitchScene(SceneType.Stage);
    }
}
