using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public static int CurrentLevel;
    public static float TimeLeft;
    public static int WinsInaRow = 0;

    public Levels LevelData;
    public GenerateMap Map;
    public int _currentLevel;

    public delegate void GameEvent();
    public static event GameEvent FirstPhaseEvent;
    public static event GameEvent SecondPhaseEvent;
    //public static event GameEvent ThirdPhaseEvent;
    public static event GameEvent ResetGameEvent;

    
    private bool StartCounter = false;
    private bool LevelRuning = false;
    private bool StartSecondPhase = false;
    private bool StartThirdPhase = false;

    [HideInInspector]public bool LevelSucceed;
    private int TimeToSpawnTraps;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        

        ///SoS - Always First//
        TakePlayerData();
        //--//

        LevelData dt = LevelData.TakeLevelData(CurrentLevel);

        TimeToSpawnTraps = dt.Time;
        
    }
    

    private void Update()
    {
        if (StartCounter)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft <= 0)
            {
                //StartCounter = false;
                
                ExecuteSecondPhase();
                TimeLeft = 0;
                LevelSucceed = false;
            }
        }
        else if (Input.touchCount > 0 && !LevelRuning)
        {

            StartCoroutine(GameFlow());

        }
        else if (Input.GetKey("space") && !LevelRuning )
        {
            StartCoroutine(GameFlow());
        }
        else if (Input.GetKey("l"))
        {
            //GameFlow().MoveNext();
        }
    }

    void TakePlayerData()
    {
        PlayerData data = SaveLoadManager.LoadGame();
        print("data  " + data);
        if (data != null)
        {
            CurrentLevel = data.CurrentLevel;
            WinsInaRow = data.WinInaRow;
            print(CurrentLevel);
            print(WinsInaRow);
        }
        else
        {
            CurrentLevel = 1;
            WinsInaRow = 0;
        }
    }


    public void ExecuteSecondPhase()
    {
        //This Phase start when Timer finished or all Traps have spawned
        StartCounter = false;
        TimeLeft = 0;
        StartSecondPhase = true;

    }
    public void ExecuteThirdPhase(bool Succeed)
    {
        //This Phase start when InfoBullet Arrive to GoalPoint or Catched from trap
        //GreatePath.cs & InfoBullet.cs

        LevelSucceed = Succeed;
        StartThirdPhase = true;
    }


    private void ResetGame()
    {
        LevelData dt = LevelData.TakeLevelData(CurrentLevel);

        TimeToSpawnTraps = dt.Time;
        StartCounter = false;
        LevelRuning = false;
        StartSecondPhase = false;
        StartThirdPhase = false;
        ResetGameEvent();
    }

    public IEnumerator GameFlow()
    {
        LevelRuning = true;

        //Game Start//
        TimeLeft = TimeToSpawnTraps;
        StartCounter = true;
        Map.GenerateNewMap();

        //First Phase -- Allow Trap Spawn
        yield return new WaitForSeconds(1);
        print("First Phase Start");
        FirstPhaseEvent();

        //Second Phase -- InfoBullet Start moving
        yield return new WaitUntil(() => StartSecondPhase == true);
        print("Second Phase Start");
        SecondPhaseEvent();

        //Thrird Phase - Check if InfoBullet has been Catched or Espace
        yield return new WaitUntil(() => StartThirdPhase == true);
        print("Trird Phase Start");
        if (LevelSucceed)
        {
            WinsInaRow++;
           if(WinsInaRow >= 10)
            {
                //Save Data//
                //Player Level++
                //Load Scene Again
            }
            
        }

        SaveLoadManager.SaveGame();

        ResetGame();



        LevelRuning = false;
    }

    
}
