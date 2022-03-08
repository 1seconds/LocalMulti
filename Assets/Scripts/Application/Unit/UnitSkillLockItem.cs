using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillLockItem : MonoBehaviour {
    [SerializeField] private Text remainTimeText;

    private Skill skill;
    private CanvasGroup cg;

    private Coroutine timeRoutine;
    private float remainCoolTime;
    
    private void ReadyData() {
        if (cg == null) {
            cg = GetComponent<CanvasGroup>();
        }
        cg.alpha = 1f;
        remainTimeText.text = remainCoolTime.ToString("F1");
    }
    
    public void Display(Skill skill) {
        ReadyData();
        
        this.skill = skill;
        remainCoolTime = skill.remainCoolTime;

        if (timeRoutine != null) {
            StopCoroutine(timeRoutine);
        }
        timeRoutine = StartCoroutine(TimeRoutine());
    }

    public void Hide() {
        if (cg == null) {
            cg = GetComponent<CanvasGroup>();
        }
        
        cg.alpha = 0f;
    }

    IEnumerator TimeRoutine() {
        var passTime = 0f;
        
        while (true) {
            yield return new WaitForSeconds(0.01f);
            
            remainTimeText.text = (remainCoolTime - passTime).ToString("F1");
            skill.SetRemainCoolTime(remainCoolTime - passTime);
            
            passTime += 0.01f;
            if (remainCoolTime - passTime <= 0) {
                break;
            }
        }
        
        cg.alpha = 0f;
    }
}