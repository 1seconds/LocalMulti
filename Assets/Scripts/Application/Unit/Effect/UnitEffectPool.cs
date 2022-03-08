using System.Collections.Generic;
using UnityEngine;

public class UnitEffectPool : MonoBehaviour {
    [SerializeField] private List<UnitEffectBase> unitEffects;

    private void OnEnable() {
        Service.unit.selectedUnitUpdate += Display;
        Service.unit.selectedUnitsUpdate += Display;
    }

    private void OnDisable() {
        Service.unit.selectedUnitUpdate -= Display;
        Service.unit.selectedUnitsUpdate -= Display;
    }

    private void ReadyData() {
        
    }
    
    private void Display(Unit unit) {
        ReadyData();
    
        Debug.LogError(unit);
        
        for (int i = 0; i < unitEffects.Count; i++) {
            unitEffects[i].Display(unit);
        }
    }
    
    private void Display(Unit origin, Unit target) {
        ReadyData();
        
        Debug.LogError(origin + ", " + target);
        
        for (int i = 0; i < unitEffects.Count; i++) {
            unitEffects[i].Display(origin, target);
        }
    }
}
