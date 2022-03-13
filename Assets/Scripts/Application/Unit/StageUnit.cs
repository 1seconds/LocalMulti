using System.Collections.Generic;
using UnityEngine;

public class EnemyStageUnit {
    public int code;
    public int level;
    public Transform transform;

    public static EnemyStageUnit Build(int code, int level, Transform transform) {
        var res = new EnemyStageUnit();
        res.code = code;
        res.level = level;
        res.transform = transform;
        return res;
    }
}

public class StageUnit : UnitBase {
    [SerializeField] private PlayerUnitController playerUnitContoller;
    [SerializeField] private EnemyUnitController enemyUnitController;
    
    [SerializeField] private List<Transform> positionSet;

    private Dictionary<int, List<EnemyStageUnit>> stageEnemyUnitSet;
    
    private void ReadyData() {
        stageEnemyUnitSet = new Dictionary<int, List<EnemyStageUnit>>();
        
        stageEnemyUnitSet.Add(1, new List<EnemyStageUnit>() {
            EnemyStageUnit.Build(1000,1, positionSet[0])
        });
        stageEnemyUnitSet.Add(2, new List<EnemyStageUnit>() {
            EnemyStageUnit.Build(1000,1, positionSet[0]),
            EnemyStageUnit.Build(1000,1, positionSet[1])
        });
        stageEnemyUnitSet.Add(3, new List<EnemyStageUnit>() {
            EnemyStageUnit.Build(1000,1, positionSet[0]),
            EnemyStageUnit.Build(1000,1, positionSet[1]),
            EnemyStageUnit.Build(1000,1, positionSet[2]),
            EnemyStageUnit.Build(1000,2, positionSet[3])
        });
    }
    private void Start() {
        ReadyData();
        
        Display(2);
    }

    public void Display(int stageIndex) {
        ReadyData();
        
        playerUnitContoller.Display();
        enemyUnitController.Display(stageEnemyUnitSet[stageIndex]);
    }
}