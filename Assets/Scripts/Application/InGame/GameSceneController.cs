using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : SceneControllerBase, GameSceneService {
    [SerializeField] private GameObject mainAnchor;

    public override SceneType sceneType => SceneType.Intro;
    public override GameObject defaultUI => mainAnchor;

    private void Awake() {
        if (InitController() == false) {
            return;
        }
    }

    public void OnClickBack() {
        Service.scene.SwitchScene(SceneType.Lobby);
    }
}