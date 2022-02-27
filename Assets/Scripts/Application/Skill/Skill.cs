public class Skill {
    public int skillCode { get; set; }
    
    public int power1 { get; set; }      //파워1
    public int power2 { get; set; }      //파워2
    public int castTime { get; set; }    //시전시간
    public int duration { get; set; }    //지속시간
    public int coolTime { get; set; }    //쿨타임
    
    public string lvUpType1 { get; set; }
    public int lvUpValue1 { get; set; }
    public string lvUpType2 { get; set; }
    public int lvUpValue2 { get; set; }

    public Skill(CsvRow row) {
        From(row);
    }

    private void From(CsvRow row) {
        skillCode = row.NextInt();
        
        power1 = row.NextInt();
        power2 = row.NextInt();
        castTime = row.NextInt();
        duration = row.NextInt();
        coolTime = row.NextInt();
        
        lvUpType1 = row.NextString();
        lvUpValue1 = row.NextInt();
        lvUpType2 = row.NextString();
        lvUpValue2 = row.NextInt();
    }
}