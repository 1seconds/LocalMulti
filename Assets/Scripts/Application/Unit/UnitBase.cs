using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour {
    private Vector2 minVector = new Vector2(-17, -5);
    private Vector2 maxVector = new Vector2(17, 9);
    
    private void OnEnable() {
        Service.unit.selectedUnitUpdate += OnUpdateSelectedUnit;
    }

    private void OnDisable() {
        Service.unit.selectedUnitUpdate -= OnUpdateSelectedUnit;
    }

    private void OnUpdateSelectedUnit(Unit unit) {
    }
    
    protected Vector2 GetTargetVector(Vector3 mousePos) {
        var targetVector = new Vector3(mousePos.x, mousePos.y, 0);

        if (targetVector.x < minVector.x) {
            targetVector.x = minVector.x;
        }

        if (targetVector.y < minVector.y) {
            targetVector.y = minVector.y;
        }
        
        if (targetVector.x > maxVector.x) {
            targetVector.x = maxVector.x;
        }

        if (targetVector.y > maxVector.y) {
            targetVector.y = maxVector.y;
        }

        return targetVector;
    } 
}