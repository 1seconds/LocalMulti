using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillLockItem : MonoBehaviour {
    [SerializeField] private Text remainTimeText;
    private int skillInitTime;
    private int remainTime;

    private Coroutine timeRoutine;
    
    private void ReadyData() {
        //skillInitTime = Service.rule.units[400].
        skillInitTime = 12;
    }
    
    public void Display() {
        gameObject.SetActive(true);
        ReadyData();
        
        remainTime = skillInitTime;
        if (timeRoutine != null) {
            StopCoroutine(timeRoutine);
        }
        timeRoutine = StartCoroutine(TimeRoutine());
    }

    IEnumerator TimeRoutine() {
        while (true) {
            yield return new WaitForSeconds(1f);
            remainTime -= 1;
            if (remainTime <= 0) {
                break;
            }
        }
        gameObject.SetActive(false);
    }
}