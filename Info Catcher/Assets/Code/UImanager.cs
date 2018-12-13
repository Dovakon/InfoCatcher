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
    }

    private void Update()
    {
        CountdownText.text = GameManager.TimeLeft.ToString("F");
        WinsInaRowText.text = GameManager.WinsInaRow.ToString("F");
        
    }

    private void LevelStarted()
    {
        CurrentLevelText.text = GameManager.CurrentLevel.ToString("F");
    }

}
