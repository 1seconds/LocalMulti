using UnityEngine;
using UnityEngine.UI;

public class UnitSkillItem : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private UnitSkillLockItem lockItem;
    
    private Skill skill;

    public void Display(Skill skill, Sprite icon) {
        this.icon.sprite = icon;
        this.skill = skill;
        
        var active = (skill.remainCoolTime > 0f);
        if (active) {
            lockItem.Display(skill);
        } else {
            lockItem.Hide();
        }
    }

    public void OnClickSkill() {
        if (skill.remainCoolTime > 0f) {
            return;
        }
        
        skill.SetRemainCoolTime(skill.coolTime);
        lockItem.Display(skill);
    }
}