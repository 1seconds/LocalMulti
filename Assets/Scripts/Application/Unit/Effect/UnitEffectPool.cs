using System.Collections.Generic;
using UnityEngine;

public class UnitEffectPool : MonoBehaviour {
    [SerializeField] private List<UnitEffectBase> unitEffects;

    private void OnEnable() {
        Service.unit.selectedUnitUpdate += ShowSelectEffect;
        Service.unit.selectedUnitsUpdate += ShowInteractionEffect;
        Service.unit.selectedUnitMovePointUpdate += ShowMoveEffect;
        Service.skill.skillNoneTargetUpdate += ShowNoneTargetSkillEffect;
        Service.skill.skillTargetUpdate += ShowTargetSkillEffect;
        
    }

    private void OnDisable() {
        Service.unit.selectedUnitUpdate -= ShowSelectEffect;
        Service.unit.selectedUnitsUpdate -= ShowInteractionEffect;
        Service.unit.selectedUnitMovePointUpdate -= ShowMoveEffect;
        Service.skill.skillNoneTargetUpdate -= ShowNoneTargetSkillEffect;
        Service.skill.skillTargetUpdate -= ShowTargetSkillEffect;
    }

    private void ShowSelectEffect(Unit unit) {
        if (unit != null) {
            unitEffects[0].OnEffect(unit);
        } else {
            unitEffects[0].OffEffect();
        }
    }
    
    private void ShowInteractionEffect(Unit origin, Unit target) {
        if (origin != null && target != null) {
            unitEffects[1].OnEffect(origin, target);
        }
    }
    
    private void ShowMoveEffect(Unit origin, Vector2 targetPoint) {
        if (origin != null && targetPoint != null) {
            unitEffects[2].OnEffect(origin, targetPoint);
        }
    }
    
    private void ShowNoneTargetSkillEffect(Unit unit, Skill skill) {
        if (unit != null && skill != null) {
            unitEffects[3].OnEffect(unit, skill);
        }
    }
    
    private void ShowTargetSkillEffect(Unit origin, Unit target, Skill skill) {
        if (origin != null && target != null && skill != null) {
            unitEffects[4].OnEffect(origin, target, skill);
        }
    }
}
