
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateMap : MonoBehaviour {

    [Range(0f, 3)]
    public float GenerateTime;

    public float sizeSpaceX;
    public float sizeSpaceY;
    public int numberBlocksX;
    public int numberBlocksY;

    public GameObject BlackBlock;
    public GameObject WhiteBlock;
    public GameObject HorizontalWallObject;
    public GameObject VerticalWallObject;
    public GameObject BlackEntry;
    public GameObject WhiteEntry;
    public GameObject BlocksParent;
    public CreatePath createPath;

    //Blocks//
    private Block[] block;
    private int BlackEntryPoint = 0;
    private int WhiteEntryPoint = 0;
    private bool AllBlocksSpawned = false;


    //Walls//
    private int numberWallsX;
    private int numberWallsY;
    private GameObject[] VerticalWalls;
    private GameObject[] HorizontalWalls;
    private GameObject[] UpWalls, RightWalls;
    private List<GameObject> LinkWalls;
    private MaterialPropertyBlock _propBlock;
    private bool dissolveWalls = false;
    //....///
    public bool insta;
    public bool CreateBlockObject;
    private bool showBlockNumbers = false;
    void Start()
    {
        _propBlock = new MaterialPropertyBlock();
        _propBlock.SetFloat("_DissolveAmount", 1);
        LinkWalls = new List<GameObject>();

        DefineBlock();
        InstantiateWalls();

        EntryPoints();

        if (insta)
        {
            SetRouteInsta();
        }
        else
        {
            StartCoroutine(SetRoute());
        }


    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("Space");
            DestroyBlocksObjects();
        }
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
            VerticalWalls[poss].gameObject.GetComponent<Wall>().isDestroyed = true;
            VerticalWalls[poss].gameObject.GetComponent<Wall>().Dissolve();
        }
        if (move == "Up")
        {
            HorizontalWalls[poss + numberBlocksX].gameObject.GetComponent<Wall>().isDestroyed = true;
            HorizontalWalls[poss + numberBlocksX].GetComponent<Wall>().Dissolve();
        }
        if (move == "Right")
        {
            VerticalWalls[poss + 1].gameObject.GetComponent<Wall>().isDestroyed = true;
            VerticalWalls[poss + 1].gameObject.GetComponent<Wall>().Dissolve();
        }
        if (move == "Down")
        {
            HorizontalWalls[poss].gameObject.GetComponent<Wall>().isDestroyed = true;
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
                Destroy(LinkWalls[wall].gameObject);
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
                Destroy(LinkWalls[wall].gameObject);
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


    //Create Map//
    private IEnumerator SetRoute()
    {
        //Track route
        List<int> BlackBlockRoute = new List<int>();
        List<int> WhiteBlockRoute = new List<int>();

        int BlackListPointer = 0;
        int WhiteListPointer = 0;

        //Two Entry Points
        int currentPossBlack = BlackEntryPoint;
        BlackBlockRoute.Add(currentPossBlack);
        UpdateBlockMovement(currentPossBlack, "Black");
        int currentPossWhite = WhiteEntryPoint;
        WhiteBlockRoute.Add(currentPossWhite);
        UpdateBlockMovement(currentPossWhite, "White");

        if (CreateBlockObject)
        {
            GameObject obj = Instantiate(BlackBlock, block[currentPossBlack].Possition, Quaternion.identity);
            obj.transform.parent = BlocksParent.transform;
            obj = Instantiate(WhiteBlock, block[currentPossWhite].Possition, Quaternion.identity);
            obj.transform.parent = BlocksParent.transform;
        }

        block[currentPossBlack].CapturedBy = "Black";
        block[currentPossWhite].CapturedBy = "White";
        yield return new WaitForSeconds(GenerateTime);

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


        int blackGoalPoint = numberBlocksX * numberBlocksY - BlackEntryPoint - 1;
        int whiteGoalPoint = numberBlocksX * numberBlocksY - WhiteEntryPoint - 1;

        dissolveWalls = true;

        yield return new WaitForSeconds(5);
        StartCoroutine(createPath.DefinePath(block, numberBlocksX, BlackEntryPoint, blackGoalPoint, WhiteEntryPoint, whiteGoalPoint));

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
            return -1;
            Debug.LogError("Danger!! No Move Available");
        }
    }

    private bool CheckAllBlockSpawn()
    {
        bool allspawned = true;
            
        foreach (var blk in block)
        {
            if(blk.CapturedBy == null)
            {
                allspawned = false;
            }
        }
        return allspawned;
    }

    private void EntryPoints()
    {
        while (BlackEntryPoint == WhiteEntryPoint)
        {
            BlackEntryPoint = Random.Range(0, numberBlocksX);
            WhiteEntryPoint = Random.Range(0, numberBlocksX);


        }

        float sizeX = (float)sizeSpaceX / numberBlocksX;
        float sizeY = (float)sizeSpaceY / numberBlocksY;
        sizeY *= .7f;
        //Entry Black Point
        GameObject obj = Instantiate(BlackEntry, block[BlackEntryPoint].Possition, Quaternion.Euler(0, 0, -90));
        obj.transform.localScale = new Vector2(sizeX, sizeY);
        Destroy(HorizontalWalls[BlackEntryPoint].gameObject);
        //Goal Black Point
        obj = Instantiate(BlackEntry, block[block.Length - BlackEntryPoint - 1].Possition, Quaternion.Euler(0, 0, -90));
        obj.transform.localScale = new Vector2(sizeX, sizeY);
        Destroy(UpWalls[UpWalls.Length - BlackEntryPoint - 1].gameObject);

        //Entry White Point
        obj = Instantiate(WhiteEntry, block[WhiteEntryPoint].Possition, Quaternion.Euler(0, 0, -90));
        obj.transform.localScale = new Vector2(sizeX, sizeY);
        Destroy(HorizontalWalls[WhiteEntryPoint].gameObject);
        //Goal White Point
        obj = Instantiate(WhiteEntry, block[block.Length - WhiteEntryPoint - 1].Possition, Quaternion.Euler(0, 0, -90));
        obj.transform.localScale = new Vector2(sizeX, sizeY);
        Destroy(UpWalls[UpWalls.Length - WhiteEntryPoint - 1].gameObject);
    }





    private void DestroyBlocksObjects()
    {
        Destroy(BlocksParent);
    }

    private void SetRouteInsta()
    {
        //Track route
        List<int> BlackBlockRoute = new List<int>();
        List<int> WhiteBlockRoute = new List<int>();

        int BlackListPointer = 0;
        int WhiteListPointer = 0;

        //Two Entry Points
        int currentPossBlack = BlackEntryPoint;
        BlackBlockRoute.Add(currentPossBlack);
        UpdateBlockMovement(currentPossBlack, "Black");
        int currentPossWhite = WhiteEntryPoint;
        WhiteBlockRoute.Add(currentPossWhite);
        UpdateBlockMovement(currentPossWhite, "White");

        if (CreateBlockObject)
        {
            GameObject obj = Instantiate(BlackBlock, block[currentPossBlack].Possition, Quaternion.identity);
            obj.transform.parent = BlocksParent.transform;
            obj = Instantiate(WhiteBlock, block[currentPossWhite].Possition, Quaternion.identity);
            obj.transform.parent = BlocksParent.transform;
        }

        block[currentPossBlack].CapturedBy = "Black";
        block[currentPossWhite].CapturedBy = "White";

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
                    //BlackBlockRoute.Add(currentPossBlack);
                    //CheckAllBlockSpawn();
                }

            }
            else if (BlackListPointer >= 0)
            {
                BlackBlockRoute.Add(currentPossBlack);
                BlackListPointer = BlackBlockRoute.Count - 1;
                currentPossBlack = nextPossLeft;
            }

            //print(BlackListPointer);

           

            //White Route
            int nextPossRight = RandomMove(currentPossWhite, WhiteBlock, "White");

            if (nextPossRight == currentPossWhite)// didnt find any available block to move
            {
                WhiteListPointer--;
                if (WhiteListPointer >= 0)
                {
                    currentPossWhite = WhiteBlockRoute[WhiteListPointer];
                    //WhiteBlockRoute.Add(currentPossWhite);
                    //CheckAllBlockSpawn();
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


        int blackGoalPoint = numberBlocksX * numberBlocksY - BlackEntryPoint - 1;
        int whiteGoalPoint = numberBlocksX * numberBlocksY - WhiteEntryPoint - 1;

        StartCoroutine(createPath.DefinePath(block, numberBlocksX, BlackEntryPoint, blackGoalPoint, WhiteEntryPoint, whiteGoalPoint));

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


