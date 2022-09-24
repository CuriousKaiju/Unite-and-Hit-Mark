using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelStatusController : MonoBehaviour
{
    [SerializeField] private RoadMover _roadMover;
    [SerializeField] private SpawnPlatform[] _arrayOfPlatforms;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private int _unitsCount;
    [SerializeField] private int _enemiesCount;

    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;

    [SerializeField] private Player _player;

    [SerializeField] private string _currentLevel;
    [SerializeField] private string _nextLevel;

    [SerializeField] private int _coinValueForThisLevel;
    [SerializeField] private TextMeshProUGUI _coinResultWin;
    [SerializeField] private TextMeshProUGUI _coinResultLose;

    [SerializeField] private int _currentLevelID;

    [SerializeField] private BlackScreenFunction _blackscreen;
    [SerializeField] private GameObject _blackIn;

    private bool _alreadyWin;
    private bool _alreadyLose;

    private void Start()
    {
        TinySauce.OnGameStarted(_currentLevelID.ToString());
    }

    private void Awake()
    {
        GameEvents.EnemyDie += MinusEnemy;
        GameEvents.UnitDie += MinusUnit;
        GameEvents.AddCoins += IncreaseCoinValueForThisLevel;
    }
    private void OnDestroy()
    {
        GameEvents.EnemyDie -= MinusEnemy;
        GameEvents.UnitDie -= MinusUnit;
        GameEvents.AddCoins -= IncreaseCoinValueForThisLevel;
    }

    public void IncreaseCoinValueForThisLevel(int coins)
    {
        _coinValueForThisLevel += coins;
    }
    public void SetCountOfEnemiesAndUnits()
    {
        foreach(SpawnPlatform spawnPlatform in _arrayOfPlatforms)
        {
            if (spawnPlatform._isItBusy && spawnPlatform._isItBought)
            {
                _unitsCount += 1;
            }
        }

        _enemiesCount = _enemies.Length;
    }

    public void MinusUnit()
    {
        _player.SetLimitPositions();
        _unitsCount -= 1;
        if(_unitsCount <= 0 && !_alreadyLose)
        {
            _alreadyLose = true;
            _loseMenu.SetActive(true);
            _roadMover.SetRoadSpeed(0);
            var coins = PlayerPrefs.GetInt("Coins");
            PlayerPrefs.SetInt("Coins", coins + _coinValueForThisLevel);
            _coinResultWin.text = _coinValueForThisLevel.ToString();
            _coinResultLose.text = _coinValueForThisLevel.ToString();
            GameEvents.OnStopRound();

            TinySauce.OnGameFinished(false, 0);

        }
    }
    public void MinusEnemy()
    {
        _enemiesCount -= 1;
        if (_enemiesCount <= 0 && !_alreadyWin)
        {
            PlayerPrefs.SetString("NextLevel", "Level " + (_currentLevelID + 1));
            _alreadyWin = true;
            _winMenu.SetActive(true);
            _roadMover.SetRoadSpeed(0);
            var coins = PlayerPrefs.GetInt("Coins");
            PlayerPrefs.SetInt("Coins", coins + _coinValueForThisLevel);
            _coinResultWin.text = _coinValueForThisLevel.ToString();
            _coinResultLose.text = _coinValueForThisLevel.ToString();
            GameEvents.OnStopRound();

            TinySauce.OnGameFinished(true, 0);
        }
    }
    public void OpenWinMenu()
    {
        PlayerPrefs.SetString("NextLevel", "Level " + (_currentLevelID + 1));
        _alreadyWin = true;
        _winMenu.SetActive(true);
        _roadMover.SetRoadSpeed(0);
        var coins = PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("Coins", coins + _coinValueForThisLevel);
        _coinResultWin.text = _coinValueForThisLevel.ToString();
        _coinResultLose.text = _coinValueForThisLevel.ToString();
    }

    public void RestartLevel()
    {
        _blackscreen.SetLoadedScene("Level " + _currentLevelID);
        _blackIn.SetActive(true);
    }
    public void NextLevel()
    {
        _blackscreen.SetLoadedScene(_nextLevel);
        _blackIn.SetActive(true);
    }
    public void MainMenu()
    {
        _blackscreen.SetLoadedScene("Main");
        _blackIn.SetActive(true);
    }


}
