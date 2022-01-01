using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public delegate void eventSelectedUnit(JobType type);

public class UnitService : Singleton<UnitService>, IService {
    public ServiceType type => ServiceType.Unit;
    
    public event eventSelectedUnit selectedUnitUpdate;
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        return true;
    }

    public void OnUpdateSelectedUnit(JobType type) {
        selectedUnitUpdate?.Invoke(type);
    }
}