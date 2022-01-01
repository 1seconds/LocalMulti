public enum StageType {
    None = 0,
    Normal,
    Reward,
    Boss
}

public class Stage {
    public int code { get; set; }
    public int index { get; set; }
    public StageType type { get; set; }
    

    public Stage(CsvRow row) {
        From(row);
    }

    private void From(CsvRow row) {
        code = row.NextInt();
        index = row.NextInt();
        type = row.NextEnum<StageType>();
    }
}