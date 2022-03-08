using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public delegate void eventSelectedUnit(Unit unit);
public delegate void eventSelectedUnits(Unit originUnit, Unit targetUnit);

public class UnitService : Singleton<UnitService>, IService {
    public ServiceType type => ServiceType.Unit;
    
    public event eventSelectedUnit selectedUnitUpdate;
    public event eventSelectedUnits selectedUnitsUpdate;

    public static Unit originUnit;
    public static Unit targetUnit;
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        return true;
    }

    public void OnUpdateSelectedUnit(Unit unit) {
        selectedUnitUpdate?.Invoke(unit);
        originUnit = unit;
    }
    
    public void OnUpdateSelectedUnits(Unit origin, Unit target) {
        selectedUnitsUpdate?.Invoke(origin, target);
        originUnit = origin;
        targetUnit = target;
    }
}