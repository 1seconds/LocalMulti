using System;
using UnityEngine;

public class UnitController : UnitBase {
    private Unit originUnit;
    private Unit targetUnit;

    public int code;
    //private int level;
    private int uid { get; set; }
    private UnitType unitType { get; set; }
    private JobType jobType { get; set; }

    private int attack { get; set; }
    private int defense { get; set; }
    private int hp { get; set; }
    private int speed { get; set; }
    private int range { get; set; }

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
        //speed = unitBaseRule.property.speed + unitLvUpPropertyRule.GetLvUpSpeed() * level;
        //range = unitBaseRule.property.range + unitLvUpPropertyRule.GetLvUpRange() * level;
        
        //Debug.LogError("code : " + code + ", level : " + level + ", uid : " + uid + ", unitType : " + unitType + ", jobType : " + jobType +
        //               ", attack : " + attack + ", defense : " + defense + ", hp : " + hp + ", speed : " + speed + ", range : " + range);
    }

    public void Display(Unit unit) {
        gameObject.SetActive(true);
        originUnit = unit;
        code = unit.code;
        
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
            if(GetComponent<Collider2D>().OverlapPoint(mousePosition)) {
                Debug.LogError(gameObject.name);
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