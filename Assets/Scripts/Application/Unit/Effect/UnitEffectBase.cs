using System.Collections.Generic;
using UnityEngine;

public class UnitEffectBase : MonoBehaviour {
    [SerializeField] protected List<UnitEffectItem> items;

    public void OnEffect(Unit unit) {
        OffEffect();
        items[unit.unitIndex].OnSelectEffect(unit);
    }
    
    public void OnEffect(Unit origin, Unit target) {
        items[origin.unitIndex].OnInteractionEffect(origin, target);
    }
    
    public void OnEffect(Unit origin, Vector2 targetPoint) {
        items[origin.unitIndex].OnMoveEffect(origin, targetPoint);
    }
    
    public void OnEffect(Unit unit, Skill skill) {
        items[unit.unitIndex].OnNoneTargetSkillEffect(unit, skill);
    }
    
    public void OnEffect(Unit origin, Unit target, Skill skill) {
        items[origin.unitIndex].OnTargetSkillEffect(origin, target, skill);
    }

    public void OffEffect() {
        for (int i = 0; i < items.Count; i++) {
            items[i].OffEffect();
        }
    }
}