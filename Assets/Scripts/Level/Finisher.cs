using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finisher : MonoBehaviour
{
    [SerializeField] private LevelStatusController _levelStatusController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Platform"))
        {
            GameEvents.OnStopRound();
            _levelStatusController.OpenWinMenu();
            TinySauce.OnGameFinished(true, 0);
        }
    }
}
