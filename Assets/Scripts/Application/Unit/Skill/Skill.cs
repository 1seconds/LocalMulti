public class Skill {
    public int skillCode { get; set; }
    
    public float power1 { get; set; }      //파워1
    public float power2 { get; set; }      //파워2
    public float castTime { get; set; }    //시전시간
    public float duration { get; set; }    //지속시간
    public float coolTime { get; set; }    //쿨타임
    public float remainCoolTime { get; set; }    //쿨타임 남은시간
    public string lvUpType1 { get; set; }
    public float lvUpValue1 { get; set; }
    public string lvUpType2 { get; set; }
    public float lvUpValue2 { get; set; }

    public Skill(CsvRow row) {
        From(row);
    }

    public void SetRemainCoolTime(float remainTime) {
        remainCoolTime = remainTime;
        if (remainCoolTime <= 0f) {
            remainCoolTime = 0f;
        }
    }

    private void From(CsvRow row) {
        skillCode = row.NextInt();
        
        power1 = row.NextFloat();
        power2 = row.NextFloat();
        castTime = row.NextFloat();
        duration = row.NextFloat();
        coolTime = row.NextFloat();
        
        lvUpType1 = row.NextString();
        lvUpValue1 = row.NextFloat();
        lvUpType2 = row.NextString();
        lvUpValue2 = row.NextFloat();

        remainCoolTime = 0f;
    }
}