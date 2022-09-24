using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class JsonManager : MonoBehaviour
{
    [SerializeField] private ArrayOfSavePlatforms _platformsList;
    [SerializeField] private ArrayOfSavePlatforms _startPlatformList;
    [SerializeField] private MainMenuController _mainMenuController;

    [SerializeField] private bool _isItMenu;
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        ReadJson();
    }
    private void OnDestroy()
    {
        if(_isItMenu)
        {
            _mainMenuController.ChangeJson();
            WriteJson();
        }
    }
    public void WriteJson()
    {
        string platformsLostToJson = JsonUtility.ToJson(_platformsList , true);

        File.WriteAllText(Application.persistentDataPath + "/jsonSaves.txt", platformsLostToJson);
    }
    public void ReadJson()
    {
        if(File.Exists(Application.persistentDataPath + "/jsonSaves.txt"))
        {
            _platformsList = JsonUtility.FromJson<ArrayOfSavePlatforms>(File.ReadAllText(Application.persistentDataPath + "/jsonSaves.txt"));
            CallSetFiguresFromJson();
        }
        else
        {
            string platformsLostToJson = JsonUtility.ToJson(_startPlatformList, true);
            File.WriteAllText(Application.persistentDataPath + "/jsonSaves.txt", platformsLostToJson);
            _platformsList = _startPlatformList;
            CallSetFiguresFromJson();
        }
    }

    private void CallSetFiguresFromJson()
    {
        _mainMenuController.SetFiguresFromJson(_platformsList._platformsArray);
    }

    public void RewritePlatformsList(SpawnPlatform[] allPlatforms)
    {
        for (int i = 0; i < allPlatforms.Length; i++)
        {
            if (allPlatforms[i]._isItBusy && allPlatforms[i]._isItBought)
            {
                _platformsList._platformsArray[i]._thisPlatformIsBusy = true;
                _platformsList._platformsArray[i]._typeOfUnit = allPlatforms[i].GetCurrentFigure().GetFigureSaveParams()._figureType;
                _platformsList._platformsArray[i]._lvlOfUnit = allPlatforms[i].GetCurrentFigure().GetFigureSaveParams()._figureLevel;

                _platformsList._platformsArray[i]._thisPlatformBogught = true;

            }
            else if (allPlatforms[i]._isItBusy && !allPlatforms[i]._isItBought)
            {
                _platformsList._platformsArray[i]._thisPlatformIsBusy = true;
                _platformsList._platformsArray[i]._typeOfUnit = null;
                _platformsList._platformsArray[i]._lvlOfUnit = 0;
                _platformsList._platformsArray[i]._thisPlatformBogught = false;

            }
            else if (!allPlatforms[i]._isItBusy && allPlatforms[i]._isItBought)
            {
                _platformsList._platformsArray[i]._thisPlatformIsBusy = false;
                _platformsList._platformsArray[i]._typeOfUnit = null;
                _platformsList._platformsArray[i]._lvlOfUnit = 0;
                _platformsList._platformsArray[i]._thisPlatformBogught = true;
                
            }
        }
    }
    
}
[System.Serializable]
public class SavePlatform
{
    public bool _thisPlatformIsBusy;
    public bool _thisPlatformBogught;
    public string _typeOfUnit;
    public int _lvlOfUnit;
}
[System.Serializable]
public class ArrayOfSavePlatforms
{
    public SavePlatform[] _platformsArray;
}
