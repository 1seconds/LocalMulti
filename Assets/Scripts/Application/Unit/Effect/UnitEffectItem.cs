using UnityEngine;

public class UnitEffectItem : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;

    public void DisplayUnitEffect1(Unit unit) {
        gameObject.SetActive(true);
    }
    public void HideUnitEffect1() {
        gameObject.SetActive(false);
    }
    
    public void DisplayUnitEffect2(Unit origin, Unit target) {
        gameObject.SetActive(true);
    }
    public void HideUnitEffect2() {
        gameObject.SetActive(false);
    }
    
    public void DisplayUnitEffect3(Unit origin, Skill skill) {
        gameObject.SetActive(true);
    }
    public void DisplayUnitEffect3(Unit origin, Unit tartget, Skill skill) {
        gameObject.SetActive(true);
    }
    public void HideUnitEffect3() {
        gameObject.SetActive(false);
    }
}