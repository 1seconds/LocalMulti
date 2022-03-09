using UnityEngine;

public class UnitEffect2 : UnitEffectBase {

    public override void Display(Unit origin, Unit target) {
        base.Display(origin, target);
        
        for (int i = 0; i < items.Count; i++) {
            items[origin.unitIndex].HideUnitEffect2();
        }
        
        items[origin.unitIndex].DisplayUnitEffect2(origin, target);
    }
}