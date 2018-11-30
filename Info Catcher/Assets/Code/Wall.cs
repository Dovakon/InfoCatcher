using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL ("http://thomasmountainborn.com/2016/05/25/materialpropertyblocks/")]

public class Wall : MonoBehaviour {

    public bool isDestroyed = false;

    private float DissolveSpeed = 1, Offset;
    public float minDissolveSpeed;
    public float maxDissolveSpeed;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private bool StartDissolve = false;
    private bool Reapear = false;
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
           
            t += (Time.deltaTime * DissolveSpeed);
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

        DissolveSpeed = Random.Range(minDissolveSpeed, maxDissolveSpeed);
        StartDissolve = true;
        isDestroyed = true;
    }

    public void ReappearWallSprite()
    {
        isDestroyed = false;
        t = 0;
        _renderer.GetPropertyBlock(_propBlock);
        
        _propBlock.SetFloat("_DissolveAmount", 0);
        
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
        
    }
}
