using UnityEngine;

public class UnitEffect3 : UnitEffectBase {
    private void ReadyData() {
        
    }
    
    public override void Display(Unit unit) {
        base.Display(unit);
        
        ReadyData();

        for (int i = 0; i < items.Count; i++) {
            items[i].Display(unit);
        }
    }
    
    public override void Display(Unit origin, Unit target) {
        base.Display(origin, target);
        
        ReadyData();

        for (int i = 0; i < items.Count; i++) {
            items[i].Display(origin, target);
        }
    }
}