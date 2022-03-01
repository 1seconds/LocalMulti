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
    public Dictionary<int, UnitLvUpProperty> unitsLvUpProperties { get; private set; }

    public Dictionary<int, UnitProperty> unitsProperties { get; private set; }
    
    public Dictionary<int, Skill> skills { get; private set; }

    public List<Unit> selectedPlayerUnits;
    
    
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        LoadUnitLvPropertyRule();
        LoadUnitPropertyRule();
        LoadUnitRule();
        LoadSkillRule();
        
        selectedPlayerUnits = new List<Unit>();
        selectedPlayerUnits.Add(units[100]);
        selectedPlayerUnits.Add(units[200]);
        selectedPlayerUnits.Add(units[300]);
        selectedPlayerUnits.Add(units[400]);
        return true;
    }

    private void LoadSkillRule() {
        var text = PersistenceUtil.LoadTextResource("Rules/skill");
        if (string.IsNullOrEmpty(text)) {
            return;
        }

        skills = new Dictionary<int, Skill>();
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var rule = new Skill(row);
            skills.Add(rule.skillCode, rule);
        }
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

        unitsProperties = new Dictionary<int, UnitProperty>();
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var rule = new UnitProperty(row);
            unitsProperties.Add(rule.unitCode, rule);
        }
    }
    
    private void LoadUnitLvPropertyRule() {
        var text = PersistenceUtil.LoadTextResource("Rules/unitLvUpProperty");
        if (string.IsNullOrEmpty(text)) {
            return;
        }

        unitsLvUpProperties = new Dictionary<int, UnitLvUpProperty>();
        var parser = new CsvParser();
        parser.Parse(text, "\t");

        for (int index = 1; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var rule = new UnitLvUpProperty(row);
            unitsLvUpProperties.Add(rule.unitCode, rule);
        }
    }
}