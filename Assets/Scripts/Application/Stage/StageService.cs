using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public delegate void eventSelectedStage(int index);

public class StageService : Singleton<StageService>, IService {
    public ServiceType type => ServiceType.Stage;
    
    public event eventSelectedStage selectedStageUpdate;

    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        selectedStageUpdate = null;
        return true;
    }

    public void OnUpdateSelectedStage(int index) {
        selectedStageUpdate?.Invoke(index);
    }
}