using System.Collections.Generic;
using UnityEngine;

public class UnitEffectPool : MonoBehaviour {
    [SerializeField] private List<UnitEffectBase> unitEffects;

    private void OnEnable() {
        Service.unit.selectedUnitUpdate += Display;
        Service.unit.selectedUnitsUpdate += Display;
        Service.skill.skillNoneTargetUpdate += Display;
        Service.skill.skillTargetUpdate += Display;
    }

    private void OnDisable() {
        Service.unit.selectedUnitUpdate -= Display;
        Service.unit.selectedUnitsUpdate -= Display;
        Service.skill.skillNoneTargetUpdate -= Display;
        Service.skill.skillTargetUpdate -= Display;
    }

    private void Display(Unit unit) {
        if (unit != null) {
            unitEffects[0].Display(unit);
        }
    }
    
    private void Display(Unit origin, Unit target) {
        if (origin != null && target != null) {
            unitEffects[1].Display(origin, target);
        }
    }
    
    private void Display(Unit unit, Skill skill) {
        if (unit != null && skill != null) {
            unitEffects[2].Display(unit, skill);
        }
    }
    
    private void Display(Unit origin, Unit target, Skill skill) {
        if (origin != null && target != null && skill != null) {
            unitEffects[2].Display(origin, target, skill);
        }
    }
}
