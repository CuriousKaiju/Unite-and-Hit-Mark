using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMover : MonoBehaviour
{
    public bool _roadStarMove;

    [SerializeField] private Vector3 _directionOfMovement;
    [SerializeField] private float _roadSpeed;

    void Update()
    {
        if(_roadStarMove)
        {
            MoveRoad();
        }
    }

    private void MoveRoad()
    {
        transform.Translate(_directionOfMovement * _roadSpeed * Time.deltaTime);
    }

    public void SetRoadSpeed(float speed)
    {
        _roadSpeed = speed;
    }
}
