using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitBase {
    [SerializeField] private Transform enemyUnitSet;
    [SerializeField] private Transform enemyPool;
    
    private void ReadyData(Dictionary<int, int> enemySet) {
        foreach (var selectedEnemy in enemySet) {
            for (int i = 0; i < selectedEnemy.Value; i++) {
                var code = selectedEnemy.Key;
                var prefab = Instantiate(GetEnemyUnit(code));
                prefab.GetComponent<UnitController>().Display();
                prefab.transform.SetParent(enemyUnitSet);
            }
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
    
    public void Display(Dictionary<int, int> enemySet) {
        ReadyData(enemySet);
    }
}