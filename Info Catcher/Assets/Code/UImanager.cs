using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UImanager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI CountdownText;
    [SerializeField] private TextMeshProUGUI WinsInaRowText;
    [SerializeField] private TextMeshProUGUI CurrentLevelText;

    private void OnEnable()
    {
        GameManager.FirstPhaseEvent += LevelStarted;
        CurrentLevelText.text = "Lv. " + GameManager.CurrentLevel.ToString();
    }

    private void Update()
    {
        CountdownText.text = GameManager.TimeLeft.ToString("F");
        WinsInaRowText.text = GameManager.WinsInaRow.ToString() + "/10";
        
    }

    private void LevelStarted()
    {
        CurrentLevelText.text = "Lv. " + GameManager.CurrentLevel.ToString();
    }

}
