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
    void Start ()
    {
        LevelData dt = levelData.TakeLevelData(GameManager.CurrentLevel);
        mapSizeX = dt.MapSizeX;
        mapSizeY = dt.MapSizeY;
        xBlocks = dt.Xblocks;
        yBlocks = dt.Yblocks;
        availableTraps = dt.Traps;
        

    }

    private void OnEnable()
    {
        //Subscribe to event
        GameManager.AllowTrapEvent += AllowSpawnTrap;
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
                    Instantiate(Trap, pos, Quaternion.identity);
                    TrapsLeftToSpawn--;
                }
                else
                {
                    canSpawnTrap = false;
                }
            }
        }
    }
    private void AllowSpawnTrap()
    {
        TrapsLeftToSpawn = availableTraps;
        canSpawnTrap = true;

    }

    private Vector2 ReturnTrapPosition(Vector2 touchPos)
    {
        print("Count");
        GameObject obj = GameObject.FindGameObjectWithTag("Trap");
        Object.Destroy(obj);

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
                print("Xtouch  " + pos.x);
                posX = ((Xtransision - everyX) + Xtransision) / 2;
                print("Xpos  " + posX);
                print("EveryX  " + everyX);
                print("praksi  " + (everyX - (mapSizeX / xBlocks)));
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
                //print("Ytouch" + posY);
                //print("EveryY" + everyY);
                //print("praksi" + (everyY - (mapSizeY / yBlocks)));
                break;
            }
            else
            {
                Ytransision += everyY;
            }
        }

        //print("Xtouch" + posX);
        
        pos = new Vector2(posX, posY);
       
        return pos;
    }
}
