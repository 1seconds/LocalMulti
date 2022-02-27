using UnityEngine;
using UnityEngine.UI;

public class UnitSkillItem : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private UnitSkillLockItem lockItem;
    private void ReadyData() {
        
    }
    
    public void Display(int index) { 
        ReadyData();
    }
}