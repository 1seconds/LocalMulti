using System.Collections.Generic;
using UnityEngine;

public class UnitSkillController : MonoBehaviour {
    [SerializeField] private List<UnitSkillItem> skillItems;
    
    private void ReadyData() {
        
    }
    
    public void Display(Unit unit) {
        ReadyData();

        for (int i = 0; i < skillItems.Count; i++) {
            skillItems[i].Display(i);
        }
    }
}