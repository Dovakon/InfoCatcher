
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateMap : MonoBehaviour {

    [Range(0f, 3)]
    public float TimeToStart;

   

    public Levels LevelData;
    public GameObject BlackBlock;
    public GameObject WhiteBlock;
    public GameObject HorizontalWallObject;
    public GameObject VerticalWallObject;
    public GameObject BlackEntry;
    public GameObject WhiteEntry;
    public GameObject BlocksParent;
    public CreatePath createPath;

    //Blocks//
    private int numberBlocksX;
    private int numberBlocksY;
    private Block[] block;
    private int RedEntryPoint = 0;
    private int GreenEntryPoint = 0;
    private int RedGoalPoint = 0;
    private int GreenGoalPoint = 0;

    private float sizeSpaceX;
    private float sizeSpaceY;

    //Walls//
    private int numberWallsX;
    private int numberWallsY;
    private GameObject[] VerticalWalls;
    private GameObject[] HorizontalWalls;
    private GameObject[] UpWalls, RightWalls;
    private List<GameObject> LinkWalls;
    
    //EntryPoints//
    private GameObject _greenEntryPoint;
    private GameObject _greenGoalPoint;
    private GameObject _redEntryPoint;
    private GameObject _redGoalPoint;

    
    //....///
    public bool insta;
    public bool CreateBlockObject;
    public bool RandomStart;
    private bool showBlockNumbers = false;



    void Start()
    {

        LevelData dt = LevelData.TakeLevelData(GameManager.CurrentLevel);

        numberBlocksX = dt.Xblocks;
        numberBlocksY = dt.Yblocks;

        sizeSpaceX = dt.MapSizeX;
        sizeSpaceY = dt.MapSizeY;

    LinkWalls = new List<GameObject>();

        
        InstantiateWalls();
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            showBlockNumbers = true;
        }
        
       
    }




    //Walls
    private void InstantiateWalls()
    {

        float wallEveryX = (float)sizeSpaceX / numberBlocksX;
        float wallEveryY = (float)sizeSpaceY / numberBlocksY;
        float Xposs = wallEveryX * .5f;
        float Yposs = sizeSpaceY;

        //UP & Right Walls
        UpWalls = new GameObject[numberBlocksX];
        RightWalls = new GameObject[numberBlocksY];
        for (int i = 0; i < numberBlocksX; i++)
        {
            GameObject wallobj = Instantiate(HorizontalWallObject, new Vector2(Xposs, Yposs), HorizontalWallObject.transform.rotation);
            wallobj.transform.localScale = new Vector3(wallobj.transform.localScale.x, wallEveryX, 1);
            UpWalls[i] = wallobj;
            Xposs += wallEveryX;

        }
        Xposs = sizeSpaceX;
        Yposs = wallEveryY * .5f;

        for (int i = 0; i < numberBlocksY; i++)
        {
            GameObject wallobj = Instantiate(HorizontalWallObject, new Vector2(Xposs, Yposs), Quaternion.identity);
            wallobj.transform.localScale = new Vector3(wallobj.transform.localScale.x, wallEveryY, 1);
            RightWalls[i] = wallobj;
            Yposs += wallEveryY;
        }


        //Walls//
        numberWallsX = numberBlocksX;
        numberWallsY = numberBlocksX;

        VerticalWalls = new GameObject[numberBlocksX * numberBlocksY];
        HorizontalWalls = new GameObject[numberBlocksX * numberBlocksY];



        wallEveryX = (float)sizeSpaceX / numberBlocksX;
        wallEveryY = (float)sizeSpaceY / numberBlocksY;


        int Vcounter = 0;
        int Hcounter = 0;
        //Vector2 poss = new Vector2(wallEveryX, wallEveryY);

        Xposs = wallEveryX * .5f;
        Yposs = 0f;

        for (int i = 0; i < numberBlocksY; i++)
        {

            for (int y = 0; y < numberBlocksX; y++)
            {
                GameObject wallobj = Instantiate(HorizontalWallObject, new Vector2(Xposs, Yposs), HorizontalWallObject.transform.rotation);
                wallobj.transform.localScale = new Vector3(wallobj.transform.localScale.x, wallEveryX, 1);
                HorizontalWalls[Hcounter] = wallobj;
                Hcounter++;
                Xposs += wallEveryX;

            }

            Xposs = 0;
            Yposs += wallEveryY * .5f;

            for (int y = 0; y < numberBlocksX; y++)
            {
                GameObject wallobj = Instantiate(VerticalWallObject, new Vector2(Xposs, Yposs), Quaternion.identity);
                wallobj.transform.localScale = new Vector3(wallobj.transform.localScale.x, wallEveryY, 1);
                VerticalWalls[Vcounter] = wallobj;
                Vcounter++;
                Xposs += wallEveryX;
            }

            Xposs = wallEveryX * .5f;
            Yposs += wallEveryY * .5f;
        }
    }

    private void RemoveWall(int poss, string move)
    {
        if (move == "Left")
        {
            VerticalWalls[poss].gameObject.GetComponent<Wall>().Dissolve();
        }
        if (move == "Up")
        {
            HorizontalWalls[poss + numberBlocksX].GetComponent<Wall>().Dissolve();
        }
        if (move == "Right")
        {
            VerticalWalls[poss + 1].gameObject.GetComponent<Wall>().Dissolve();
        }
        if (move == "Down")
        {
            HorizontalWalls[poss].gameObject.GetComponent<Wall>().Dissolve();
        }
    }

    private void AddLinkWall(int poss, string move)
    {
        if (move == "Left")
        {
            LinkWalls.Add(VerticalWalls[poss].gameObject);
        }
        if (move == "Up")
        {
            LinkWalls.Add(HorizontalWalls[poss + numberBlocksX].gameObject);
        }
        if (move == "Right")
        {
            LinkWalls.Add(VerticalWalls[poss + 1].gameObject);
        }
        if (move == "Down")
        {
            LinkWalls.Add(HorizontalWalls[poss].gameObject);
        }

    }

    private void ChoseLinkWall()
    {
        int wall = Random.Range(0, LinkWalls.Count);

        for (int i = 0; i < HorizontalWalls.Length; i++)
        {
            if (HorizontalWalls[i].gameObject == LinkWalls[wall].gameObject)
            {
                LinkWalls[wall].gameObject.GetComponent<Wall>().Dissolve();
                // Update Block Movement
                block[i].DownMove = true;
                block[i - numberBlocksX].UpMove = true;
                return;
            }
        }

        for (int i = 0; i < VerticalWalls.Length; i++)
        {
            if (VerticalWalls[i].gameObject == LinkWalls[wall].gameObject)
            {
                LinkWalls[wall].gameObject.GetComponent<Wall>().Dissolve();
                block[i - 1].RightMove = true;
                block[i].LeftMove = true;
                return;
            }
        }


    }

    //Create Blocks//

    private void DefineBlock()
    {



        block = new Block[numberBlocksX * numberBlocksY];
        float blockEveryX = (float)sizeSpaceX / numberBlocksX;
        float blockEveryY = (float)sizeSpaceY / numberBlocksY;


        float xposs, yposs;
        xposs = blockEveryX / 2;
        yposs = blockEveryY / 2;

        int counter = 0;

        for (int y = 0; y < numberBlocksY; y++)
        {


            for (int x = 0; x < numberBlocksX; x++)
            {


                block[counter] = new Block
                {
                    Possition = new Vector2(xposs, yposs)
                };

                WhereCanMove(counter);
                counter++;
                xposs += blockEveryX;


            }
            xposs = blockEveryX / 2;
            yposs += blockEveryY;

            //**//
            //Cant Move Right
            //Probably Will Change  
            block[counter - 1]._rightMove = false; // Right
            block[counter - 1].RightMove = false;
            //**//

        }
    }
    private void WhereCanMove(int blk)
    {

        if (blk % numberBlocksX == 0)  //Left
        {
            block[blk]._leftMove = false;
            block[blk].LeftMove = false;
        }
        //if (blk  == numberBlocksX -1) //Right
        //{
        //    block[blk].rightMove = false;
        //}
        if (blk < numberBlocksX)       // Down
        {
            block[blk]._downMove = false;
            block[blk].DownMove = false;
        }
        if (blk >= numberBlocksX * (numberBlocksY - 1))     // UP
        {
            block[blk]._upMove = false;
            block[blk].UpMove = false;
        }
    }

    ///......///
    //GameManager Called it
    public void GenerateNewMap()
    {
        //if this is not the first time this fuction runing in current scene
        if (!(LinkWalls.Count <= 0))
        {
            LinkWalls = new List<GameObject>();

            foreach (var wall in HorizontalWalls)
            {
                if (wall.gameObject.GetComponent<Wall>().isDestroyed)
                    wall.gameObject.GetComponent<Wall>().ReappearWallSprite();
            }
            foreach (var wall in VerticalWalls)
            {
                if (wall.gameObject.GetComponent<Wall>().isDestroyed)
                    wall.gameObject.GetComponent<Wall>().ReappearWallSprite();
            }

            foreach (var wall in UpWalls)
            {
                if (wall.gameObject.GetComponent<Wall>().isDestroyed)
                    wall.gameObject.GetComponent<Wall>().ReappearWallSprite();
            }
        }
        else //if basic map already generate in current scene
        {


        }
        if(CreateBlockObject)
        {
            foreach (Transform child in BlocksParent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        DefineBlock();

        EntryPoints();

        SetRoute();

    }

    //Create Map//
    
    private void SetRoute()
    {
        //Track route
        List<int> BlackBlockRoute = new List<int>();
        List<int> WhiteBlockRoute = new List<int>();

        int BlackListPointer = 0;
        int WhiteListPointer = 0;

        int currentPossBlack = 0;
        int currentPossWhite = 0;
        if (RandomStart)
        {
            while(currentPossBlack == currentPossWhite)
            {
                Debug.Log("Block Length  " + block.Length);
                currentPossBlack = Random.Range(0, block.Length);
                currentPossWhite = Random.Range(0, block.Length); ;
            }
            
            BlackBlockRoute.Add(currentPossBlack);
            UpdateBlockMovement(currentPossBlack, "Black");
            
            WhiteBlockRoute.Add(currentPossWhite);
            UpdateBlockMovement(currentPossWhite, "White");
        }
        else
        {
            //Two Entry Points
            currentPossBlack = RedEntryPoint;
            BlackBlockRoute.Add(currentPossBlack);
            UpdateBlockMovement(currentPossBlack, "Black");
            currentPossWhite = GreenEntryPoint;
            WhiteBlockRoute.Add(currentPossWhite);
            UpdateBlockMovement(currentPossWhite, "White");
        }


        if (CreateBlockObject)
        {
            GameObject obj = Instantiate(BlackBlock, block[currentPossBlack].Possition, Quaternion.identity);
            obj.transform.parent = BlocksParent.transform;
            obj = Instantiate(WhiteBlock, block[currentPossWhite].Possition, Quaternion.identity);
            obj.transform.parent = BlocksParent.transform;
        }

        block[currentPossBlack].CapturedBy = "Black";
        block[currentPossWhite].CapturedBy = "White";
        
        ///Test///
        ///cure


        while (BlackListPointer >= 0 || WhiteListPointer >= 0)
        {   
            //Black Route
            int nextPossLeft = RandomMove(currentPossBlack, BlackBlock, "Black");


            if (nextPossLeft == currentPossBlack)// didnt find any available block to move
            {
                BlackListPointer--;              //Go To previews Position
                if (BlackListPointer >= 0)
                {
                    currentPossBlack = BlackBlockRoute[BlackListPointer];
                    
                }

            }
            else if (BlackListPointer >= 0)
            {
                BlackBlockRoute.Add(currentPossBlack);
                BlackListPointer = BlackBlockRoute.Count - 1;
                currentPossBlack = nextPossLeft;
            }
            
            
            //White Route
            int nextPossRight = RandomMove(currentPossWhite, WhiteBlock, "White");

            if (nextPossRight == currentPossWhite)// didnt find any available block to move
            {
                WhiteListPointer--;
                if (WhiteListPointer >= 0)
                {
                    currentPossWhite = WhiteBlockRoute[WhiteListPointer];
                    
                }
            }
            else
            {
                if (WhiteListPointer >= 0)
                {
                    WhiteBlockRoute.Add(currentPossWhite);
                    WhiteListPointer = WhiteBlockRoute.Count - 1;
                    currentPossWhite = nextPossRight;
                }
            }

        }

        print("Set Route Finished");
        ChoseLinkWall();


        //int blackGoalPoint = numberBlocksX * numberBlocksY - RedEntryPoint - 1;
        //int whiteGoalPoint = numberBlocksX * numberBlocksY - GreenEntryPoint - 1;

        
        createPath.DefinePath(block, numberBlocksX, RedEntryPoint, RedGoalPoint, GreenGoalPoint, GreenEntryPoint);

    }

   
    private int RandomMove(int poss, GameObject blk, string route)
    {
        UpdateBlockMovement(poss, route);

        List<string> availableMoves = new List<string>();

        if (block[poss]._leftMove == true)
        {
            availableMoves.Add("Left");
        }
        if (block[poss]._upMove == true)
        {
            availableMoves.Add("Up");
        }
        if (block[poss]._rightMove == true)
        {
            availableMoves.Add("Right");
        }
        if (block[poss]._downMove == true)
        {
            availableMoves.Add("Down");
        }


        if (availableMoves.Count > 0)
        {
            int move = Random.Range(0, availableMoves.Count);

            int nextPoss = ReturnNextPosition(availableMoves[move], poss);

            if (CreateBlockObject)
            {
                GameObject obj = Instantiate(blk, block[nextPoss].Possition, Quaternion.identity);
                obj.transform.parent = BlocksParent.transform;
            }

            PreventBackTracking(nextPoss, availableMoves[move]);
            RemoveWall(poss, availableMoves[move]);
            block[nextPoss].CapturedBy = route;

            return nextPoss;
        }
        else
        {
            return poss;
        }

    }


    private void PreventBackTracking(int poss, string move) //poss = Current position, move = from where you came
    {
        if (move == "Left")
        {
            block[poss]._rightMove = false;
            //block[poss].RightMove = false;
        }
        else if (move == "Up") // Up
        {
            block[poss]._downMove = false;
            //block[poss].DownMove = false;
        }

        else if (move == "Right") // Right
        {
            block[poss]._leftMove = false;
            //block[poss].LeftMove = false;
        }
        else if (move == "Down") // Down
        {
            block[poss]._upMove = false;
            //block[poss].UpMove = false;

        }
    }
    private void UpdateBlockMovement(int poss, string route)
    {
       
        
        if (block[poss]._leftMove)
        {

            if (block[poss - 1].CapturedBy != null) //The block at the left side is captured
            {
                block[poss]._leftMove = false;
                if (block[poss - 1].CapturedBy == block[poss].CapturedBy)// The block at the left captured by the same route
                {
                    if (!VerticalWalls[poss].gameObject.GetComponent<Wall>().isDestroyed) //If there is a wall between them
                    {
                        block[poss].LeftMove = false;
                    }
                }
                else
                {
                    AddLinkWall(poss, "Left");
                    block[poss].LeftMove = false;
                }

            }
        }
        if (block[poss]._upMove)
        {

            if (block[poss + numberBlocksX].CapturedBy != null) // Up
            {
                block[poss]._upMove = false;
                if (block[poss + numberBlocksX].CapturedBy == block[poss].CapturedBy)
                {
                    if (!HorizontalWalls[poss + numberBlocksX].gameObject.GetComponent<Wall>().isDestroyed)
                    {
                        block[poss].UpMove = false;
                    }
                }

                else
                {
                    AddLinkWall(poss, "Up");
                    block[poss].UpMove = false;
                }
            }
        }
        if (block[poss]._rightMove)
        {

            if (block[poss + 1].CapturedBy != null) //Right
            {
                block[poss]._rightMove = false;
                if (block[poss + 1].CapturedBy == block[poss].CapturedBy)
                {
                    if (!VerticalWalls[poss + 1].gameObject.GetComponent<Wall>().isDestroyed)
                    {
                        block[poss].RightMove = false;
                    }

                }
                else
                {
                    block[poss].RightMove = false;
                    AddLinkWall(poss, "Right");
                }
            }
        }
        if (block[poss]._downMove)
        {

            if (block[poss - numberBlocksX].CapturedBy != null) //Down
            {
                block[poss]._downMove = false;
                if (block[poss - numberBlocksX].CapturedBy == block[poss].CapturedBy)
                {
                    if (!HorizontalWalls[poss].gameObject.GetComponent<Wall>().isDestroyed)
                    {
                        block[poss].DownMove = false;
                    }
                }
                else
                {
                    block[poss].DownMove = false;

                    AddLinkWall(poss, "Down");

                }
            }
        }


    }


    private int ReturnNextPosition(string move, int poss)
    {
        if (move == "Left")
        {
            poss--;
            
            return poss;
        }
        else if (move == "Up") // Up
        {
            poss += numberBlocksX;
            
            return poss;
        }
        
        else if (move == "Right") // Right
        {
            poss++;
            
            return poss;
        }
        else if (move == "Down") // Down
        {
            poss -= numberBlocksX;
            
            return poss;
            
        }
        else
        {
            Debug.LogError("Danger!! No Move Available");
            return -1;
        }
    }

    private void EntryPoints()
    {
        RedEntryPoint = 0;
        GreenEntryPoint = 0;
        RedGoalPoint = 0;
        GreenGoalPoint = 0;
        
        while (RedEntryPoint == GreenGoalPoint)
        {
            RedEntryPoint = Random.Range(0, numberBlocksX);
            RedGoalPoint = block.Length - RedEntryPoint - 1;
            
            GreenGoalPoint = Random.Range(0, numberBlocksX);
            GreenEntryPoint = block.Length - GreenGoalPoint - 1;
            
        }

        float sizeX = (float)sizeSpaceX / numberBlocksX;
        float sizeY = (float)sizeSpaceY / numberBlocksY;
        sizeY *= .7f;

        if (_redEntryPoint == null)
        {
            //Entry Red Point
            _redEntryPoint = Instantiate(BlackEntry, block[RedEntryPoint].Possition, Quaternion.Euler(0, 0, -90));
            _redEntryPoint.transform.localScale = new Vector2(sizeX, sizeY);
            
            //Goal Red Point
            _redGoalPoint = Instantiate(BlackEntry, block[RedGoalPoint].Possition, Quaternion.Euler(0, 0, -90));
            _redGoalPoint.transform.localScale = new Vector2(sizeX, sizeY);
            
            //Entry Green Point
            _greenEntryPoint = Instantiate(WhiteEntry, block[GreenEntryPoint].Possition, Quaternion.Euler(0, 0, -90));
            _greenEntryPoint.transform.localScale = new Vector2(sizeX, sizeY);
            
            //Goal Green Point
            _greenGoalPoint = Instantiate(WhiteEntry, block[GreenGoalPoint].Possition, Quaternion.Euler(0, 0, -90));
            _greenGoalPoint.transform.localScale = new Vector2(sizeX, sizeY);
            
        }
        else
        {
            _redEntryPoint.transform.position = block[RedEntryPoint].Possition;
            _redGoalPoint.transform.position = block[RedGoalPoint].Possition;
            _greenEntryPoint.transform.position = block[GreenEntryPoint].Possition;
            _greenGoalPoint.transform.position = block[GreenGoalPoint].Possition;
        }
        
        
        //Wall Remove
        RemoveWall(RedEntryPoint, "Down");
        UpWalls[UpWalls.Length - RedEntryPoint - 1].gameObject.GetComponent<Wall>().Dissolve();
        RemoveWall(GreenGoalPoint, "Down");
        UpWalls[UpWalls.Length - GreenGoalPoint - 1].gameObject.GetComponent<Wall>().Dissolve();
    }
    

    private void DestroyBlocksObjects()
    {
        Destroy(BlocksParent);
    }

   


    //void OnDrawGizmos()
    //{
    //    if (showBlockNumbers)
    //    {
    //        for (int i = 0; i < block.Length; i++)
    //        {

    //            Handles.Label(block[i].Possition, i.ToString());
    //        }
    //    }

    //}
}

[System.Serializable]
public class Block
{
    //Searching Movement//
    public bool _leftMove = true;
    public bool _upMove = true;
    public bool _rightMove = true;
    public bool _downMove = true;

    public string CapturedBy = null;

    public Vector2 Possition = new Vector2(0,0);

    //Create Path
    public bool LeftMove = true;
    public bool UpMove = true;
    public bool RightMove = true;
    public bool DownMove = true;


}


