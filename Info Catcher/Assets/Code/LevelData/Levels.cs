using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Levels")]
public class Levels : ScriptableObject {

    [SerializeField]
    private LevelData[] _levels;

    public LevelData TakeLevelData(int currentLevel)
    {
        for (int i = 0; i < _levels.Length ; i++)
        {
            if(currentLevel < _levels[i].MaxLevel)
            {
                return _levels[i];
            }
        }

        return _levels[1];
    }


}

[System.Serializable]
public class LevelData
    {
        
        public string name;
        
        public int MaxLevel;

        public int Xblocks;
        public int Yblocks;

        public int Time;

        public int Traps;

        public bool ShowCounter;
        [Range(.1f, .3f)]
        public float MinDissolveWallTime;
        [Range(.3f, .8f)]
        public float MaxDissolveWallTime;

        public float MapSizeX;
        public float MapSizeY;
}