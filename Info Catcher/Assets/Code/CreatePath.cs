using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePath : MonoBehaviour {

    List<string> path;
    List<int> possitions;


    public void DefinePath(Block[] block,int numberBlocksX, int blackEntry, int blackGoalPoint, int whiteEntry, int whiteGoalPoint)
    {
        int entrypoint = Random.Range(0, 2);

        print("BlackPoint  " + blackGoalPoint);
        print("whitePoint  " + whiteGoalPoint);

        int goalPoint = entrypoint == 0 ? blackGoalPoint : whiteGoalPoint;
        entrypoint = entrypoint == 0 ? blackEntry : whiteEntry;
        

        print("Start Point:  " + entrypoint);
        print("Goal Point:  " + goalPoint);

        int currentPos = entrypoint;
        path = new List<string>();
        possitions = new List<int>();
        int move;
        int pointer = 0;
        while (currentPos != goalPoint)
        {
            List<string> availableMoves = new List<string>();

            if (block[currentPos].LeftMove == true)
            {
                availableMoves.Add("Left");
            }
            if (block[currentPos].UpMove == true)
            {
                availableMoves.Add("Up");
            }
            if (block[currentPos].RightMove == true)
            {
                availableMoves.Add("Right");
            }
            if (block[currentPos].DownMove == true)
            {
                availableMoves.Add("Down");
            }

            if (availableMoves.Count > 0)
            {
                move = Random.Range(0, availableMoves.Count);
                

                path.Add(availableMoves[move]);
                possitions.Add(currentPos);
                pointer++;
                

                if (availableMoves[move] == "Left")
                {
                    block[currentPos].LeftMove = false;
                }
                else if (availableMoves[move] == "Up")
                {
                    block[currentPos].UpMove = false;
                }
                else if (availableMoves[move] == "Right")
                {
                    block[currentPos].RightMove = false;
                }
                else if (availableMoves[move] == "Down")
                {
                    block[currentPos].DownMove = false;
                }

                int nextPoss = ReturnNextPosition(availableMoves[move], currentPos, numberBlocksX);
                currentPos = nextPoss;
            }
            else
            {
                if (path[pointer] != null)
                {
                    path.RemoveAt(pointer);
                    possitions.RemoveAt(pointer);
                }
                pointer--;

                currentPos = possitions[pointer];
                


            }
        }

        print("Create path Finished");
        
    }

   

    private int ReturnNextPosition(string move, int poss,int numberBlocksX)
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

    }
