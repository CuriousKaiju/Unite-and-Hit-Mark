using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT VARIABLES")]
    [SerializeField] private Transform _touchPoint;
    [SerializeField] private RoadMover _roadMover;
    [SerializeField] private Player _player;
    private bool _isFingerOnTheScreen;
    private bool _isMovementPhaseNow;

    void Update()
    {
        TouchHandler();
    }
    private void TouchHandler()
    {

        if (Input.GetMouseButtonDown(0))
        {
            InitiateMovement();
            _roadMover._roadStarMove = true;
            HitRayAndMoveTouchPoint();
            _isFingerOnTheScreen = true;
        }
        else if (_isFingerOnTheScreen)
        {
            HitRayAndMoveTouchPoint();

            if (Input.GetMouseButtonUp(0))
            {
                _touchPoint.transform.position = new Vector3(_player.transform.position.x, _touchPoint.position.y, _touchPoint.position.z);
                _isFingerOnTheScreen = false;
            }
        }
    }
    private void HitRayAndMoveTouchPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            _touchPoint.position = hit.point;
        }
    }
    private void InitiateMovement()
    {
        if(!_isMovementPhaseNow)
        {
            _isMovementPhaseNow = true;
            _player.StartRunAnimation();
        }
    }
}
