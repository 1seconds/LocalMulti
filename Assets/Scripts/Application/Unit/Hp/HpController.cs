using System;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour {    
    [SerializeField] private Transform hpSet;
    [SerializeField] private HpItem hpItem;

    [SerializeField] private Transform enemyUnitSet;
    [SerializeField] private Transform playerUnitSet;
    
    private List<Unit> allUnits;

    private void OnEnable() {
        Service.unit.setUnitUpdate += Display;
    }

    private void OnDisable() {
        Service.unit.setUnitUpdate -= Display;
    }

    private void ReadyData() {
        allUnits = new List<Unit>();
        for (int i = 0; i < enemyUnitSet.childCount; i++) {
            allUnits.Add(enemyUnitSet.GetChild(i).GetComponent<UnitController>().GetUnit());
        }
        for (int i = 0; i < playerUnitSet.childCount; i++) {
            allUnits.Add(playerUnitSet.GetChild(i).GetComponent<UnitController>().GetUnit());
        }
    }

    private void Display() {
        ReadyData();

        for (int i = 0; i < hpSet.childCount; i++) {
            Destroy(hpSet.GetChild(i).gameObject); 
        }
        
        for (int i = 0; i < allUnits.Count; i++) {
            var prefab = Instantiate(hpItem);
            prefab.gameObject.SetActive(true);
            prefab.GetComponent<HpItem>().Display(allUnits[i]);
            prefab.transform.SetParent(hpSet);
        }
    }
}