using System.Collections.Generic;
using UnityEngine;

public class UnitEffectBase : MonoBehaviour {
    [SerializeField] protected List<UnitEffectItem> items;

    public virtual void Display(Unit unit) {
        
    }
    
    public virtual void Display(Unit origin, Unit target) {
        
    }
    
    public virtual void Display(Unit unit, Skill skill) {
        
    }
    
    public virtual void Display(Unit origin, Unit target, Skill skill) {
        
    }

    public virtual void Hide() {
        
    }
}