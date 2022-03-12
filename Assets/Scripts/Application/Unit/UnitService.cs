using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public delegate void eventSelectedUnit(Unit unit);
public delegate void eventSelectedUnits(Unit originUnit, Unit targetUnit);
public delegate void eventSelectedUnitMovePoint(Unit originUnit, Vector2 point);

public class UnitService : Singleton<UnitService>, IService {
    public ServiceType type => ServiceType.Unit;
    
    public event eventSelectedUnit selectedUnitUpdate;
    public event eventSelectedUnits selectedUnitsUpdate;
    public event eventSelectedUnitMovePoint selectedUnitMovePointUpdate;

    public static Unit originUnit;
    public static Unit targetUnit;
    public Dictionary<Unit, UnitController> selectedUnits = new Dictionary<Unit, UnitController>();
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        selectedUnitUpdate = null;
        selectedUnitsUpdate = null;
        selectedUnitMovePointUpdate = null;
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
    
    public void OnUpdateSelectedUnitMovePoint(Unit origin, Vector2 point) {
        selectedUnitMovePointUpdate?.Invoke(origin, point);
        originUnit = origin;
    }
}