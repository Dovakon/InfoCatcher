
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


    //Walls//
    private int Walls;
    private int numberWallsX;
    private int numberWallsY;
    //....///

    void Start () {
        
        //Walls//
        numberWallsX = numberBlocksX - 1;
        numberWallsY = numberBlocksY - 1;

        float wallEveryX = (float)sizeSpaceX / numberWallsX;
        
        float wallEveryY = (float)sizeSpaceY / numberWallsY;
        //


        block = new Block[numberBlocksX * numberBlocksY];
        float blockEveryX = (float)sizeSpaceX / numberBlocksX;
        float blockEveryY = (float)sizeSpaceY / numberBlocksY;

        //print(blockEveryX + " " + blockEveryY);

        float xposs, yposs;
        xposs = blockEveryX / 2;
        yposs = blockEveryY / 2;
        //print(xposs +" "+ yposs);

        int counter = 0;
        
        for (int y = 0; y < numberBlocksY; y++)
        {

            //block[counter].leftMove = false;
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
            xposs = blockEveryX/2;
            yposs += blockEveryY;

            //Cant Move Right
            //Probably Will Change  
            block[counter - 1].rightMove = false;
            

        }

        
        //SetRoute();

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


    private void SetRoute()
    {
        //Two Start Pints
        int currentPossLeft = 0;
        int currentPossRight = numberBlocksX - 1;
        Instantiate(LeftBlock, block[currentPossLeft].Possition, Quaternion.identity);
        Instantiate(RightBlock, block[currentPossRight].Possition, Quaternion.identity);


        //RandomMove(currentPossLeft, LeftBlock);


    }

    private void RandomMove(int poss, GameObject block)
    {
        bool moveChosen = false;
        while (!moveChosen)
        {
            
            int move = Random.Range(0, 5);

            if (move == 0) //Left
            {
                if ((poss - 1) > 0)
                {
                    poss--;
                    //Instantiate(block, block[poss].Possition, Quaternion.identity);
                    moveChosen = true;
                }

            }
            else if (move == 1) // Up
            {
                if ((poss + numberBlocksX) > numberBlocksX * numberBlocksY)
                {
                    poss += numberBlocksX;
                   // Instantiate(block, BlockPossition[poss], Quaternion.identity);
                    moveChosen = true;
                }

            }
            else if (move == 2) // Right
            {
                if ((poss + 1) > numberBlocksX)
                {
                    poss += numberBlocksX;
                   // Instantiate(block, BlockPossition[poss], Quaternion.identity);
                    moveChosen = true;
                }

            }
        }
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


    public Vector2 Possition;
}
    

    