using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockMovement", menuName = "Block")]
public class BlockMovement : ScriptableObject {

    Movement[] Momements;

}


[System.Serializable]
public class Movement
{
    public bool CanMoveLeft = true;
    public bool CanMoveUp = true;
    public bool CanMoveright = true;
    public bool CanMoveDown = true;
}