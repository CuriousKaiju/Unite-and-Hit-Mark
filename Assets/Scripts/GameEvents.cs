using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents
{

    public static event Action UnitDie;
    public static event Action EnemyDie;
    public static event Action<int> AddCoins;
    public static event Action<Vector3> NewCoind;
    public static event Action StopRound;
    public static event Action<GameObject> RemoveBoxFromList;

    public static void OnRemoveBoxFromList(GameObject box)
    {
        RemoveBoxFromList?.Invoke(box);
    }
    public static void OnUnityDie()
    {
        UnitDie?.Invoke();
    }

    public static void OnEnemyDie()
    {
        EnemyDie?.Invoke();
    }
    public static void OnAddCoins(int coinsAdd)
    {
        AddCoins?.Invoke(coinsAdd);
    }
    public static void OnNewCoin(Vector3 boxPos)
    {
        NewCoind?.Invoke(boxPos);
    }
    public static void OnStopRound()
    {
        StopRound?.Invoke();
    }

}
