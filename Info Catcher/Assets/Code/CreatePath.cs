using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePath : MonoBehaviour {

    List<Vector2> RedPoints;
    List<Vector2> GreenPoints;
    
    public void DefinePath(Block[] block,int numberBlocksX, int EntryPoint, int GoalPoint, string ColorPath)
    {
        
        //int entrypoint = Random.Range(0, 2);

        //int goalPoint = entrypoint == 0 ? blackGoalPoint : whiteGoalPoint;
        //entrypoint = entrypoint == 0 ? blackEntry : whiteEntry;
        
      

        //print("Start Point:  " + entrypoint);
        //print("Goal Point:  " + goalPoint);

        int currentPos = EntryPoint;
        List<Vector2> Points = new List<Vector2>();
        List<int> Possitions = new List<int>();
        int move;
        int pointer = 0;
        
        while (currentPos != GoalPoint)
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


                Points.Add(block[currentPos].Possition);
                Possitions.Add(currentPos);
                pointer++;

                int nextPoss = ReturnNextPosition(availableMoves[move], currentPos, numberBlocksX);

                if (availableMoves[move] == "Left")
                {
                    block[currentPos].LeftMove = false;
                    block[nextPoss].RightMove = false; //Prevent Backtracking
                }
                else if (availableMoves[move] == "Up")
                {
                    block[currentPos].UpMove = false;
                    block[nextPoss].DownMove = false;
                }
                else if (availableMoves[move] == "Right")
                {
                    block[currentPos].RightMove = false;
                    block[nextPoss].LeftMove = false;
                }
                else if (availableMoves[move] == "Down")
                {
                    block[currentPos].DownMove = false;
                    block[nextPoss].UpMove = false;
                }


                currentPos = nextPoss;
            }
            else
            {
                pointer--;
                currentPos = Possitions[pointer];
                if (pointer < Points.Count)
                {
                    Points.RemoveAt(pointer);
                    Possitions.RemoveAt(pointer);
                }
                
            }
            
        }

        Points.Add(block[GoalPoint].Possition);
        Possitions.Add(GoalPoint);

        if(ColorPath == "Red")
        {
            RedPoints = Points;
        }
        else if(ColorPath == "Green")
        {
            GreenPoints = Points;
        }


        print("Create path Finished");


        /////////////////////////////////////////


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


  

    public IEnumerator<Vector2> GetPathEnumerator(string colorPath)
    {

        List<Vector2> Points = new List<Vector2>();

        if (colorPath == "Red")
        {
            Points = RedPoints;
        }
        else if(colorPath == "Green")
        {
            Points = GreenPoints;
        }
        //if (Points == null || Points.Length < 1)
        //    yield break;

        var direction = 1;
        var index = 0;
        while (true)
        {
            
            yield return Points[index];

            if (Points.Count == 1)
                continue;

            //if Arrive to GoalPoint
            else if (index >= Points.Count - 1)
            {
                //GameManager.Instance.ExecuteThirdPhase(false);
                break;
            }  

            index = index + direction;
        }

    }
   
}
