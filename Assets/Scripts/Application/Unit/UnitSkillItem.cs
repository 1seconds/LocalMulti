using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillItem : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private UnitSkillLockItem lockItem;
    private Skill skill;
    
    public void Display(Skill skill, Sprite icon) {
        this.icon.sprite = icon;
        this.skill = skill;
    }

    public void OnClickSkill() {
        
    }
}