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

public class UnitProperty {
    public int code { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public int hp { get; set; }
    public int speed { get; set; }
    public int range { get; set; }

    public static UnitProperty Build(int code) {
        var text = PersistenceUtil.LoadTextResource("Rules/unitProperty");
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);

            var res = new UnitProperty();
            //var rule = new Unit(row);//infinite loop occur
            //if (rule.code == code) {
                res.code = row.NextInt();
                res.attack = row.NextInt();
                res.defense = row.NextInt();
                res.hp = row.NextInt();
                res.speed = row.NextInt();
                res.range = row.NextInt();

                return res;
            //}
        }
        return null;
    }

    public UnitProperty() {
    }
    
    public UnitProperty(CsvRow row) {
        LvUpPropertyFrom(row);
    }

    private void LvUpPropertyFrom(CsvRow row) {        //todo bug fix.
        code = row.NextInt();
        if (row.NextString().Equals("attack")) {
            attack += row.NextInt();
        } else if (row.NextString().Equals("defense")) {
            defense += row.NextInt();
        } else if (row.NextString().Equals("hp")) {
            hp += row.NextInt();
        } else if (row.NextString().Equals("speed")) {
            speed += row.NextInt();
        } else if (row.NextString().Equals("range")) {
            range += row.NextInt();
        } else if (row.NextString().Equals("none")) {
            return;
        } 
        
        if (row.NextString().Equals("attack")) {
            attack += row.NextInt();
        } else if (row.NextString().Equals("defense")) {
            defense += row.NextInt();
        } else if (row.NextString().Equals("hp")) {
            hp += row.NextInt();
        } else if (row.NextString().Equals("speed")) {
            speed += row.NextInt();
        } else if (row.NextString().Equals("range")) {
            range += row.NextInt();
        } else if (row.NextString().Equals("none")) {
            return;
        } 
        
        if (row.NextString().Equals("attack")) {
            attack += row.NextInt();
        } else if (row.NextString().Equals("defense")) {
            defense += row.NextInt();
        } else if (row.NextString().Equals("hp")) {
            hp += row.NextInt();
        } else if (row.NextString().Equals("speed")) {
            speed += row.NextInt();
        } else if (row.NextString().Equals("range")) {
            range += row.NextInt();
        }
    }
}

public class Unit {
    public int code { get; set; }
    public static int uid { get; set; }
    public UnitType unitType { get; set; }
    public JobType jobType { get; set; }
    public UnitProperty property { get; set; }


    public Unit(CsvRow row) {
        BaseFrom(row);
    }

    private void BaseFrom(CsvRow row) {
        code = row.NextInt();
        unitType = row.NextEnum<UnitType>();
        jobType = row.NextEnum<JobType>();
        property = UnitProperty.Build(code);    //todo bugfix.

        Service.setting.value.unitIndex += 1;
        uid = Service.setting.value.unitIndex;
        Service.setting.Sync();
    }
}