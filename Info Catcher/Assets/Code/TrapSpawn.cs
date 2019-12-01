using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawn : MonoBehaviour {

    public Levels levelData;
    public GameObject Trap;

    private bool canSpawnTrap;

    private int availableTraps;
    private int TrapsLeftToSpawn;
    private float mapSizeX;
    private float mapSizeY;
    private int xBlocks;
    private int yBlocks;

    private void OnEnable()
    {
        //Subscribe to event
        GameManager.FirstPhaseEvent += AllowSpawnTrap;
        GameManager.SecondPhaseEvent += ForbitSpawnTrap;
        GameManager.ResetGameEvent += ResetGame;
    }

    void Start()
    {
        LevelData dt = levelData.TakeLevelData(GameManager.CurrentLevel);
        mapSizeX = dt.MapSizeX;
        mapSizeY = dt.MapSizeY;
        xBlocks = dt.Xblocks;
        yBlocks = dt.Yblocks;
        availableTraps = dt.Traps;

        TrapsLeftToSpawn = availableTraps;
    }
    

    void Update()
    {
          if (Input.touchCount > 0 && canSpawnTrap)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                if (TrapsLeftToSpawn > 0)
                {
                    Vector2 pos = ReturnTrapPosition(touch.position);
                    SpawnTrap(pos);
                    TrapsLeftToSpawn--;

                    if (TrapsLeftToSpawn <= 0)
                    {
                        canSpawnTrap = false;
                        GameManager.Instance.ExecuteSecondPhase();
                    }
                }

            }
        }
    }
    
    private void SpawnTrap(Vector2 pos)
    {
        float xTrapSize = mapSizeX / xBlocks;
        float yTrapSize = mapSizeY / yBlocks;
        GameObject obj = Instantiate(Trap, pos, Quaternion.identity);
        obj.transform.localScale = new Vector2(xTrapSize, yTrapSize);
    }

    public void TakeAvailableBlocks()
    {

    }

    private void AllowSpawnTrap()
    {
        canSpawnTrap = true;
    }

    private void ForbitSpawnTrap()
    {
        canSpawnTrap = false;
    }
    private void ResetGame()
    {
        LevelData dt = levelData.TakeLevelData(GameManager.CurrentLevel);
        mapSizeX = dt.MapSizeX;
        mapSizeY = dt.MapSizeY;
        xBlocks = dt.Xblocks;
        yBlocks = dt.Yblocks;
        availableTraps = dt.Traps;

        TrapsLeftToSpawn = availableTraps;

        GameObject obj = GameObject.FindGameObjectWithTag("Trap");
        Object.Destroy(obj);


    }
    private Vector2 ReturnTrapPosition(Vector2 touchPos)
    {
       
        Vector2 pos;
        pos = Camera.main.ScreenToWorldPoint(touchPos);
        float everyX = (float)mapSizeX/xBlocks;
        float everyY = (float)mapSizeY/yBlocks;

       
        float posX = 0;
        float posY = 0;

        float Xtransision = everyX;
        for (int i = 0; i < xBlocks; i++)
        {
            
            if(pos.x < Xtransision)
            {
                posX = ((Xtransision - everyX) + Xtransision) / 2;
                break;
            }
            else
            {
                Xtransision += everyX;
            }
        }

        float Ytransision = everyY;
        for (int i = 0; i < yBlocks; i++)
        {
            if (pos.y < Ytransision)
            {
                
                posY = ((Ytransision - everyY) + Ytransision) / 2;
                break;
            }
            else
            {
                Ytransision += everyY;
            }
        }

        pos = new Vector2(posX, posY);
       
        return pos;
    }
}
