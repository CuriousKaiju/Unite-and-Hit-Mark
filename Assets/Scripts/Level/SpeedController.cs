using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _boxes;
    [SerializeField] private RoadMover _roadMover;
    [SerializeField] private float _slow;
    [SerializeField] private float _fast;

    private void Start()
    {
        _roadMover.SetRoadSpeed(_fast);
    }
    private void Awake()
    {
        GameEvents.RemoveBoxFromList += RemoveBox;
    }
    private void OnDestroy()
    {
        GameEvents.RemoveBoxFromList -= RemoveBox;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _boxes.Add(other.gameObject);
            _roadMover.SetRoadSpeed(_slow);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _boxes.Remove(other.gameObject);

            if (_boxes.Count == 0)
            {
                _roadMover.SetRoadSpeed(_fast);
            }
        }
    }
    private void RemoveBox(GameObject box)
    {
        if(_boxes.Contains(box))
        {
           _boxes.Remove(box);

            if(_boxes.Count == 0)
            {
                _roadMover.SetRoadSpeed(_fast);
            }
        }
    }
}