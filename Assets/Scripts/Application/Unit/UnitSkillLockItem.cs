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
    }
    
    public void Display(Skill skill) {
        ReadyData();
        
        this.skill = skill;
        
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
        while (true) {
            remainTimeText.text = (remainCoolTime).ToString("F1");
            yield return new WaitForSeconds(0.01f);
            remainCoolTime = skill.remainCoolTime;
            if (remainCoolTime <= 0) {
                break;
            }
        }
        cg.alpha = 0f;
    }
}