using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UImanager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI CountdownText;
    
    private void Update()
    {
        CountdownText.text = GameManager.TimeLeft.ToString("F");
    }


}
