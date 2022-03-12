using System.Collections;
using UnityEngine;

public class UnitEffectItem : MonoBehaviour {
    private Coroutine selectEffectRoutine;
    private Coroutine interactionEffectRoutine;
    private Coroutine moveEffectRoutine;

    public void OnSelectEffect(Unit unit) {
        var unitTransForm = Service.unit.selectedUnits[unit].transform;
        transform.position = unitTransForm.position + Vector3.up * 1.1f;
        
        gameObject.SetActive(true);
        if (selectEffectRoutine != null) {
            StopCoroutine(selectEffectRoutine);
        }
        selectEffectRoutine = StartCoroutine(SelectRoutine(unit));
    }

    IEnumerator SelectRoutine(Unit unit) {
        var pingpongTime = 0f;
        
        while (true) {
            yield return new WaitForEndOfFrame();
            pingpongTime += Time.deltaTime;
            var unitTransForm = Service.unit.selectedUnits[unit].transform;
            transform.position = Vector2.Lerp(unitTransForm.position + Vector3.up * 1.1f, unitTransForm.position + Vector3.up * 1.3f, Mathf.PingPong(pingpongTime, 1));
        }
    }

    public void OnInteractionEffect(Unit origin, Unit target) {
        var unitTransForm = Service.unit.selectedUnits[target].transform;
        transform.position = unitTransForm.position + Vector3.up * 1.1f;
        
        gameObject.SetActive(true);
        if (interactionEffectRoutine != null) {
            StopCoroutine(interactionEffectRoutine);
        }
        interactionEffectRoutine = StartCoroutine(InteractionRoutine(target));
    }
    
    IEnumerator InteractionRoutine(Unit unit) {
        var pingpongTime = 0f;
        
        while (true) {
            yield return new WaitForEndOfFrame();
            pingpongTime += Time.deltaTime;
            var unitTransForm = Service.unit.selectedUnits[unit].transform;
            transform.position = Vector2.Lerp(unitTransForm.position + Vector3.up * 1.1f, unitTransForm.position + Vector3.up * 1.3f, Mathf.PingPong(pingpongTime, 1));
        }
    }
    
    public void OnMoveEffect(Unit origin, Vector2 targetPoint) {
        transform.position = targetPoint;
        
        gameObject.SetActive(true);
        if (moveEffectRoutine != null) {
            StopCoroutine(moveEffectRoutine);
        }
        moveEffectRoutine = StartCoroutine(MoveRoutine(targetPoint));
    }
    
    IEnumerator MoveRoutine(Vector2 targetPoint) {
        var pingpongTime = 0f;
        
        while (true) {
            yield return new WaitForEndOfFrame();
            pingpongTime += Time.deltaTime;
            transform.position = targetPoint;
            transform.localScale = Vector2.Lerp(Vector2.one * 2f, Vector2.one * 3f, Mathf.PingPong(pingpongTime, 0.5f));
        }
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