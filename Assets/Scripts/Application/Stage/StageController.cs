using System;
using UnityEngine;

public class StageController : MonoBehaviour {
    [SerializeField] private UnitSkillController skillController;
    
    private void OnEnable() {
        Service.stage.selectedStageUpdate += OnUpdateSelectedStage;
        Service.unit.selectedUnitUpdate += OnUpdateUnit;
    }

    private void OnDisable() {
        Service.stage.selectedStageUpdate -= OnUpdateSelectedStage;
        Service.unit.selectedUnitUpdate -= OnUpdateUnit;
    }

    private void OnUpdateSelectedStage(int index) {
        
    }

    private void OnUpdateUnit(Unit unit) {
        skillController.gameObject.SetActive(unit != null);
        if (unit == null) {
            return;
        }
        
        if (unit.unitType == UnitType.PLAYER) {
            skillController.Display(unit);
        }
    }
    
    private void ReadyData() {
        skillController.gameObject.SetActive(false);
    }
    private void Start() {
        ReadyData();
    }
}