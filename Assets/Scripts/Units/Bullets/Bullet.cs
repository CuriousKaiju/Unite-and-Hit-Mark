using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private float _lifeTime;
    [SerializeField] private PoolObject _poolObject;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private Vector3 _hitOfset;

    [SerializeField] private string _bulletType;
    [SerializeField] float _damageRange;
    [SerializeField] LayerMask _units;

    private float _elapsedTime;
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _bulletSpeed);
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= _lifeTime)
        {
            _elapsedTime = 0;
            _poolObject.ReturnToPool();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            switch (_bulletType)
            {
                case "Base":
                    other.gameObject.GetComponent<Box>().GetDamage(_bulletDamage);
                    _elapsedTime = 0;
                    Instantiate(_hitEffect, transform.localPosition + _hitOfset, Quaternion.Euler(0, 180, 0));
                    _poolObject.ReturnToPool();
                    break;

                case "Rocket":

                    Collider[] enemies = Physics.OverlapSphere(transform.position, _damageRange, _units);
                    foreach (Collider col in enemies)
                    {
                        col.gameObject.GetComponent<Box>().GetDamage(_bulletDamage);
                    }
                    _elapsedTime = 0;
                    Instantiate(_hitEffect, transform.localPosition + _hitOfset, Quaternion.Euler(0, 180, 0));
                    _poolObject.ReturnToPool();
                    break;

                case "Laser":
                    other.gameObject.GetComponent<Box>().GetDamage(_bulletDamage);
                    _elapsedTime = 0;
                    Instantiate(_hitEffect, transform.localPosition + _hitOfset, Quaternion.Euler(0, 180, 0));
                    _poolObject.ReturnToPool();
                    break;

            }
        }
    }

}
