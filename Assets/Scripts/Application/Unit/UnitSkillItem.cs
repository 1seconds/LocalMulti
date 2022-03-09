using UnityEngine;
using UnityEngine.UI;

public class UnitSkillItem : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private UnitSkillLockItem lockItem;
    
    private Skill skill;
    private Unit unit;
    
    public Coroutine unit1SkillRoutine;
    public Coroutine unit2SkillRoutine;
    public Coroutine unit3SkillRoutine;
    public Coroutine unit4SkillRoutine;

    public void Display(Unit unit, Skill skill, Sprite icon) {
        this.icon.sprite = icon;
        this.skill = skill;
        this.unit = unit;
        
        var active = (skill.remainCoolTime > 0f);
        if (active) {
            lockItem.Display(skill);
        } else {
            lockItem.Hide();
        }
        StartTimerRoutine();
    }

    public void OnClickSkill() {
        if (skill.remainCoolTime > 0f) {
            return;
        }
        //
        Service.skill.OnUpdateNoneTargetSkill(unit, skill);
        for (int i = 0; i < skill.targets.Count; i++) {
            Service.skill.OnUpdateTargetSkill(unit, skill.targets[i], skill);
        }
        
        skill.SetRemainCoolTime(skill.coolTime);
        lockItem.Display(skill);
        StartTimerRoutine();
    }

    private void StartTimerRoutine() {
        if (unit.unitIndex == 0) {
            StartCoroutine(Service.skill.Unit1SkillRoutine(unit));
        } else if (unit.unitIndex == 1) {
            StartCoroutine(Service.skill.Unit2SkillRoutine(unit));
        } else if (unit.unitIndex == 2) {
            StartCoroutine(Service.skill.Unit3SkillRoutine(unit));
        } else if (unit.unitIndex == 3) {
            StartCoroutine(Service.skill.Unit4SkillRoutine(unit));
        } else {
            Debug.LogError("Error : " + unit.unitIndex);
        }
    }
}