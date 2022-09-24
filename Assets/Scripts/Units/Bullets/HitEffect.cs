using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private Transform _bullet;
    [SerializeField] private Vector3 _hitPosition;

    private void OnDisable()
    {
        transform.localPosition = _hitPosition;
        transform.SetParent(_bullet);
    }
}
