using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class InfoBullet : MonoBehaviour {

    
    public CreatePath Path;
    public float Speed=1;
    public float MaxDistanceToGoal=.1f;



    private bool CanMove = false;
    private IEnumerator<Vector2> _currentPoint;

    private void OnEnable()
    {
        GameManager.SecondPhaseEvent += StartMoving;
    }

    public void Update()
    {
        if(Input.GetKeyDown("q"))
        {
            StartMoving();
        }
        
        if (CanMove)
        {
            Move();
        }
    }


    public void StartMoving()
    {
        if (Path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        _currentPoint = Path.GetPathEnumerator();
        _currentPoint.MoveNext();

        if (_currentPoint.Current == null)
            return;

        transform.position = _currentPoint.Current;
        CanMove = true;
    }

    private void Move()
    {
        if (_currentPoint == null || _currentPoint.Current == null)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current, Time.deltaTime * Speed);
        
        var distanceSquared = (transform.position - new Vector3(_currentPoint.Current.x, _currentPoint.Current.y, 0)).sqrMagnitude;
        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
            _currentPoint.MoveNext();
    }


    private void OnTriggerEnter2D(Collider2D Trap)
    {
        CanMove = false;
        transform.position = new Vector2(-2, -2);
        GameManager.Instance.ExecuteThirdPhase(true);
    }
}
