using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public delegate void eventSelectedUnit(Unit unit);

public class UnitService : Singleton<UnitService>, IService {
    public ServiceType type => ServiceType.Unit;
    
    public event eventSelectedUnit selectedUnitUpdate;

    public static Unit originUnit;
    public static Unit targetUnit;
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        return true;
    }

    public void OnUpdateSelectedUnit(Unit unit) {
        selectedUnitUpdate?.Invoke(unit);
        originUnit = unit;
    }
}