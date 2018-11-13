
using System.Collections;
using System.Collections.Generic;
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
    public GameObject HorizontalWall;
    public GameObject VerticalWall;
    public GameObject BlackArrow;
    public GameObject WhiteArrow;
    public GameObject BlocksParent;
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
    //....///

    void Start ()
    {
        LinkWalls = new List<GameObject>();


        
        DefineBlock();
        InstantiateWalls();


        EntryPoints();
        StartCoroutine(SetRoute());
    }

    void Update()
    {
         if(Input.GetKeyDown("space"))
            {
                DestroyBlocksObjects();
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
            GameObject wallobj = Instantiate(HorizontalWall, new Vector2(Xposs, Yposs), HorizontalWall.transform.rotation);
            wallobj.transform.localScale = new Vector2(wallobj.transform.localScale.x, wallEveryX);
            UpWalls[i] = wallobj;
            Xposs += wallEveryX;

        }
        Xposs = sizeSpaceX;
        Yposs = wallEveryY * .5f;
        
        for (int i = 0; i < numberBlocksY; i++)
        {
            GameObject wallobj = Instantiate(HorizontalWall, new Vector2(Xposs, Yposs), Quaternion.identity);
            wallobj.transform.localScale = new Vector2(wallobj.transform.localScale.x, wallEveryY);
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
                GameObject wallobj = Instantiate(HorizontalWall,new Vector2 (Xposs, Yposs), HorizontalWall.transform.rotation);
                wallobj.transform.localScale = new Vector2(wallobj.transform.localScale.x, wallEveryX);
                HorizontalWalls[Hcounter] = wallobj;
                Hcounter++;
                Xposs += wallEveryX;
                
            }

            Xposs = 0;
            Yposs += wallEveryY * .5f;
            
            for (int y = 0; y < numberBlocksX; y++)
            {
                GameObject wallobj = Instantiate(VerticalWall, new Vector2(Xposs, Yposs), Quaternion.identity);
                wallobj.transform.localScale = new Vector2(wallobj.transform.localScale.x, wallEveryY);
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
            Destroy(VerticalWalls[poss].gameObject);
        }
        if (move == "Up")
        {
            Destroy(HorizontalWalls[poss + numberBlocksX].gameObject);
        }
        if (move == "Right")
        {
            Destroy(VerticalWalls[poss + 1].gameObject);
        }
        if (move == "Down")
        {
            Destroy(HorizontalWalls[poss].gameObject);
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

    private void DestroyLinkWall()
    {
        int wall = Random.Range(0, LinkWalls.Count);
        Destroy(LinkWalls[wall].gameObject);

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
            block[counter - 1].rightMove = false; // Right
            //**//

        }
    }
    private void WhereCanMove(int blk)
    {

        if(blk % numberBlocksX == 0)  //Left
        {
            block[blk].leftMove = false;
        }
        //if (blk  == numberBlocksX -1) //Right
        //{
        //    block[blk].rightMove = false;
        //}
        if (blk < numberBlocksX)       // Down
        {
            block[blk].downMove = false;
        }
        if (blk >= numberBlocksX * (numberBlocksY - 1))     // UP
        {
            block[blk].upMove = false;
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
        int currentPossWhite = WhiteEntryPoint;
        WhiteBlockRoute.Add(currentPossWhite);
        
        GameObject obj = Instantiate(BlackBlock, block[currentPossBlack].Possition, Quaternion.identity);
        obj.transform.parent = BlocksParent.transform;
        block[currentPossBlack].CapturedBy = "Black";
        obj = Instantiate(WhiteBlock, block[currentPossWhite].Possition, Quaternion.identity);
        obj.transform.parent = BlocksParent.transform;
        block[currentPossWhite].CapturedBy = "White";

        while(!AllBlocksSpawned)
        {
            yield return new WaitForSeconds(GenerateTime);
            //Black Route
            int nextPossLeft = RandomMove(currentPossBlack, BlackBlock, "Black");


            if (nextPossLeft == currentPossBlack)// didnt find any available block to move
            {
                BlackListPointer--;
                if (BlackListPointer >= 0)
                {
                    currentPossBlack = BlackBlockRoute[BlackListPointer];
                    BlackBlockRoute.Add(currentPossBlack);
                    CheckAllBlockSpawn();
                }

            }
            else if (BlackListPointer >= 0)
            {
                BlackBlockRoute.Add(currentPossBlack);
                BlackListPointer = BlackBlockRoute.Count - 1;
                currentPossBlack = nextPossLeft;
            }
            
           
            yield return new WaitForSeconds(GenerateTime);
            //White Route
            int nextPossRight = RandomMove(currentPossWhite, WhiteBlock, "White");

            if (nextPossRight == currentPossWhite)// didnt find any available block to move
            {
                WhiteListPointer--;
                if (WhiteListPointer >= 0)
                {
                    currentPossWhite = WhiteBlockRoute[WhiteListPointer];
                    WhiteBlockRoute.Add(currentPossWhite);
                    CheckAllBlockSpawn();
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
        DestroyLinkWall();
    }

    private int RandomMove(int poss, GameObject blk, string route)
    {

        List<string> availableMoves = new List<string>();

        if (block[poss].leftMove == true)
        {
            if (block[poss - 1].CapturedBy == null) //Left
            {
                availableMoves.Add("Left");
            }
            else
            {
                block[poss].leftMove = false;
                if (block[poss].CapturedBy != block[poss - 1].CapturedBy)
                {
                    AddLinkWall(poss, "Left");
                }
            }
           
        }
        if (block[poss].upMove == true)
        {
            if (block[poss + numberBlocksX].CapturedBy == null) // Up
            {
                availableMoves.Add("Up");
            }
            else
            {
                block[poss].upMove = false;
                if (!(block[poss].CapturedBy == block[poss + numberBlocksX].CapturedBy))
                {
                    AddLinkWall(poss, "Up");
                }
            }
        }
        if (block[poss].rightMove == true)
        {
            if (block[poss + 1].CapturedBy == null) //Right
            {
                availableMoves.Add("Right");
            }
            else
            {
                block[poss].rightMove = false;
                if (!(block[poss].CapturedBy == block[poss + 1].CapturedBy))
                {
                    AddLinkWall(poss, "Right");
                }
            }
        }
        if (block[poss].downMove == true)
        {
            if (block[poss - numberBlocksX].CapturedBy == null) //Down
            {
                availableMoves.Add("Down");
            }
            else
            {
                block[poss].downMove = false;
                if(!(block[poss].CapturedBy == block[poss - numberBlocksX].CapturedBy))
                {
                    AddLinkWall(poss, "Down");
                }
            }
        }


        if (availableMoves.Count == 0)
        {
            if (CheckAllBlockSpawn())
            {
                AllBlocksSpawned = true;
                return 0;
            }
            else
            {
                return poss;
            }

        }
        else
        {
            int move = Random.Range(0, availableMoves.Count);

            RemoveWall(poss, availableMoves[move]);
            poss = ReturnNextPosition(availableMoves[move], poss, block[poss].CapturedBy);
            GameObject obj = Instantiate(blk, block[poss].Possition, Quaternion.identity);
            obj.transform.parent = BlocksParent.transform;
            return poss;
        }
        
    }

    private int ReturnNextPosition(string move, int poss, string route)
    {
        if (move == "Left")
        {
            poss--;
            block[poss].CapturedBy = route;
            return poss;
        }
        else if (move == "Up") // Up
        {
            poss += numberBlocksX;
            block[poss].CapturedBy = route;
            return poss;
        }
        
        else if (move == "Right") // Right
        {
            poss++;
            block[poss].CapturedBy = route;
            return poss;
        }
        else if (move == "Down") // Down
        {
            poss -= numberBlocksX;
            block[poss].CapturedBy = route;
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
       
        Instantiate(BlackArrow, block[BlackEntryPoint].Possition - new Vector2(0, .8f), Quaternion.Euler(0, 0, -90));
        Destroy(HorizontalWalls[BlackEntryPoint].gameObject);
        Instantiate(BlackArrow, block[block.Length - BlackEntryPoint - 1].Possition + new Vector2(0, .8f), Quaternion.Euler(0, 0, -90));
        Destroy(UpWalls[UpWalls.Length - BlackEntryPoint - 1].gameObject);


        Instantiate(WhiteArrow, block[WhiteEntryPoint].Possition - new Vector2(0, .8f), Quaternion.Euler(0, 0, -90));
        Destroy(HorizontalWalls[WhiteEntryPoint].gameObject);
        Instantiate(WhiteArrow, block[block.Length - WhiteEntryPoint - 1].Possition + new Vector2(0, .8f), Quaternion.Euler(0, 0, -90));
        Destroy(UpWalls[UpWalls.Length - WhiteEntryPoint - 1].gameObject);
    }

    private void DestroyBlocksObjects()
    {
        Destroy(BlocksParent);
    }

}

[System.Serializable]
public class Block
{
    //Moving//
    public bool leftMove = true;
    public bool upMove = true;
    public bool rightMove = true;
    public bool downMove = true;

    public string CapturedBy = null;

    public Vector2 Possition;
}
    

    