using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField] private float _coinSpeed;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private PoolObject _poolObject;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _coinSpeed);

        if (Vector3.Distance(transform.position, _targetPosition.position) < 0.001f)
        {
            _poolObject.ReturnToPool();
        }
    }
    public void SetFinishPoint(Transform player)
    {
        _targetPosition = player;
    }
}
