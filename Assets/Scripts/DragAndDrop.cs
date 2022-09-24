using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [Header("DRAG AND DROP COMPONENTS")]
    [SerializeField] private Transform _touchPoint;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private LayerMask _touchPointLayer;
    [SerializeField] private MainMenuController _mainMenuController;
    private bool _isFingerOnTheScreen;

    [Header("FIRST TOUCH VARIABLES")]
    [SerializeField] private Figure _selectedFigure;

    [SerializeField] private GameObject _finger;
    

    void Start()
    {
        
    }
    void Update()
    {
        TouchHandler();
    }
    private void TouchHandler()
    {
        HitRayAndMoveTouchPoint();

        if (Input.GetMouseButtonDown(0))
        {
            _isFingerOnTheScreen = true;
            TryToGetUnitFrofPlatform();

            if (_selectedFigure)
            {
                _mainMenuController.CheckPlatformsOnConnection(_selectedFigure.GetFigureSaveParams(), _selectedFigure.GetSpawnPlatform());

                if (PlayerPrefs.GetString("NextLevel") == "Level 1")
                {
                    _finger.SetActive(false);
                }
            }
        }
        else if (_isFingerOnTheScreen && _selectedFigure)
        {
            HitRayAndMoveTouchPoint();

            if (Input.GetMouseButtonUp(0) && _selectedFigure)
            {
                TryToSetUnitOnThePlatform();
                _mainMenuController.ClearAllPlatforms();
                _isFingerOnTheScreen = false;
                _selectedFigure = null;
            }
        }
    }
    private void HitRayAndMoveTouchPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _touchPointLayer))
        {
            _touchPoint.position = hit.point;
        }
    }
    private void TryToGetUnitFrofPlatform()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _platformLayer))
        {
            var currentPlatform = hit.collider.gameObject.GetComponent<SpawnPlatform>();
            
            if (currentPlatform._isItBusy && currentPlatform._isItBought)
            {
                _selectedFigure = currentPlatform.GetCurrentFigure();

                if(_selectedFigure)
                {
                    _selectedFigure.SetSelectedState(_touchPoint);
                }
            }

            if(!currentPlatform._isItBought)
            {
                currentPlatform.TryBuyPlatform(); //сюда передать монетки
            }
        }
    }
    private void TryToSetUnitOnThePlatform()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _platformLayer))
        {
            var currentPlatform = hit.collider.gameObject.GetComponent<SpawnPlatform>();

            if (!currentPlatform._isItBusy)
            {
                _selectedFigure.SetNewPlatformForUnit(currentPlatform);
            }
            else if (currentPlatform._isItBought && currentPlatform._isItBusy && currentPlatform != _selectedFigure.GetSpawnPlatform() )
            {
                var paramsOfTheFigureOnThePlatform = currentPlatform.GetCurrentFigure().GetFigureSaveParams();
                var paramsOfTheSelectedFigure = _selectedFigure.GetFigureSaveParams();

                if (paramsOfTheFigureOnThePlatform._figureLevel == paramsOfTheSelectedFigure._figureLevel && paramsOfTheFigureOnThePlatform._figureType == paramsOfTheSelectedFigure._figureType && paramsOfTheSelectedFigure._figureLevel != 3)
                {
                    _selectedFigure.ClearDestroy();
                    var newUnit = Instantiate(currentPlatform.GetCurrentFigure().GetNextLevelFigure(), transform.position, Quaternion.Euler(0, 180, 0)).GetComponent<Figure>();
                    currentPlatform.GetCurrentFigure().ClearDestroyWithoutPlatformOverdride();
                    newUnit.SetPlatformForUnitAfterSpawn(currentPlatform);
                    newUnit.ActiveSpawnParticles();
                }
                else
                {
                    _selectedFigure.MoveToCurrentPlatform();
                }
            }
            else
            {
                _selectedFigure.MoveToCurrentPlatform();
            }
        }
        else
        {
            _selectedFigure.MoveToCurrentPlatform();
        }
    }
}
