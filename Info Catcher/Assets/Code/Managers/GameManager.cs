using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [HideInInspector] public static int CurrentLevel;
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

            //StartCoroutine(GameFlow());

        }
        else if (Input.GetKey("space") && !LevelRuning )
        {
            StartCoroutine(GameFlow());
        }
        else if (Input.GetKey("d"))
        {
            DeleteSave();
        }
    }

    void TakePlayerData()
    {
        PlayerData data = SaveLoadManager.LoadGame();

        if (data != null)
        {
            CurrentLevel = data.CurrentLevel;
            WinsInaRow = data.WinInaRow;
        }
        else
        {
            CurrentLevel = 1;
            WinsInaRow = 0;
        }
    }

    public void StartGame()
    {
        if(LevelRuning == false)
            StartCoroutine(GameFlow());
    }


    public void ExecuteSecondPhase()
    {
        //This Phase start when Timer finished or all Traps have spawned
        StartCounter = false;
        TimeLeft = 0;
        StartSecondPhase = true;
        //Event Trigger
        SecondPhaseEvent();

    }
    public void ExecuteThirdPhase(bool IsSucceed)
    {
        //This Phase start when InfoBullet Arrive to GoalPoint or Catched from trap
        //GreatePath.cs & InfoBullet.cs

        LevelSucceed = IsSucceed;
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
        //Allow to for game to prepared
        yield return new WaitForSeconds(1f);




        LevelRuning = true;

        //Game Start//
        TimeLeft = TimeToSpawnTraps;
        
        Map.GenerateNewMap();

        yield return new WaitForSeconds(1f);
        Map.RunNewMap();
        //First Phase -- Allow Trap Spawn
        yield return new WaitForSeconds(2);
        print("First Phase Start");
        StartCounter = true;
        FirstPhaseEvent();

        //Second Phase -- InfoBullet Start moving
        yield return new WaitUntil(() => StartSecondPhase == true);
        print("Second Phase Start");
        

        //Thrird Phase - Check if InfoBullet has been Catched or Espace
        yield return new WaitUntil(() => StartThirdPhase == true);
        print("Trird Phase Start");
        if (LevelSucceed)
        {
            WinsInaRow++;
           if(WinsInaRow >= 5)
            {
                CurrentLevel++;
                WinsInaRow = 0;
                SaveLoadManager.SaveGame();

                FirstPhaseEvent = null;
                SecondPhaseEvent = null; ;
                ResetGameEvent = null;

                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
                yield return null;
            }
            
        }

        //WinsInaRow = 0;
        SaveLoadManager.SaveGame();
        ResetGame();






        LevelRuning = false;
    }


    public void DeleteSave()
    {
        SaveLoadManager.DeleteSavedGame();
    }

}
