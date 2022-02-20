using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour {
    private void OnEnable() {
        Service.unit.selectedUnitUpdate += OnUpdateSelectedUnit;
    }

    private void OnDisable() {
        Service.unit.selectedUnitUpdate -= OnUpdateSelectedUnit;
    }

    private void OnUpdateSelectedUnit(Unit unit) {
    }
}