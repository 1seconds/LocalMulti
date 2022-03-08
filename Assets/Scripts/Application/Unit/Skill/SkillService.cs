using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public delegate void eventSelectedSkill(Unit unit);

public class SkillService : Singleton<SkillService>, IService {
    public ServiceType type => ServiceType.Skill;
    
    public event eventSelectedSkill selectedSkillUpdate;
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        return true;
    }

    public void OnUpdateSelectedSkill(Unit unit) {
        selectedSkillUpdate?.Invoke(unit);
    }
}