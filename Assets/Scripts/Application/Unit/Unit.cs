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

public class Unit {
    public int code { get; set; }
    public UnitType unitType { get; set; }
    public JobType jobType { get; set; }
    public int level { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public int hp { get; set; }
    public int speed { get; set; }
    public int range { get; set; }

    public Unit(CsvRow row) {
        From(row);
    }

    private void From(CsvRow row) {
        code = row.NextInt();
        unitType = row.NextEnum<UnitType>();
        jobType = row.NextEnum<JobType>();
        level = row.NextInt();
        attack = row.NextInt();
        defense = row.NextInt();
        hp = row.NextInt();
        speed = row.NextInt();
        range = row.NextInt();
    }
}