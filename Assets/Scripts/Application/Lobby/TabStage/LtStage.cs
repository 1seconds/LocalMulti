using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LtStage : TabBase {
    [SerializeField] private StageBox stageBox;
    protected override void OnShow() {
        Display();
    }
    
    private void Display() {
        stageBox.Display();
    }
}