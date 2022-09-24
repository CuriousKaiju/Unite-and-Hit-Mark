using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolObject prefabBullet;
    [SerializeField] private Transform container; //Where we created a bullet
    [SerializeField] private int minCopacity;
    [SerializeField] private int maxCopacity;
    [SerializeField] private bool isExpand;

    private List<PoolObject> pool;

   
    public void LevelCreatePool()
    {
        CreatePool();
    }
    private void OnValidate()
    {
        if (isExpand)
        {
            maxCopacity = int.MaxValue;
        }
    }
    private void CreatePool()
    {
        pool = new List<PoolObject>(minCopacity);

        for (int i = 0; i < minCopacity; i++)
        {
            CreateElement();
        }
    }
    private PoolObject CreateElement(bool isActioveByDefault = false)
    {
        var createdObject = Instantiate(prefabBullet, container);
        createdObject.gameObject.SetActive(isActioveByDefault);

        pool.Add(createdObject);

        return createdObject;
    }

    public bool TryGetElement(out PoolObject element)
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.gameObject.SetActive(true);
                

                return true;
            }
        }

        element = null;
        return false;
    }

    public PoolObject GetFreeElement()
    {
        if (TryGetElement(out var element))
        {
            return element;
        }

        if (isExpand)
        {
            return CreateElement(true);
        }

        if (pool.Count < maxCopacity)
        {
            return CreateElement(true);
        }

        throw new System.Exception("Pool is over");
    }

    public PoolObject GetFreeElement(Vector3 pos)
    {
        var element = GetFreeElement();
        element.transform.position = pos;
        return element;
    }

    public PoolObject GetFreeElement(Vector3 pos, Quaternion rot)
    {
        var element = GetFreeElement(pos);
        element.transform.rotation = rot;
        return element;
    }

}
