using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public IEnumerator Unit1SkillRoutine(Unit unit) {
        var passTime = 0f;
        List<float> initCoolTime = new List<float>();
        
        for (int i = 0; i < unit.skills.Count; i++) {
            initCoolTime.Add(unit.skills[i].remainCoolTime);
        }
        
        while (true) {
            yield return new WaitForEndOfFrame();
            passTime += Time.deltaTime;

            bool coolTimeDone = true;
            for (int i = 0; i < unit.skills.Count; i++) {
                unit.skills[i].SetRemainCoolTime(initCoolTime[i] - passTime);
                if (initCoolTime[i] - passTime > 0f) {
                    coolTimeDone = false;
                }
            }

            if (coolTimeDone) {
                break;
            }
        }
    }
    
    public IEnumerator Unit2SkillRoutine(Unit unit) {
        var passTime = 0f;
        List<float> initCoolTime = new List<float>();
        
        for (int i = 0; i < unit.skills.Count; i++) {
            initCoolTime.Add(unit.skills[i].remainCoolTime);
        }
        
        while (true) {
            yield return new WaitForEndOfFrame();
            passTime += Time.deltaTime;

            bool coolTimeDone = true;
            for (int i = 0; i < unit.skills.Count; i++) {
                unit.skills[i].SetRemainCoolTime(initCoolTime[i] - passTime);
                if (initCoolTime[i] - passTime > 0f) {
                    coolTimeDone = false;
                }
            }

            if (coolTimeDone) {
                break;
            }
        }
    }
    
    public IEnumerator Unit3SkillRoutine(Unit unit) {
        var passTime = 0f;
        List<float> initCoolTime = new List<float>();
        
        for (int i = 0; i < unit.skills.Count; i++) {
            initCoolTime.Add(unit.skills[i].remainCoolTime);
        }
        
        while (true) {
            yield return new WaitForEndOfFrame();
            passTime += Time.deltaTime;

            bool coolTimeDone = true;
            for (int i = 0; i < unit.skills.Count; i++) {
                unit.skills[i].SetRemainCoolTime(initCoolTime[i] - passTime);
                if (initCoolTime[i] - passTime > 0f) {
                    coolTimeDone = false;
                }
            }

            if (coolTimeDone) {
                break;
            }
        }
    }
    
    public IEnumerator Unit4SkillRoutine(Unit unit) {
        var passTime = 0f;
        List<float> initCoolTime = new List<float>();
        
        for (int i = 0; i < unit.skills.Count; i++) {
            initCoolTime.Add(unit.skills[i].remainCoolTime);
        }
        
        while (true) {
            yield return new WaitForEndOfFrame();
            passTime += Time.deltaTime;

            bool coolTimeDone = true;
            for (int i = 0; i < unit.skills.Count; i++) {
                unit.skills[i].SetRemainCoolTime(initCoolTime[i] - passTime);
                if (initCoolTime[i] - passTime > 0f) {
                    coolTimeDone = false;
                }
            }

            if (coolTimeDone) {
                break;
            }
        }
    }
}