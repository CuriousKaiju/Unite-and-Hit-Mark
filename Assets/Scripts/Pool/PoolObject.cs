using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private GameObject trailRenderer;
    [SerializeField] private bool _isTrailExist;
    public void ReturnToPool()
    {
        if (_isTrailExist)
        {
            trailRenderer.SetActive(false);
        }

        gameObject.SetActive(false);
    }
    public void TurnOnTrail()
    {
        if(_isTrailExist)
        {
            trailRenderer.SetActive(false);
        }
    }
}
