using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _touchPoint;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private SpawnPlatform[] _spawnPlatforms;

    [SerializeField] private float _maxLeftPosition_one;
    [SerializeField] private float _maxRightPosition_one;

    [SerializeField] private float _maxLeftPosition_two;
    [SerializeField] private float _maxRightPosition_two;

    [SerializeField] private float _maxLeftPosition_three;
    [SerializeField] private float _maxRightPosition_three;



    [SerializeField] private float _totalMaxLeftPosition;
    [SerializeField] private float _totalMaxRightPosition;


    [SerializeField] private SpawnPlatform[] _leftLeftArray;
    [SerializeField] private SpawnPlatform[] _leftArray;
    [SerializeField] private SpawnPlatform[] _middleArray;
    [SerializeField] private SpawnPlatform[] _rightArray;
    [SerializeField] private SpawnPlatform[] _rightRightArray;


    [SerializeField] private float[] _leftSidePoints;
    [SerializeField] private float[] _rightSidePoints;

    private bool _canMove = true;

    private void Awake()
    {
        GameEvents.StopRound += CanMoveToFalse;    
    }

    private void OnDestroy()
    {
        GameEvents.StopRound -= CanMoveToFalse;
    }

    private void Start()
    {
        foreach(SpawnPlatform pl in _spawnPlatforms)
        {
            pl.SetFalseOfCanvas();
        }
    }

    void Update()
    {
        if(_canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Clamp(_touchPoint.position.x, _totalMaxLeftPosition, _totalMaxRightPosition), transform.position.y, transform.position.z), _movementSpeed * Time.deltaTime);
        }    
    }
    private void CanMoveToFalse()
    {
        _canMove = false;
    }
    public void SetLimitPositions()
    {
        _totalMaxLeftPosition = _leftSidePoints[4];
        _totalMaxRightPosition = _rightSidePoints[4];

        //проблема тут
        /*
        foreach (SpawnPlatform sp in _leftArray)
        {
            if (sp._isItBusy)
            {
                _totalMaxLeftPosition = _maxLeftPosition_two;
            }
        }

        foreach (SpawnPlatform sp in _leftLeftArray)
        {
            if (sp._isItBusy)
            {
                _totalMaxLeftPosition = _maxLeftPosition_three;
            }
        }

        foreach (SpawnPlatform sp in _rightArray)
        {
            if (sp._isItBusy)
            {
                _totalMaxRightPosition = _maxRightPosition_two;
            }
        }

        foreach (SpawnPlatform sp in _rightRightArray)
        {
            if (sp._isItBusy)
            {
                _totalMaxRightPosition = _maxRightPosition_three;
            }
        }
        */


        for (int i = 0; i < _leftSidePoints.Length; i++)
        {
            if (_leftLeftArray[i]._isItBusy && _leftLeftArray[i]._isItBought)
            {
                _totalMaxRightPosition = _rightSidePoints[0];
            }
        }

        for (int i = 0; i < _leftSidePoints.Length; i++)
        {
            if (_leftArray[i]._isItBusy && _leftArray[i]._isItBought)
            {
                _totalMaxRightPosition = _rightSidePoints[1];
            }
        }

        for (int i = 0; i < _leftSidePoints.Length; i++)
        {
            if (_middleArray[i]._isItBusy && _middleArray[i]._isItBought)
            {
                _totalMaxRightPosition = _rightSidePoints[2];
            }
        }

        for (int i = 0; i < _leftSidePoints.Length; i++)
        {
            if (_rightArray[i]._isItBusy && _rightArray[i]._isItBought) 
            {
                _totalMaxRightPosition = _rightSidePoints[3];
            }
        }

        for (int i = 0; i < _leftSidePoints.Length; i++)
        {
            if (_rightRightArray[i]._isItBusy && _rightRightArray[i]._isItBought)
            {
                _totalMaxRightPosition = _rightSidePoints[4];
            }
        }





        for (int i = 0; i < _rightSidePoints.Length; i++)
        {
            if (_rightRightArray[i]._isItBusy && _rightRightArray[i]._isItBought)
            {
                _totalMaxLeftPosition = _leftSidePoints[0];
            }
        }

        for (int i = 0; i < _rightSidePoints.Length; i++)
        {
            if (_rightArray[i]._isItBusy && _rightArray[i]._isItBought)
            {
                _totalMaxLeftPosition = _leftSidePoints[1];
            }
        }

        for (int i = 0; i < _rightSidePoints.Length; i++)
        {
            if (_middleArray[i]._isItBusy && _middleArray[i]._isItBought)
            {
                _totalMaxLeftPosition = _leftSidePoints[2];
            }
        }

        for (int i = 0; i < _rightSidePoints.Length; i++)
        {
            if (_leftArray[i]._isItBusy && _leftArray[i]._isItBought)
            {
                _totalMaxLeftPosition = _leftSidePoints[3];
            }
        }

        for (int i = 0; i < _rightSidePoints.Length; i++)
        {
            if (_leftLeftArray[i]._isItBusy && _leftLeftArray[i]._isItBought)
            {
                _totalMaxLeftPosition = _leftSidePoints[4];
            }
        }

    }


    public void StartRunAnimation()
    {
        SetLimitPositions();
        foreach (SpawnPlatform platform in _spawnPlatforms)
        {
            if(platform._isItBusy && platform._isItBought)
            {
                platform.GetCurrentFigure().StartMoveAnimation();
                platform.GetCurrentFigure().StartAttackPhase();
            }
        }
    }
}
