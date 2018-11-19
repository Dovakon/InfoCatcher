using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {

    public enum FollowType
    {
        MoveTowards,
        Lerp

    }

    public FollowType Type=FollowType.MoveTowards;
    public CreatePath Path;
    public float Speed=1;
    public float MaxDistanceToGoal=.1f;

    private IEnumerator<Vector2> _currentPoint;
     
    public void Start()
    {
        if (Path==null)
        {
            Debug.LogError("Path cannot be null",gameObject);
            return;
        }

        _currentPoint=Path.GetPathEnumerator();
        _currentPoint.MoveNext();

        if(_currentPoint.Current==null)
            return;

        transform.position = _currentPoint.Current;
    }

    public void Update()
    {
        if (_currentPoint == null || _currentPoint.Current == null)
            return;
        
        if (Type == FollowType.MoveTowards)
            transform.position = Vector3.MoveTowards (transform.position, _currentPoint.Current, Time.deltaTime * Speed);
        else if (Type == FollowType.Lerp)
            transform.position = Vector3.Lerp(transform.position, _currentPoint.Current, Time.deltaTime * Speed);
        
        var distanceSquared = (transform.position - new Vector3(_currentPoint.Current.x, _currentPoint.Current.y, 0)).sqrMagnitude;
        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
            _currentPoint.MoveNext();
    }

  
}
