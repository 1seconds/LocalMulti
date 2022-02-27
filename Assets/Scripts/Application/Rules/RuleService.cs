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
    public Dictionary<int, UnitLvUpProperty> unitsLvUpProperty { get; private set; }

    public Dictionary<int, UnitProperty> unitsProperty { get; private set; }
    
    public Dictionary<int, Unit> skills { get; private set; }
    public Dictionary<int, UnitLvUpProperty> skillsLvUpProperty { get; private set; }

    public Dictionary<int, UnitProperty> skillsProperty { get; private set; }
    

    public List<Unit> selectedPlayerUnits;
    
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        LoadUnitLvPropertyRule();
        LoadUnitPropertyRule();
        LoadUnitRule();
        
        selectedPlayerUnits = new List<Unit>();
        selectedPlayerUnits.Add(units[100]);
        selectedPlayerUnits.Add(units[200]);
        selectedPlayerUnits.Add(units[300]);
        selectedPlayerUnits.Add(units[400]);
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
            units.Add(rule.unitCode, rule);
        }
    }
    
    private void LoadUnitPropertyRule() {
        var text = PersistenceUtil.LoadTextResource("Rules/unitProperty");
        if (string.IsNullOrEmpty(text)) {
            return;
        }

        unitsProperty = new Dictionary<int, UnitProperty>();
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var rule = new UnitProperty(row);
            unitsProperty.Add(rule.code, rule);
        }
    }
    
    private void LoadUnitLvPropertyRule() {
        var text = PersistenceUtil.LoadTextResource("Rules/unitLvUpProperty");
        if (string.IsNullOrEmpty(text)) {
            return;
        }

        unitsLvUpProperty = new Dictionary<int, UnitLvUpProperty>();
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var rule = new UnitLvUpProperty(row);
            unitsLvUpProperty.Add(rule.code, rule);
        }
    }
}