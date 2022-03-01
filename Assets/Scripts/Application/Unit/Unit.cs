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
    public int unitCode { get; set; }
    public string lvUpType1 { get; set; }
    public float lvUpValue1 { get; set; }
    public string lvUpType2 { get; set; }
    public float lvUpValue2 { get; set; }
    public string lvUpType3 { get; set; }
    public float lvUpValue3 { get; set; }
    
    public UnitLvUpProperty(CsvRow row) {
        From(row);
    }
    private void From(CsvRow row) {
        unitCode = row.NextInt();
        lvUpType1 = row.NextString();
        lvUpValue1 = row.NextFloat();
        lvUpType2 = row.NextString();
        lvUpValue2 = row.NextFloat();
        lvUpType3 = row.NextString();
        lvUpValue3 = row.NextFloat();
    }

    public float GetLvUpAttack(int level) {
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
    public float GetLvUpDefense(int level) {
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
    public float GetLvUpHp(int level) {
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
    public float GetLvUpSpeed(int level) {
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
    public float GetLvUpRange(int level) {
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

    private float GetValue(int index, int level) {
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
    public int unitCode { get; set; }
    public float attack { get; set; }
    public float defense { get; set; }
    public float hp { get; set; }
    public float speed { get; set; }
    public float range { get; set; }

    public UnitProperty(CsvRow row) {
        From(row);
    }
    
    private void From(CsvRow row) {
        unitCode = row.NextInt();
        attack = row.NextFloat();
        defense = row.NextFloat();
        hp = row.NextFloat();
        speed = row.NextFloat();
        range = row.NextFloat();
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
        
        Service.setting.value.unitId += 1;
        unitId = Service.setting.value.unitId;
        Service.setting.Sync();
    }
}