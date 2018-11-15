using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePath : MonoBehaviour {
    


    
    public void DefinePath(Block[] block, int blackEntry, int whiteEntry)
    {
        int entrypoint = Random.Range(0, 2);

        entrypoint = entrypoint == 0 ? blackEntry : whiteEntry;

        print("Start Point:  " + entrypoint);

    }

}
