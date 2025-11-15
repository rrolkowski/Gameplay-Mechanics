using UnityEngine;
using UnityEngine.UI;

public class PMana : MonoBehaviour
{
    private Statistics _statistics;

    [Header("Mana Settings")]
    [SerializeField] private float _currentMana = 100f;
    [SerializeField] private float _maxMana = 100f;
    [SerializeField] private float _regenRate = 5f;

    [Header("UI")]
    [SerializeField] private Image _manaBar;
    [SerializeField] private RectTransform _manaBackground;

    private void Awake()
    {
        _statistics = GetComponent<StatisticsProvider>().GetStatistics();
    }

    private void Start()
    {
        _maxMana = _statistics.baseMaxMana;
        _currentMana = _maxMana;

        if (_manaBar != null)
            _manaBar.fillAmount = _currentMana / _maxMana;

        _statistics.StatPointsChanged.AddListener(UpdateMana);
    }

    private void OnDestroy()
    {
        _statistics.StatPointsChanged.RemoveListener(UpdateMana);
    }

    private void Update()
    {
        if (_currentMana < _maxMana)
        {
            _currentMana += _regenRate * Time.deltaTime;
            _currentMana = Mathf.Clamp(_currentMana, 0, _maxMana);
        }

        if (_manaBar != null)
            _manaBar.fillAmount = _currentMana / _maxMana;
    }

    public void RefillMana(float refill)
    {
        if (refill < 0) return;
        _currentMana += refill;
        _currentMana = Mathf.Clamp(_currentMana, 0, _maxMana);
        _manaBar.fillAmount = _currentMana / _maxMana;
    }

    public bool TryConsumeMana(float cost)
    {
        if (_currentMana >= cost)
        {
            _currentMana -= cost;
            return true;
        }
        return false;
    }

    public float GetCurrentMana() => _currentMana;
    public float GetMaxMana() => _maxMana;

    private void UpdateMana()
    {
        _maxMana = 100f + (_statistics.statPoints[BaseStatType.INTELLIGENCE] * 5f);
        _manaBar.fillAmount = _currentMana / _maxMana;
        UpdateManaBar();
    }

    private void UpdateManaBar()
    {
        if (_manaBackground != null && _manaBar != null)
        {
            RectTransform bgTransform = _manaBackground;
            RectTransform barTransform = _manaBar.rectTransform;

            float baseWidth = 100f;
            float newWidth = baseWidth * (_maxMana / 100f);

            bgTransform.sizeDelta = new Vector2(newWidth, bgTransform.sizeDelta.y);
            barTransform.sizeDelta = new Vector2(newWidth, barTransform.sizeDelta.y);
        }      
    }
}
