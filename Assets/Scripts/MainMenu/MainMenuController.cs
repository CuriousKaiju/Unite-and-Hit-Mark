using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("SPAWN VARIABLES")]
    [SerializeField] private SpawnPlatform[] _allPlatformsArray;
    [SerializeField] private List<SpawnPlatform> _freePlatformsArray;
    [SerializeField] private JsonManager _jsonManager;
    [SerializeField] private GameObject[] _nextUnit;
    [SerializeField] private int[] _nextPriceForUnit;
    [SerializeField] private int _clickId;
    [SerializeField] private bool _isItlevel;

    [Header("ALL FIGURES")]
    [SerializeField] private GameObject[] _guns;
    [SerializeField] private GameObject[] _rockets;
    [SerializeField] private GameObject[] _laser;
    [SerializeField] private GameObject[] _split;

    [SerializeField] private bool _isItLevel;
    [SerializeField] private LevelStatusController _levelStatusController;

    [Header("LEVEL INFO")]
    [SerializeField] private string _nextLevelName;
    [SerializeField] private int _coinValue;
    [SerializeField] private TextMeshProUGUI _levelTextMeshPro;
    [SerializeField] private TextMeshProUGUI _currentCoinValue;
    [SerializeField] private TextMeshProUGUI _costForNextUnit;
    [SerializeField] private Button _buyUnitButton;

    [SerializeField] private GameObject _blackIn;
    [SerializeField] private BlackScreenFunction _blackFunction;

    [SerializeField] private GameObject _finger;


    public int GetCoinValue()
    {
        return _coinValue;
    }

    public void ClearAllSaves()
    { 
        PlayerPrefs.DeleteAll();
    }
    private void Start()
    {

        if (!_isItlevel)
        {
            if (PlayerPrefs.HasKey("ClickID"))
            {
                _clickId = PlayerPrefs.GetInt("ClickID");
            }
            else
            {
                PlayerPrefs.SetInt("ClickID", 0);
                _clickId = PlayerPrefs.GetInt("ClickID");
            }


            if (PlayerPrefs.HasKey("NextLevel"))
            {
                _nextLevelName = PlayerPrefs.GetString("NextLevel");
                _levelTextMeshPro.text = _nextLevelName;
            }
            else
            {
                PlayerPrefs.SetString("NextLevel", "Level 1");
                _nextLevelName = PlayerPrefs.GetString("NextLevel");
                _levelTextMeshPro.text = _nextLevelName;
            }


            if (PlayerPrefs.HasKey("Coins"))
            {
                _coinValue = PlayerPrefs.GetInt("Coins");
                _currentCoinValue.text = _coinValue.ToString();
            }
            else
            {
                PlayerPrefs.SetInt("Coins", 10);
                _coinValue = PlayerPrefs.GetInt("Coins");
                _currentCoinValue.text = _coinValue.ToString();
            }

            _costForNextUnit.text = _nextPriceForUnit[_clickId].ToString();

            SetButtonStatus();

            if(PlayerPrefs.GetString("NextLevel") == "Level 1")
            {
                _finger.SetActive(true);
            }
        }
    }
    private void OnDestroy()
    {
        
          
    }
    public void ChangeJson()
    {
        _jsonManager.RewritePlatformsList(_allPlatformsArray);
    }
    public void ChangeCoinValue(int coins)
    {
        _coinValue -= coins;
        PlayerPrefs.SetInt("Coins", _coinValue);
        _currentCoinValue.text = _coinValue.ToString();

    }
    public void SpawnNextUnit()
    {
        if (_freePlatformsArray.Count > 0)
        {
            var selectedPlatform = _freePlatformsArray[Random.Range(0, _freePlatformsArray.Count - 1)];
            _freePlatformsArray.Remove(selectedPlatform);
            selectedPlatform._isItBusy = false;

            var nextUnit = Instantiate(_nextUnit[_clickId], transform.position, Quaternion.Euler(0, 180, 0));
            nextUnit.GetComponent<Figure>().SetPlatformForUnit(selectedPlatform);

            _coinValue -= _nextPriceForUnit[_clickId];
            _clickId += 1;

            PlayerPrefs.SetInt("Coins", _coinValue);
            PlayerPrefs.SetInt("ClickID", _clickId);

            SetButtonStatus();
        }
    }

    public void SetFiguresFromJson(SavePlatform[] platformsArray)
    {
        for (int i = 0; i < platformsArray.Length; i++)
        {
            if (platformsArray[i]._thisPlatformIsBusy && platformsArray[i]._thisPlatformBogught)
            {

                GameObject figure;
                if(_isItLevel)
                {
                    figure = Instantiate(FindCorrectFigure(platformsArray[i]._typeOfUnit, platformsArray[i]._lvlOfUnit), transform.position, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    figure = Instantiate(FindCorrectFigure(platformsArray[i]._typeOfUnit, platformsArray[i]._lvlOfUnit), transform.position, Quaternion.Euler(0, 180, 0));
                }

                figure.GetComponent<Figure>().SetPlatformForUnit(_allPlatformsArray[i]);

                if(_isItlevel)
                {
                    figure.gameObject.GetComponent<Pool>().LevelCreatePool();
                }
            }
            else if(platformsArray[i]._thisPlatformIsBusy && !platformsArray[i]._thisPlatformBogught)
            {
                _allPlatformsArray[i].SeTboughtStatus();
            }
            else if(!platformsArray[i]._thisPlatformIsBusy && platformsArray[i]._thisPlatformBogught)
            {
                _allPlatformsArray[i].SetBasePlatformStatus();
            }
        }

        if(_isItLevel)
        {
            _levelStatusController.SetCountOfEnemiesAndUnits();
        }
    }


    private GameObject FindCorrectFigure(string figureType, int figureLevel)
    {
        GameObject returnedUnit = null;

        switch (figureType)
        {
            case "Gun":
                returnedUnit = _guns[figureLevel - 1];
                break;

            case "Rocket":
                returnedUnit = _rockets[figureLevel - 1];
                break;

            case "Laser":
                returnedUnit = _laser[figureLevel - 1];
                break;

            case "Split":
                returnedUnit = _split[figureLevel - 1];
                break;
        }

        return returnedUnit;
    }

    public void SetButtonStatus()
    {
        if(_coinValue < _nextPriceForUnit[_clickId])
        {
            _buyUnitButton.interactable = false;
        }
        else
        {
            _buyUnitButton.interactable = true;
        }

        _currentCoinValue.text = _coinValue.ToString();
        _costForNextUnit.text = _nextPriceForUnit[_clickId].ToString();
    }

    public void AddFreePlatform(SpawnPlatform spawnPlatform)
    {
        _freePlatformsArray.Add(spawnPlatform);
    }
    public void RemoveFreePlatform(SpawnPlatform spawnPlatform)
    {
        _freePlatformsArray.Remove(spawnPlatform);
    }
    public void CheckPlatformsOnConnection(FigureSaveParams figureSaveParams, SpawnPlatform startPlatform)
    {
        foreach (SpawnPlatform spawnPlatform in _allPlatformsArray)
        {
            if (spawnPlatform._isItBusy && startPlatform != spawnPlatform && spawnPlatform._isItBought)
            {
                var comparableFigure = spawnPlatform.GetCurrentFigure();

                if (comparableFigure.GetFigureSaveParams()._figureLevel == figureSaveParams._figureLevel && comparableFigure.GetFigureSaveParams()._figureType == figureSaveParams._figureType && comparableFigure.GetFigureSaveParams()._figureLevel != 3)
                {
                    spawnPlatform.SetGlowStatus();
                    spawnPlatform.GetCurrentFigure().JustGlow();
                }
            }
        }
    }
    public void ClearAllPlatforms()
    {
        foreach (SpawnPlatform spawnPlatform in _allPlatformsArray)
        {
            spawnPlatform.SetBaseStatus();
            if (spawnPlatform._isItBusy && spawnPlatform._isItBought)
            {
                spawnPlatform.GetCurrentFigure().JustBaseState();
            }
        }
    }

    public void LoadLevel()
    {
        _blackFunction.SetLoadedScene(_nextLevelName);
        _blackIn.SetActive(true);
    }
}
