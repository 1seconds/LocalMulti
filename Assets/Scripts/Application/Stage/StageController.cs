using System;
using UnityEngine;

public class StageController : MonoBehaviour {
    private void OnEnable() {
        Service.stage.selectedStageUpdate += OnUpdateSelectedStage;
    }

    private void OnDisable() {
        Service.stage.selectedStageUpdate -= OnUpdateSelectedStage;
    }

    private void OnUpdateSelectedStage(int index) {
        
    }
    
    private void ReadyData() {
        
    }
    private void Start() {
        ReadyData();
    }
}