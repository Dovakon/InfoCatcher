using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int CurrentLevel;
    public static float TimeLeft;

    public Levels LevelData;
    public GenerateMap Map;
    public int _currentLevel;

    public delegate void GameEvent();
    public static event GameEvent AllowTrapEvent; 

    private bool StartCounter = false;
    private bool LevelRuning = false;

    private void Awake()
    {
        TakeCurrentPlayerLevel();
    }
    

    private void Update()
    {
        if (StartCounter)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft <= 0)
            {
                StartCounter = false;
                TimeLeft = 0;
            }
        }
        else if (Input.touchCount > 0 && !LevelRuning)
        {
           
            StartCoroutine(GameFlow());
            
        }
    }

    void TakeCurrentPlayerLevel()
    {
        CurrentLevel = _currentLevel;
    }

    IEnumerator GameFlow()
    {
        LevelRuning = true;
        LevelData dt = LevelData.TakeLevelData(CurrentLevel);
        
        TimeLeft = dt.counterTime;

        //Game Start//
        //yield return new WaitForSeconds(3);
        Map.GenerateNewMap();
        yield return new WaitForSeconds(1);
        AllowTrapEvent();
        StartCounter = true;
        yield return new WaitUntil(() => StartCounter == false);
        LevelRuning = false;
    }

    
}
