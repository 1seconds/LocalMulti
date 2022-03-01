using System;
using System.Collections;
using UnityEngine;

public class UnitController : UnitBase {
    private Unit originUnit;
    private Unit targetUnit;

    public int code;
    //private int level;
    private int uid { get; set; }
    private UnitType unitType { get; set; }
    private JobType jobType { get; set; }

    private float attack { get; set; }
    private float defense { get; set; }
    private float hp { get; set; }
    private float speed { get; set; }
    private float range { get; set; }

    private Coroutine moveRoutine;

    private void Start() {
        ReadyData();
    }

    private void ReadyData() {
        //var unitBaseRule = Service.rule.units[code];
        //var unitLvUpPropertyRule = Service.rule.unitsLvUpProperty[code];

        //uid = Unit.uid;
        //unitType = unitBaseRule.unitType;
        //jobType = unitBaseRule.jobType;
        
        //attack = unitBaseRule.property.attack + unitLvUpPropertyRule.GetLvUpAttack() * level;
        //defense = unitBaseRule.property.defense + unitLvUpPropertyRule.GetLvUpDefense() * level;
        //hp = unitBaseRule.property.hp + unitLvUpPropertyRule.GetLvUpHp() * level;
        speed = Service.rule.unitsProperties[code].speed; /*+ unitLvUpPropertyRule.GetLvUpSpeed() * level*/
        //range = unitBaseRule.property.range + unitLvUpPropertyRule.GetLvUpRange() * level;
        
        //Debug.LogError("code : " + code + ", level : " + level + ", uid : " + uid + ", unitType : " + unitType + ", jobType : " + jobType +
        //               ", attack : " + attack + ", defense : " + defense + ", hp : " + hp + ", speed : " + speed + ", range : " + range);
    }

    public void Display(Unit unit) {
        gameObject.SetActive(true);
        originUnit = unit;
        code = unit.unitCode;
        
        ReadyData();
    }

    public void Display(EnemyStageUnit unit) {
        gameObject.SetActive(true);
        originUnit = Service.rule.units[unit.code];
        code = unit.code;
        
        ReadyData();
    }

    //pick one way
    public void Update() {
        if (Input.GetMouseButtonDown(0)){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>().OverlapPoint(mousePosition)) {
                Service.unit.OnUpdateSelectedUnit(originUnit);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (UnitService.originUnit != null) {
                if (UnitService.originUnit == originUnit) {
                    UnitService.originUnit = null;

                    if (moveRoutine != null) {
                        StopCoroutine(moveRoutine);
                    }
                    moveRoutine = StartCoroutine(MoveRoutine());
                }
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