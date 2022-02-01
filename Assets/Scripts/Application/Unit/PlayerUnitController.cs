using UnityEngine;
public class PlayerUnitController : UnitBase {
    [SerializeField] private Transform playerUnitSet;
    [SerializeField] private Transform playerPool;

    private void ReadyData() {
        var selectedUnitCodes = Service.rule.selectedPlayerUnits;
        for (int i = 0; i < selectedUnitCodes.Count; i++) {
            var code = selectedUnitCodes[i];
            var prefab = Instantiate(GetPlayerUnit(code));
            prefab.GetComponent<UnitController>().Display();
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