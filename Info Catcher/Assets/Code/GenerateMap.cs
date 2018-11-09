
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour {

    [Range(0f, 3)]
    public float GenerateTime;

    public float sizeSpaceX;
    public float sizeSpaceY;
    

    public GameObject LeftBlock;
    public GameObject RightBlock;
    public GameObject HorizontalWall;
    public GameObject VerticalWall;

    //Blocks//
    public int numberBlocksX;
    public int numberBlocksY;
    private Block[] block;
    
    private bool AllBlocksSpawned = false;

    //Walls//
    private int numberWallsX;
    private int numberWallsY;
    private GameObject[] VerticalWalls;
    private GameObject[] HorizontalWalls;
    //....///

    void Start ()
    {   
        InstantiateWalls();
        DefineBlock();
        
        StartCoroutine(SetRoute());
    }



    //Walls
    private void InstantiateWalls()
    {
        //Walls//
        numberWallsX = numberBlocksX;
        numberWallsY = numberBlocksX;

        VerticalWalls = new GameObject[numberBlocksX * numberBlocksY];
        HorizontalWalls = new GameObject[numberBlocksX * numberBlocksY];



        float wallEveryX = (float)sizeSpaceX / numberBlocksX;
        float wallEveryY = (float)sizeSpaceY / numberBlocksY;

        
        int Vcounter = 0;
        int Hcounter = 0;
        //Vector2 poss = new Vector2(wallEveryX, wallEveryY);
        
        float Xposs = wallEveryY * .5f;
        float Yposs = 0f;

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
`   private void WhereCanMove(int blk)
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
        List<int> LeftBlockRoute = new List<int>();
        List<int> RightBlockRoute = new List<int>();
        
        int leftListPointer = 0;
        int rightListPointer = 0;

        //Two Start Points
        int currentPossLeft = 0;
        LeftBlockRoute.Add(currentPossLeft);
        int currentPossRight = 18;
        RightBlockRoute.Add(currentPossRight);
        
        Instantiate(LeftBlock, block[currentPossLeft].Possition, Quaternion.identity);
        block[currentPossLeft].isCaptured = true;
        Instantiate(RightBlock, block[currentPossRight].Possition, Quaternion.identity);
        block[currentPossRight].isCaptured = true;

        while(!AllBlocksSpawned)
        {
            yield return new WaitForSeconds(GenerateTime);
            //Left Route
            int nextPossLeft = RandomMove(currentPossLeft, LeftBlock);


            if (nextPossLeft == currentPossLeft)// didnt find any available block to move
            {
                leftListPointer--;
                if (leftListPointer >= 0)
                {
                    currentPossLeft = LeftBlockRoute[leftListPointer];
                    LeftBlockRoute.Add(currentPossLeft);
                    CheckAllBlockSpawn();
                }

            }
            else if (leftListPointer >= 0)
            {
                LeftBlockRoute.Add(currentPossLeft);
                leftListPointer = LeftBlockRoute.Count - 1;
                currentPossLeft = nextPossLeft;
            }
            
           
            yield return new WaitForSeconds(GenerateTime);
            //Right Route
            int nextPossRight = RandomMove(currentPossRight, RightBlock);

            if (nextPossRight == currentPossRight)// didnt find any available block to move
            {
                rightListPointer--;
                if (rightListPointer >= 0)
                {
                    currentPossRight = RightBlockRoute[rightListPointer];
                    RightBlockRoute.Add(currentPossRight);
                    CheckAllBlockSpawn();
                }
            }
            else
            {
                if (rightListPointer >= 0)
                {
                    RightBlockRoute.Add(currentPossRight);
                    rightListPointer = RightBlockRoute.Count - 1;
                    currentPossRight = nextPossRight;
                }
            }
        }

        print("Set Route Finished");
    }

    private int RandomMove(int poss, GameObject blk)
    {

        List<string> availableMoves = new List<string>();
        if (block[poss].leftMove == true)
        {
            if (!block[poss - 1].isCaptured) //Left
            {
                availableMoves.Add("Left");
            }
           
        }
        if (block[poss].upMove == true)
        {
            if (!block[poss + numberBlocksX].isCaptured) // Up
            {
                availableMoves.Add("Up");
            }
           
        }
        if (block[poss].rightMove == true)
        {
            if (!block[poss + 1].isCaptured) //Right
            {
                availableMoves.Add("Right");
            }
           
        }
        if (block[poss].downMove == true)
        {
            if (!block[poss - numberBlocksX].isCaptured) //Down
            {
                availableMoves.Add("Down");
            }
           
        }


        if (availableMoves.Count == 0)
        {
            if(CheckAllBlockSpawn())
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
            poss = ChoseMovement(availableMoves[move], poss);
            Instantiate(blk, block[poss].Possition, Quaternion.identity);
            return poss;
        }
        
    }

    private int ChoseMovement(string move, int poss)
    {
        if (move == "Left")
        {
            poss--;
            block[poss].isCaptured = true;
            return poss;
        }
        else if (move == "Up") // Up
        {
            poss += numberBlocksX;
            block[poss].isCaptured = true;
            return poss;
        }
        
        else if (move == "Right") // Right
        {
            poss++;
            block[poss].isCaptured = true;
            return poss;
        }
        else if (move == "Down") // Down
        {
            poss -= numberBlocksX;
            block[poss].isCaptured = true;
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
            if(blk.isCaptured == false)
            {
                allspawned = false;
            }
        }
        return allspawned;
    }
    
    private void ChoseLink()
    {

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

    public bool isCaptured;

    public Vector2 Possition;
}
    

    