using System.Collections.Generic;
using UnityEngine;

public enum UnitType {
    PLAYER = 0,
    ENEMY,
}

public enum JobType {
    NONE = 0,
    WARRIOR,
    HEALER,
    TANKER,
    ARCHER,
    MAGICIAN,
    KNIGHT
}

public class UnitLvUpProperty {
    public int code { get; set; }
    public string lvUpType1 { get; set; }
    public int lvUpValue1 { get; set; }
    public string lvUpType2 { get; set; }
    public int lvUpValue2 { get; set; }
    public string lvUpType3 { get; set; }
    public int lvUpValue3 { get; set; }
    
    public UnitLvUpProperty(CsvRow row) {
        From(row);
    }
    private void From(CsvRow row) {
        code = row.NextInt();
        lvUpType1 = row.NextString();
        lvUpValue1 = row.NextInt();
        lvUpType2 = row.NextString();
        lvUpValue2 = row.NextInt();
        lvUpType3 = row.NextString();
        lvUpValue3 = row.NextInt();
    }

    public int GetLvUpAttack(int level) {
        if (lvUpType1 == "attack") {
            return GetValue(1, level);
        }
        if (lvUpType2 == "attack") {
            return GetValue(2, level);
        }
        if (lvUpType3 == "attack") {
            return GetValue(3, level);
        }
        return 0;
    }
    public int GetLvUpDefense(int level) {
        if (lvUpType1 == "defense") {
            return GetValue(1, level);
        }
        if (lvUpType2 == "defense") {
            return GetValue(2, level);
        }
        if (lvUpType3 == "defense") {
            return GetValue(3, level);
        }
        return 0;
    }
    public int GetLvUpHp(int level) {
        if (lvUpType1 == "hp") {
            return GetValue(1, level);
        }
        if (lvUpType2 == "hp") {
            return GetValue(2, level);
        }
        if (lvUpType3 == "hp") {
            return GetValue(3, level);
        }
        return 0;
    }
    public int GetLvUpSpeed(int level) {
        if (lvUpType1 == "speed") {
            return GetValue(1, level);
        }
        if (lvUpType2 == "speed") {
            return GetValue(2, level);
        }
        if (lvUpType3 == "speed") {
            return GetValue(3, level);
        }
        return 0;
    }
    public int GetLvUpRange(int level) {
        if (lvUpType1 == "range") {
            return GetValue(1, level);
        }
        if (lvUpType2 == "range") {
            return GetValue(2, level);
        }
        if (lvUpType3 == "range") {
            return GetValue(3, level);
        }
        return 0;
    }

    private int GetValue(int index, int level) {
        if (index == 1) {
            return lvUpValue1 * level;
        }
        if (index == 2) {
            return lvUpValue2 * level;
        }
        if (index == 3) {
            return lvUpValue3 * level;
        }
        return -1;
    }
}

public class UnitProperty {
    public int code { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public int hp { get; set; }
    public int speed { get; set; }
    public int range { get; set; }

    public UnitProperty(CsvRow row) {
        From(row);
    }
    
    private void From(CsvRow row) {
        code = row.NextInt();
        attack = row.NextInt();
        defense = row.NextInt();
        hp = row.NextInt();
        speed = row.NextInt();
        range = row.NextInt();
    }
}

public class Unit {
    public int unitCode { get; set; }
    public int level { get; set; }
    public static int unitId { get; set; }
    public UnitType unitType { get; set; }
    public JobType jobType { get; set; }
    public UnitProperty property { get; set; }
    public List<Skill> skills { get; set; }

    public Unit(CsvRow row) {
        From(row);
    }

    private void From(CsvRow row) {
        unitCode = row.NextInt();
        unitType = row.NextEnum<UnitType>();
        jobType = row.NextEnum<JobType>();
        property = Service.rule.unitsProperty[unitCode];

        Service.setting.value.unitId += 1;
        unitId = Service.setting.value.unitId;
        Service.setting.Sync();
    }
}