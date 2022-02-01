using System.Collections.Generic;
using UnityEngine;

public class StageUnit : UnitBase {
    [SerializeField] private PlayerUnitController playerUnitContoller;
    [SerializeField] private EnemyUnitController enemyUnitController;

    private Dictionary<int, Dictionary<int, int>> stageEnemyUnitSet;
    
    private void ReadyData() {
        stageEnemyUnitSet = new Dictionary<int, Dictionary<int, int>>();
        
        stageEnemyUnitSet.Add(1, new Dictionary<int, int>() { { 1001, 1 }});
        stageEnemyUnitSet.Add(2, new Dictionary<int, int>() { { 1001, 2 }});
        stageEnemyUnitSet.Add(3, new Dictionary<int, int>() { { 1001, 3 }, { 1002, 1 }});
        stageEnemyUnitSet.Add(4, new Dictionary<int, int>() { { 1001, 3 }, { 1002, 3 }});
        stageEnemyUnitSet.Add(5, new Dictionary<int, int>() { { 1003, 1 }});
        stageEnemyUnitSet.Add(6, new Dictionary<int, int>() { { 1003, 2 }});
    }
    private void Start() {
        ReadyData();
        
        Display(1);
    }

    public void Display(int stageIndex) {
        ReadyData();
        
        playerUnitContoller.Display();
        enemyUnitController.Display(stageEnemyUnitSet[stageIndex]);
    }
}