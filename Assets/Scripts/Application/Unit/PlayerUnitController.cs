using UnityEngine;
public class PlayerUnitController : UnitBase {
    [SerializeField] private Transform playerUnitSet;
    [SerializeField] private Transform playerPool;

    private void ReadyData() {
        var selectedUnits = Service.rule.selectedPlayerUnits;
        for (int i = 0; i < selectedUnits.Count; i++) {
            var prefab = Instantiate(GetPlayerUnit(selectedUnits[i].unitCode));
            prefab.GetComponent<UnitController>().Display(selectedUnits[i]);
            prefab.transform.SetParent(playerUnitSet);
        }
    }

    private GameObject GetPlayerUnit(int code) {
        for (int i = 0; i < playerPool.childCount; i++) {
            if (playerPool.GetChild(i).GetComponent<UnitController>().GetUnitCode() == code) {
                return playerPool.GetChild(i).gameObject;
            }
        }
        Debug.LogError("error : " + code);
        return null;
    }
    
    public void Display() {
        ReadyData();
    }
}