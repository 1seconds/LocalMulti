using UnityEngine;

public class UnitController : UnitBase {
    public int code;
    public int level;
    private int uid { get; set; }
    private UnitType unitType { get; set; }
    private JobType jobType { get; set; }

    private int attack { get; set; }
    private int defense { get; set; }
    private int hp { get; set; }
    private int speed { get; set; }
    private int range { get; set; }

    private void Awake() {
        var unitBaseRule = Service.rule.units[code];
        var unitLvUpPropertyRule = Service.rule.unitsLvUpProperty[code];

        uid = Unit.uid;
        unitType = unitBaseRule.unitType;
        jobType = unitBaseRule.jobType;
        
        attack = unitBaseRule.property.attack + unitLvUpPropertyRule.attack * level;
        defense = unitBaseRule.property.defense + unitLvUpPropertyRule.defense * level;
        hp = unitBaseRule.property.hp + unitLvUpPropertyRule.attack * hp;
        speed = unitBaseRule.property.speed + unitLvUpPropertyRule.attack * speed;
        range = unitBaseRule.property.range + unitLvUpPropertyRule.attack * range;
        
        Debug.LogError("code : " + code + ", level : " + level + ", uid : " + uid + ", unitType : " + unitType + ", jobType : " + jobType +
                       ", attack : " + attack + ", defense : " + defense + ", hp : " + hp + ", speed : " + speed + ", range : " + range);
    }

    public void Display() {
        gameObject.SetActive(true);
    }

    public void OnDead() {
        gameObject.SetActive(false);
    }

    public int GetUnitCode() {
        return this.code;
    }
}