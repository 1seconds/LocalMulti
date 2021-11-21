using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class RuleService : Singleton<RuleService>, IService {
    public ServiceType type => ServiceType.Rule;
    //public Dictionary<int, RuleWsStage> wsStages { get; private set; }
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        LoadLocalRules();
        return true;
    }

    private void LoadLocalRules() {
        
        
        //enStudies = new Dictionary<int, RuleEnStudy>();
        //LoadLocalRule<RuleEnStudy>("EngStudy/Studies").ForEach(e => {
        //    enStudies.Add(e.no, e);
        //);
    }
}