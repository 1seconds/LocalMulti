using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour {
    public Dictionary<int, Unit> myUnits;
    private SceneType currentScene;

    private void OnEnable() {
        Service.unit.selectedUnitUpdate += OnUpdateSelectedUnit;
    }

    private void OnDisable() {
        Service.unit.selectedUnitUpdate -= OnUpdateSelectedUnit;
    }

    private void OnUpdateSelectedUnit(JobType type) {
    }
    
    private void ReadyData() {
    }
    private void Start() {
        ReadyData();
    }
}