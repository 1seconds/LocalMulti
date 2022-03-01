using System.Collections.Generic;
using UnityEngine;

public class UnitSkillController : MonoBehaviour {
    [SerializeField] private List<UnitSkillItem> skillItems;

    [SerializeField] private List<Sprite> skillIcons;
    private Dictionary<int, Sprite> skills;

    private void ReadyData() {
        skills = new Dictionary<int, Sprite>();
        skills.Add(100, skillIcons[0]);
        skills.Add(101, skillIcons[1]);
        skills.Add(102, skillIcons[2]);
        skills.Add(103, skillIcons[3]);
        
        skills.Add(200, skillIcons[4]);
        skills.Add(201, skillIcons[5]);
        skills.Add(202, skillIcons[6]);
        skills.Add(203, skillIcons[7]);
        
        skills.Add(300, skillIcons[8]);
        skills.Add(301, skillIcons[9]);
        skills.Add(302, skillIcons[10]);
        skills.Add(303, skillIcons[11]);
        
        skills.Add(400, skillIcons[12]);
        skills.Add(401, skillIcons[13]);
        skills.Add(402, skillIcons[14]);
        skills.Add(403, skillIcons[15]);
        
        skills.Add(500, skillIcons[16]);
        skills.Add(501, skillIcons[17]);
        skills.Add(502, skillIcons[18]);
        skills.Add(503, skillIcons[19]);
        
        skills.Add(600, skillIcons[20]);
        skills.Add(601, skillIcons[21]);
        skills.Add(602, skillIcons[22]);
        skills.Add(603, skillIcons[23]);
    }
    
    public void Display(Unit unit) {
        ReadyData();
        
        for (int i = 0; i < skillItems.Count; i++) {
            skillItems[i].Display(Service.rule.skills[unit.unitCode + i], skills[unit.unitCode + i]);
        }
    }
}