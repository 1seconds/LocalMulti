using System.Collections.Generic;
using UnityEngine;

public class UnitEffectBase : MonoBehaviour {
    [SerializeField] protected List<UnitEffectItem> items;

    public virtual void Display(Unit unit) {
        
    }
    
    public virtual void Display(Unit origin, Unit target) {
        
    }
}