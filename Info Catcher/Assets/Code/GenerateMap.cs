
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour {

    public int sizeSpaceX;
    public int sizeSpaceY;
    

    public GameObject LeftBlock;
    public GameObject RightBlock;

    //Blocks//
    public int numberBlocksX;
    public int numberBlocksY;
    private Block[] block;
    private bool AllBlocksSpawned = false;

    //Walls//
    private int Walls;
    private int numberWallsX;
    private int numberWallsY;
    //....///

    void Start () {
        
        //Walls//
        numberWallsX = numberBlocksX - 1;
        numberWallsY = numberBlocksY - 1;



        DefineBlock();
        
        
        StartCoroutine(SetRoute());

    }

    private void DefineBlock()
    {

        float wallEveryX = (float)sizeSpaceX / numberWallsX;
        float wallEveryY = (float)sizeSpaceY / numberWallsY;
        
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

                WhereToMove(counter);
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

    private void WhereToMove(int blk)
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
        int currentPossRight = numberBlocksX - 1;
        RightBlockRoute.Add(currentPossRight);
        
        Instantiate(LeftBlock, block[currentPossLeft].Possition, Quaternion.identity);
        block[currentPossLeft].isCaptured = true;
        Instantiate(RightBlock, block[currentPossRight].Possition, Quaternion.identity);
        block[currentPossRight].isCaptured = true;

        for (int i = 0; i < 500; i++)
        {
            yield return new WaitForSeconds(.5f);
            //Left Route
            int nextPossLeft = RandomMove(currentPossLeft, LeftBlock);
            if(nextPossLeft == currentPossLeft)// didnt find any available block to move
            {
                leftListPointer--;
                currentPossLeft = LeftBlockRoute[leftListPointer];
                LeftBlockRoute.Add(currentPossLeft);
            }
            else
            {
                LeftBlockRoute.Add(currentPossLeft);
                leftListPointer = LeftBlockRoute.Count;
                currentPossLeft = nextPossLeft;
            }

            yield return new WaitForSeconds(.5f);
            //Right Route
            int nextPossRight = RandomMove(currentPossRight, RightBlock);
            if (nextPossRight == currentPossRight)// didnt find any available block to move
            {
                rightListPointer--;
                currentPossRight = RightBlockRoute[rightListPointer];
                RightBlockRoute.Add(currentPossRight);
            }
            else
            {
                RightBlockRoute.Add(currentPossRight);
                rightListPointer = RightBlockRoute.Count;
                currentPossRight = nextPossRight;
            }
        }
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

    private void test()
    {
        bool moveChosen = true;
        while (moveChosen)
        {

            int move = Random.Range(0, 5);

            if (moveChosen)
            {
                if (moveChosen)
                {
                   print("prwto if");
                    break; 

                }
                if(moveChosen)
                {
                    print("Deutero if");
                    break;
                }

            }
            print("While");
            moveChosen = false;
        }
        print("teleuteo");
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
    

    