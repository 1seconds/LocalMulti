using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class RuleService : Singleton<RuleService>, IService {
    public ServiceType type => ServiceType.Rule;
    public Dictionary<int, Unit> units { get; private set; }
    public Dictionary<int, UnitProperty> unitsLvUpProperty { get; private set; }

    public List<int> selectedPlayerUnits;
    
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        LoadUnitRule();
        LoadUnitLvPropertyRule();
        return true;
    }

    
    private void LoadUnitRule() {
        var text = PersistenceUtil.LoadTextResource("Rules/unit");
        if (string.IsNullOrEmpty(text)) {
            return;
        }

        units = new Dictionary<int, Unit>();
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var rule = new Unit(row);
            units.Add(rule.code, rule);
        }
    }
    
    private void LoadUnitLvPropertyRule() {
        var text = PersistenceUtil.LoadTextResource("Rules/unitLvUpProperty");
        if (string.IsNullOrEmpty(text)) {
            return;
        }

        unitsLvUpProperty = new Dictionary<int, UnitProperty>();
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var rule = new UnitProperty(row);
            unitsLvUpProperty.Add(rule.code, rule);
        }
    }
}