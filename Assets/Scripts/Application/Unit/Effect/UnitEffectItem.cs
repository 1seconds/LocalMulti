using System.Collections;
using UnityEngine;

public class UnitEffectItem : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;

    private Coroutine selectEffectRoutine;

    public void OnSelectEffect(Unit unit) {
        Debug.LogError("Select " + unit.unitCode);
        var unitTransForm = Service.unit.selectedUnits[unit].transform;
        transform.position = unitTransForm.position + Vector3.up * 1.1f;
        
        gameObject.SetActive(true);
        if (selectEffectRoutine != null) {
            StopCoroutine(selectEffectRoutine);
        }
        selectEffectRoutine = StartCoroutine(Effect1Routine(unit));
    }

    IEnumerator Effect1Routine(Unit unit) {
        var pingpongTime = 0f;
        
        while (true) {
            yield return new WaitForEndOfFrame();
            pingpongTime += Time.deltaTime;
            var unitTransForm = Service.unit.selectedUnits[unit].transform;
            transform.position = Vector2.Lerp(unitTransForm.position + Vector3.up * 1.1f, unitTransForm.position + Vector3.up * 1.3f, Mathf.PingPong(pingpongTime, 1));
        }
    }

    public void OnInteractionEffect(Unit origin, Unit target) {
        Debug.LogError("Interaction " + origin.unitCode + ", " + target.unitCode);
        gameObject.SetActive(true);
    }
    
    public void OnMoveEffect(Unit origin, Vector2 targetPoint) {
        Debug.LogError("Move " + origin.unitCode + ", " + targetPoint);
        gameObject.SetActive(true);
    }

    public void OnNoneTargetSkillEffect(Unit origin, Skill skill) {
        Debug.LogError("NoneTargetSkill " + origin.unitCode + ", " + skill.skillCode);
        gameObject.SetActive(true);
    }
    public void OnTargetSkillEffect(Unit origin, Unit tartget, Skill skill) {
        Debug.LogError("TargetSkill " + origin.unitCode + ", " + tartget.unitCode + ", " + skill.skillCode);
        gameObject.SetActive(true);
    }
    
    public void OffEffect() {
        gameObject.SetActive(false);
    }
}