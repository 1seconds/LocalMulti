using UnityEngine;

public class UnitEffectItem : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    
    private void ReadyData() {
        
    }
    public void Display(Unit unit) {
        ReadyData();
    }
    
    public void Display(Unit origin, Unit target) {
        ReadyData();
    }
}