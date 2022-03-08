using UnityEngine;

public class UnitEffect2 : UnitEffectBase {
    private void ReadyData() {
        
    }
    
    public override void Display(Unit unit) {
        base.Display(unit);
        
        ReadyData();

        for (int i = 0; i < items.Count; i++) {
            items[i].Display(unit);
        }
    }
}