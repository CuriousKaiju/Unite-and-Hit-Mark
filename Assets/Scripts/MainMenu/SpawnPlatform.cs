using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnPlatform : MonoBehaviour
{
    [Header("PLATFORM'S COMPONENTS")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private MeshRenderer _colorOfPlatform;
    [SerializeField] private MainMenuController _mainMenuController;

    [Header("MATERIALS")]
    [SerializeField] private Material _standartMaterial;
    [SerializeField] private Material _glowMaterial;

    [Header("PLATFORM'S VARIABLES")]
    public bool _isItBusy;
    public bool _isItBought;
    public bool _isItMenu;
    [SerializeField] private int _boughtCost;
    [SerializeField] private GameObject _canvasOfCost;
    [SerializeField] private TextMeshProUGUI _costText;

    [Header("PLATFORM STATUS")]
    [SerializeField] private Figure _currentFigure;
    [SerializeField] private GameObject _buyParticles;
    //[SerializeField] private GameObject

    
    public void SetFalseOfCanvas()
    {
        _canvasOfCost.SetActive(false);
    }


    public void SetBasePlatformStatus()
    {
        _isItBusy = false;
        _isItBought = true;
    }

    public void SeTboughtStatus()
    {
        if(_isItMenu)
        {
            _canvasOfCost.SetActive(true);
            _costText.text = _boughtCost.ToString();
            RemovePlatformFromAvaliblePlatforms();
            _isItBusy = true;
            _isItBought = false;
        }
    }
    public Transform GetSpawnPointTransform()
    {
        return _spawnPoint;
    }
    public void SetCurrentFigure(Figure figure)
    {
        RemovePlatformFromAvaliblePlatforms();
        _currentFigure = figure;
    }
    public Figure GetCurrentFigure()
    {
        if (_isItBought)
        {
            return _currentFigure;
        }
        else
        {
            TryBuyPlatform();
            return null;
        }
    }
    public void TryBuyPlatform()
    {
        if(_mainMenuController.GetCoinValue() >= _boughtCost)
        {
            _isItBought = true;
            _isItBusy = false;
            _canvasOfCost.SetActive(false);
            AddPlatformToAvaliblePlatforms();
            _mainMenuController.ChangeCoinValue(_boughtCost);

            _mainMenuController.SetButtonStatus();
            _buyParticles.SetActive(true);

        }
    }
    public void AddPlatformToAvaliblePlatforms()
    {
        _isItBusy = false;
        _mainMenuController.AddFreePlatform(this);
    }
    public void RemovePlatformFromAvaliblePlatforms()
    {
        _isItBought = true;
        _isItBusy = true;
        _mainMenuController.RemoveFreePlatform(this);
    }
    public void SetGlowStatus()
    {
        _colorOfPlatform.material = _glowMaterial;
    }
    public void SetBaseStatus()
    {
        _colorOfPlatform.material = _standartMaterial;
    } 
}
