using UnityEngine;

public class UnitEffect3 : UnitEffectBase {
    
    public override void Display(Unit unit, Skill skill) {
        base.Display(unit, skill);
        
        for (int i = 0; i < items.Count; i++) {
            items[unit.unitIndex].HideUnitEffect3();
        }
        items[unit.unitIndex].DisplayUnitEffect3(unit, skill);
    }
    
    public override void Display(Unit origin, Unit target, Skill skill) {
        base.Display(origin, target, skill);
        
        for (int i = 0; i < items.Count; i++) {
            items[origin.unitIndex].HideUnitEffect3();
        }
        items[origin.unitIndex].DisplayUnitEffect3(origin, target, skill);
    }
}