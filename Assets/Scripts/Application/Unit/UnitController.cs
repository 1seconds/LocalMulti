public class UnitController : UnitBase {
    public int code;
    private int uid { get; set; }
    private UnitType unitType { get; set; }
    private JobType jobType { get; set; }
    private int level { get; set; }
    private int attack { get; set; }
    private int defense { get; set; }
    private int hp { get; set; }
    private int speed { get; set; }
    private int range { get; set; }

    private void Awake() {
        var rule = Unit.Build(code);
        
        uid = rule.uid;
        unitType = rule.unitType;
        jobType = rule.jobType;
        level = rule.level;
        attack = rule.attack;
        defense = rule.defense;
        hp = rule.hp;
        speed = rule.speed;
        range = rule.range;
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