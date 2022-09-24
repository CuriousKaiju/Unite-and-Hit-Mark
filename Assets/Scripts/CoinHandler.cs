using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHandler : MonoBehaviour
{
    [SerializeField] private Pool _pool;
    [SerializeField] private Transform _playerPosition;

    private void Awake()
    {
        GameEvents.NewCoind += SpawnNewCoin;
    }
    private void OnDestroy()
    {
        GameEvents.NewCoind -= SpawnNewCoin;
    }
    private void Start()
    {
        _pool.LevelCreatePool();
    }
    public void SpawnNewCoin(Vector3 boxPos)
    {
        var newBullet = _pool.GetFreeElement();
        newBullet.gameObject.transform.position = boxPos;
        newBullet.gameObject.SetActive(true);
        newBullet.GetComponent<Coin>().SetFinishPoint(_playerPosition);
    }
}
