using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Figure : MonoBehaviour
{
    [Header("FIGURE'S COMPONENTS")]
    [SerializeField] private Animator _figureAnimator;
    [SerializeField] private Animator _visualOfFigureAnimator;
    [SerializeField] private SpawnPlatform _currentPlatform;
    [SerializeField] private SkinnedMeshRenderer _scinnedMeshRenderer;
    [SerializeField] private Transform _visualTransform;
    [SerializeField] private FigureSaveParams _figureSaveParams;
    public Pool _pool;

    [Header("ATTACK VARIABLES")]
    public float _attackDelay;
    [SerializeField] private int _health;
    public GameObject _spawnParticlesAnimations;
    [SerializeField] private Collider _figureCollider;

    [Header("STANDART MATERIAL")]
    [SerializeField] private Material _standartMaterial;
    [SerializeField] private Vector3 _standartScale;

    [Header("SELECTED MATERIAL")]
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private Vector3 _selectedScale;

    [Header("UPGRADE VARIABLES")]
    [SerializeField] private GameObject _nextLevelUnit;
    [SerializeField] private GameObject _spawnParticles;

    [SerializeField] private GameObject _dieParticles;

    private bool _unitAlreadyDie;
    public virtual void Awake()
    {
        GameEvents.StopRound += StopAttack;
    }
    private void OnDisable()
    {
        GameEvents.StopRound -= StopAttack;
    }
    public void StopAttack()
    {
        _visualOfFigureAnimator.SetTrigger("ToIdle");
        StopAllCoroutines();
    }
    public virtual void SetPlatformForUnit(SpawnPlatform platform)
    {
        _figureAnimator.SetTrigger("Spawn");
        transform.SetParent(platform.GetSpawnPointTransform());
        transform.localPosition = new Vector3(0, 0, 0);
        _currentPlatform = platform;
        _currentPlatform._isItBusy = true;
        platform.SetCurrentFigure(this);
    }
    public virtual void SetPlatformForUnitAfterSpawn(SpawnPlatform platform)
    {
        _figureAnimator.SetTrigger("Bounce");
        transform.SetParent(platform.GetSpawnPointTransform());
        transform.localPosition = new Vector3(0, 0, 0);
        _currentPlatform = platform;
        _currentPlatform._isItBusy = true;
        platform.SetCurrentFigure(this);
        
    }
    public virtual void SetNewPlatformForUnit(SpawnPlatform platform)
    {
        _figureAnimator.SetTrigger("Bounce");
        _currentPlatform.AddPlatformToAvaliblePlatforms();
        _currentPlatform = platform;
        _currentPlatform.RemovePlatformFromAvaliblePlatforms();
        MoveToCurrentPlatform();
        _currentPlatform.SetCurrentFigure(this);
    }


    public virtual void SetSelectedState(Transform touchPoint)
    {
        transform.SetParent(touchPoint);
        transform.localPosition = new Vector3(0, 0, 0);
        SetMaterialAndSize(_selectedMaterial, _selectedScale);
    }
    public virtual void SetMaterialAndSize(Material material, Vector3 scale)
    {
        _scinnedMeshRenderer.material = material;
        _visualTransform.localScale = scale;
    }
    public virtual void MoveToCurrentPlatform()
    {
        _figureAnimator.SetTrigger("Bounce");
        SetMaterialAndSize(_standartMaterial, _standartScale);
        transform.SetParent(_currentPlatform.GetSpawnPointTransform());
        transform.localPosition = new Vector3(0, 0, 0);
    }
    public virtual void StartMoveAnimation()
    {
        _visualOfFigureAnimator.SetTrigger("StartWalk");
    }
    public virtual FigureSaveParams GetFigureSaveParams()
    {
        return _figureSaveParams;
    }
    public virtual SpawnPlatform GetSpawnPlatform()
    {
        return _currentPlatform;
    }
    public virtual void ClearDestroy()
    {
        _currentPlatform.AddPlatformToAvaliblePlatforms();
        Destroy(gameObject);
    }
    public virtual void ClearDestroyWithoutPlatformOverdride()
    {
        Destroy(gameObject);
    }
    public virtual GameObject GetNextLevelFigure()
    {
        return _nextLevelUnit;
    }
    public virtual void ActiveSpawnParticles()
    {
        _spawnParticles.SetActive(true);
    }

    public virtual void JustGlow()
    {
        SetMaterialAndSize(_selectedMaterial, _selectedScale);
    }
    public virtual void JustBaseState()
    {
        SetMaterialAndSize(_standartMaterial, _standartScale);
    }

    public virtual void StartAttackPhase()
    {
        StartCoroutine(StartAttackAction());
    }

    public virtual IEnumerator StartAttackAction()
    {
        while (true)
        {
            _spawnParticlesAnimations.SetActive(true);
            var newBullet = _pool.GetFreeElement();
            newBullet.gameObject.transform.position = transform.position + new Vector3(0, 0.6f, 0);
            newBullet.gameObject.SetActive(true);
            newBullet.TurnOnTrail();
            yield return new WaitForSeconds(_attackDelay);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var newBox = other.gameObject.GetComponent<Box>();
            var totalPlayerHealth = _health;
            var totalBoxHealth = newBox.GetCurrentHp();

            newBox.GetDamage(totalPlayerHealth);
            GetDamage(totalBoxHealth);
        }
    }
    public virtual void GetDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Instantiate(_dieParticles, transform.position, Quaternion.Euler(0, 0, 0));
            StartDieAction();
        }
    }
    public virtual void StartDieAction()
    {
        if (!_unitAlreadyDie)
        {
            _unitAlreadyDie = true;
            StopCoroutine(StartAttackAction());
            _currentPlatform._isItBusy = false;
            GameEvents.OnUnityDie();
            _visualTransform.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
[System.Serializable]
public class FigureSaveParams
{
    public string _figureType;
    public int _figureLevel;
}

