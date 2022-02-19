using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitBase {
    [SerializeField] private Transform enemyUnitSet;
    [SerializeField] private Transform enemyPool;
    
    private void ReadyData(List<EnemyStageUnit> enemySet) {
        for (int i = 0; i < enemySet.Count; i++) {
            var prefab = Instantiate(GetEnemyUnit(enemySet[i].code));
            prefab.GetComponent<UnitController>().Display();
            prefab.transform.SetParent(enemyUnitSet);
            prefab.transform.localPosition = enemySet[i].transform.position;
        }
    }
    
    private GameObject GetEnemyUnit(int code) {
        for (int i = 0; i < enemyPool.childCount; i++) {
            if (enemyPool.GetChild(i).GetComponent<UnitController>().GetUnitCode() == code) {
                return enemyPool.GetChild(i).gameObject;
            }
        }
        Debug.LogError("error : " + code);
        return null;
    }
    
    public void Display(List<EnemyStageUnit> enemySet) {
        ReadyData(enemySet);
    }
}