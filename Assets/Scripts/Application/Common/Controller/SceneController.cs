using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public enum SceneType {
    Intro,
    Lobby,
    Stage,
    
}

public interface SceneController {
    SceneType sceneType { get; }
    Canvas canvas { get; }
    GameObject defaultUI { get; }
    HashSet<IPopupPanel> popups { get; }
    CommonAlertBox currentCommonAlert { get; set; }

    Vector2 GetSceneSize();
    
    Task SwitchScene(SceneType scene, SceneTransitionEffect effect = null);
    Task SwitchPrevScene(SceneTransitionEffect effect = null);
    Task SwitchSceneWithHistoryClear(SceneType scene, SceneTransitionEffect effect = null);

    Coroutine Run(IEnumerator iterationResult);
    void RunOnce(IEnumerator method);
    void Stop(Coroutine coroutine);
    void Stop(IEnumerator method);
    void ShowBlackPanel(BlackPanelObserver ob);
    void HideBlackPanel(BlackPanelObserver ob);

    void RunAfter(float delay, Action action);
    void AddPopup(IPopupPanel panel);
    void RemovePopup(IPopupPanel panel);
    void HideAllPopups();
    bool HasPopup();
}
