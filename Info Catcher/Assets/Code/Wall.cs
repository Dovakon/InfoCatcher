using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    [HideInInspector]public bool isDestroyed = false;

    public float Speed = 1, Offset;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private bool StartDissolve = false; 
    private float dissolveAmount;
    private float t;
    void Awake()
    {

        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (StartDissolve)
        {
            _renderer.GetPropertyBlock(_propBlock);
           
            t += (Time.deltaTime * Speed);
            dissolveAmount = Mathf.Lerp(0, 1, t);
            
            // Assign our new value.
            _propBlock.SetFloat("_DissolveAmount", dissolveAmount);
            // Apply the edited values to the renderer.
            _renderer.SetPropertyBlock(_propBlock);

            if (t > .99f)
                StartDissolve = false; 

        }
    }

    public void Dissolve()
    {

        Speed = Random.RandomRange(.3f, .8f);
        StartDissolve = true;
        
    }
}
