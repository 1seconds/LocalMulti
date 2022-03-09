using UnityEngine;

public class UnitEffect1 : UnitEffectBase {
    
    public override void Display(Unit unit) {
        base.Display(unit);
        
        for (int i = 0; i < items.Count; i++) {
            items[unit.unitIndex].HideUnitEffect1();
        }
        
        items[unit.unitIndex].DisplayUnitEffect1(unit);
    }
}