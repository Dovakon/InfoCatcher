using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UImanager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI CountdownText;
    [SerializeField] private Image CountdownSprite;
    [SerializeField] private TextMeshProUGUI CurrentLevelText;
    [SerializeField] private Slider FillArea;

    private Animator anim;
    private float levelTime = 0;
    private void OnEnable()
    {
        GameManager.FirstPhaseEvent += GameStarted;
        GameManager.ResetGameEvent += PauseGame;
    }

    private void Start()
    {
        CurrentLevelText.text = "Level " + GameManager.CurrentLevel.ToString();
        FillArea.value = GameManager.WinsInaRow / 5;
        anim = GetComponent<Animator>();
  
    }
    private void Update()
    {
        CountdownText.text = GameManager.TimeLeft.ToString("F");
        CountdownSprite.fillAmount = GameManager.TimeLeft/levelTime;
        FillArea.value = GameManager.WinsInaRow * .2f;

    }

    public void HideMenu()
    {
        anim.SetBool("PrepareGame", true);
    }
    
    //private void ShowMenu()
    //{   
    //    anim.SetBool("GameRunning", false);
    //}
   
    public void PauseGame()
    {
        anim.SetBool("PauseGame", true);
    }
    public void ResumeGame()
    {
        anim.SetBool("PauseGame", false);
    }
    
    public void GameStarted()
    {
        levelTime = GameManager.TimeLeft;
        print(levelTime);
    }

}
