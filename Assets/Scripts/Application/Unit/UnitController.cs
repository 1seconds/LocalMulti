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

    private Vector3 targetVector;
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
        /*if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
            if(hit.collider != null && hit.collider.transform == gameObject.transform) {
                Debug.LogError(gameObject.name);
            }
        }*/
        
        if (Input.GetMouseButtonDown(0)){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>().OverlapPoint(mousePosition)) {
                Service.unit.OnUpdateSelectedUnit(originUnit);
            }
        }
        
        if (Input.GetMouseButton(0)) {
            if (UnitService.originUnit != null) {
                if (UnitService.originUnit == originUnit) {
                    OnDraging();
                }
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
        while (true) {
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.MoveTowards(transform.position, targetVector, Time.deltaTime * speed);
        }
    }
    
    private void OnDraging() {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetVector = new Vector3(mousePos.x, mousePos.y, 0);
    }

    public void OnDead() {
        gameObject.SetActive(false);
    }

    public int GetUnitCode() {
        return code;
    }
}