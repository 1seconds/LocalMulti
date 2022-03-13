using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour {
    protected Unit originUnit;
    protected Unit targetUnit;

    public int code;
    protected int unitId;
    [HideInInspector] public int level;
    protected UnitType unitType { get; set; }
    protected JobType jobType { get; set; }

    protected float attack { get; set; }
    protected float defense { get; set; }
    protected float hp { get; set; }
    protected float speed { get; set; }
    protected float range { get; set; }
    
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
    
    protected bool IsMouseOver(Vector3 mousePos) {
        var targetVector = new Vector3(mousePos.x, mousePos.y, 0);

        if (targetVector.x < minVector.x) {
            return true;
        }

        if (targetVector.y < minVector.y) {
            return true;
        }
        
        if (targetVector.x > maxVector.x) {
            return true;
        }

        if (targetVector.y > maxVector.y) {
            return true;
        }
        return false;
    }
    
    public int GetUnitCode() {
        return code;
    }

    public int GetUnitId() {
        return unitId;
    }
    
    public Unit GetUnit() {
        if (originUnit != null) {
            return originUnit;
        } return null;
    }
}