using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public abstract class Box : MonoBehaviour
{
    [Header("BOX COMPONENTS")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private GameObject _canvasOfHealth;
    [SerializeField] private Transform _visualTransform;
    [SerializeField] private GameObject _destroyParticles;

    [Header("VARIABLES")]
    [SerializeField] private int _startHealth;
    [SerializeField] private Vector3 _minCanvasPos;
    [SerializeField] private Vector3 _maxCanvasPos;
    [SerializeField] private Vector3 _maxScale;
    [SerializeField] private float _scaleDuration;
    [SerializeField] private int _rewardForDie;
    private int _currentHealth;

    [Header("ARRAY OF ROTATION POINTS")]
    [SerializeField] private Vector3[] _rotationsPoints;

    [Header("BLAND SHAPE")]
    [SerializeField] private float _maxBlandShape;

    [Header("ARRAY OF COLLORS")]
    [SerializeField] private Color[] _arrayOfCoolors;
    [SerializeField] private Color _currentCollor;


    [Header("BOSS")]
    [SerializeField] private float _freezeDeley;
    [SerializeField] private bool _isItBoss;

    private void Awake()
    {
        
    }
    public virtual void Start()
    {
        _healthText.text = _startHealth.ToString();
        _currentHealth = _startHealth;
        SetStartBlandShapeStatus();
        
    }

    public virtual void Update()
    {

    }

    public virtual void GetDamage(int damage)
    {
        //s_visualTransform.DOScale(_maxScale, _scaleDuration).SetLoops(-1, LoopType.Yoyo);

        _visualTransform.DOComplete();
        _visualTransform.DOPunchScale(new Vector3(1, 0, 1) * 0.1f, _scaleDuration, 1, 0);

        SetRandomRotationPos();
        _currentHealth -= damage;
        _healthText.text = _currentHealth.ToString();
        ChangeBoxLevel((float)_currentHealth / (float)_startHealth);

        if (_currentHealth <= 0)
        {
            GameEvents.OnRemoveBoxFromList(gameObject);
            GameEvents.OnNewCoin(transform.position);
            _destroyParticles.SetActive(true);
            _skinnedMeshRenderer.enabled = false;
            _boxCollider.enabled = false;
            _canvasOfHealth.SetActive(false);
            GameEvents.OnEnemyDie();
            GameEvents.OnAddCoins(_rewardForDie);


            if(_isItBoss)
            {
                StartCoroutine(Freezer());
            }
        }
    }
    public virtual IEnumerator Freezer()
    {
        var original = Time.timeScale;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(_freezeDeley);
        Time.timeScale = original;
    }
    public virtual void SetRandomRotationPos()
    {
        transform.DORotate(_rotationsPoints[Random.Range(0, _rotationsPoints.Length - 1)], 0.5f);
        _skinnedMeshRenderer.material.DOColor(Color.white, 0.1f).OnComplete(() =>
            _skinnedMeshRenderer.material.DOColor(_currentCollor, 0.1f));

        //transform.rotation = _rotationsPoints[Random.Range(0, _rotationsPoints.Length - 1)];
    }

    public virtual void ChangeBoxLevel(float expectedLevel)
    {
        SetCurrentCollor();
        _canvasOfHealth.transform.localPosition = Vector3.Lerp(_minCanvasPos, _maxCanvasPos, expectedLevel);
        _skinnedMeshRenderer.SetBlendShapeWeight(0, (float)_maxBlandShape * expectedLevel);
    }

    public virtual void SetStartBlandShapeStatus()
    {
        SetCurrentCollor();
        _maxCanvasPos = Vector3.Lerp(_minCanvasPos, _maxCanvasPos, (float)_maxBlandShape / 100);
        _canvasOfHealth.transform.localPosition = Vector3.Lerp(_minCanvasPos, _maxCanvasPos, 1);
        _skinnedMeshRenderer.SetBlendShapeWeight(0, _maxBlandShape);
    }

    public virtual void SetCurrentCollor()
    {
        string stringNumber = _currentHealth.ToString();
        char lastNumber = stringNumber[stringNumber.Length - 1];
        double idOfColor = char.GetNumericValue(lastNumber);
        _currentCollor = _arrayOfCoolors[(int)idOfColor];
        _skinnedMeshRenderer.material.color = _currentCollor;
    }

    public virtual int GetCurrentHp()
    {
        return _currentHealth;
    }

}
