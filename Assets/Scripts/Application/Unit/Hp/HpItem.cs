using UnityEngine;
using UnityEngine.UI;

public class HpItem : MonoBehaviour {
    [SerializeField] private Image hp;
    private Unit unit;
    
    public void Display(Unit unit) {
        this.unit = unit;
    }

    private void Update() {
        if (unit != null && Service.unit.selectedUnits.ContainsKey(unit)) {
            transform.position = Camera.main.WorldToScreenPoint(Service.unit.selectedUnits[unit].transform.position + Vector3.up);
        }
    }
}