using System;
using System.Collections;
using UnityEngine;

public class UnitController : UnitBase {
    private Unit originUnit;
    private Unit targetUnit;

    public int code;
    [HideInInspector] public int level;
    private int uid { get; set; }
    private UnitType unitType { get; set; }
    private JobType jobType { get; set; }

    private float attack { get; set; }
    private float defense { get; set; }
    private float hp { get; set; }
    private float speed { get; set; }
    private float range { get; set; }
    
    private Coroutine moveRoutine;
    private Coroutine interactionRoutine;

    private void Start() {
        ReadyData();
    }

    private void ReadyData() {
        targetUnit = null;
        level = Service.rule.units[code].level;
        speed = Service.rule.unitsProperties[code].speed + Service.rule.unitsLvUpProperties[code].GetLvUpSpeed(level);
        range = Service.rule.unitsProperties[code].range + Service.rule.unitsLvUpProperties[code].GetLvUpRange(level);
        unitType = Service.rule.units[code].unitType;
    }

    public void Display(Unit unit) {
        gameObject.SetActive(true);
        originUnit = unit;
        code = unit.unitCode;
        
        ReadyData();
    }

    public Unit GetUnit() {
        if (originUnit != null) {
            return originUnit;
        } return null;
    }

    public void Display(EnemyStageUnit unit) {
        gameObject.SetActive(true);
        originUnit = Service.rule.units[unit.code];
        code = unit.code;
        
        ReadyData();
    }

    public void Update() {
        if (unitType == UnitType.ENEMY) {
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) {
            var isMouseOver = IsMouseOver(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (isMouseOver) {
                UnitService.originUnit = null;
                return;
            }
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>().OverlapPoint(mousePosition)) {
                Service.unit.OnUpdateSelectedUnit(originUnit);
            } else if (originUnit == UnitService.originUnit) {
                Service.unit.OnUpdateSelectedUnit(null);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (UnitService.originUnit == GetUnit()) {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Physics2D.OverlapPoint(mousePosition) != null) {
                    targetUnit = Physics2D.OverlapPoint(mousePosition).GetComponent<UnitController>().GetUnit();
                    if (targetUnit != null && targetUnit != originUnit) {
                        Service.unit.OnUpdateSelectedUnits(originUnit, targetUnit);
                    }
                } else {
                    targetUnit = null;
                }

                if (UnitService.originUnit == originUnit) {
                    if (targetUnit != originUnit) {
                        if (targetUnit != null) {
                            if (interactionRoutine != null) {
                                StopCoroutine(interactionRoutine);
                            }
                            interactionRoutine = StartCoroutine(InteractionRoutine());
                        } else {
                            if (moveRoutine != null) {
                                StopCoroutine(moveRoutine);
                            }
                            moveRoutine = StartCoroutine(MoveRoutine());
                        }
                    }
                }
            }
        }
    }

    IEnumerator InteractionRoutine() {
        var targetVector = GetTargetVector(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        while (true) {
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.MoveTowards(transform.position, targetVector, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, targetVector) <= range) {
                break;
            }
        }
    }
    
    IEnumerator MoveRoutine() {
        var targetVector = GetTargetVector(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        while (true) {
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.MoveTowards(transform.position, targetVector, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, targetVector) < Vector3.kEpsilon) {
                break;
            }
        }
    }

    public void OnDead() {
        gameObject.SetActive(false);
    }

    public int GetUnitCode() {
        return code;
    }
}