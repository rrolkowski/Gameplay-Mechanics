using UnityEngine;
using UnityEngine.UI;

public class PStamina : MonoBehaviour
{
    private Statistics _statistics;

    [Header("Stamina Settings")]
    [SerializeField] float _minStamina = 0f;
    [SerializeField] float _currentStamina = 100f;
    [SerializeField] float _maxStamina = 100f;
    [SerializeField] float _rollCost = 20f;
    [SerializeField] float _increaseRate = 5f;

    [Header("UI Elements")]
    [SerializeField] private Image _staminaBar;
    [SerializeField] private RectTransform _staminaBackground;

    [Header("References")]
    [SerializeField] private PMovement _pMovement;

    private void Awake()
    {
        _statistics = GetComponent<StatisticsProvider>().GetStatistics();
    }
    void Start()
    {
        _statistics = StatisticsProvider.Instance.GetStatistics();
        if (_staminaBar != null)
        {
            _staminaBar.fillAmount = _currentStamina / _maxStamina;
        }

        _statistics.StatPointsChanged.AddListener(UpdateMaxStamina);
    }
    private void OnDestroy()
    {
        _statistics.StatPointsChanged.RemoveListener(UpdateMaxStamina);
    }
    void Update()
    {
        if(_pMovement.wasRollingTriggered && _pMovement.isRolling)
        {
            _currentStamina -= _rollCost;
            _pMovement.wasRollingTriggered = false;
        }
        else
        {
            if(_currentStamina < _maxStamina)
            _currentStamina += _increaseRate * Time.deltaTime;
        }

        _currentStamina = Mathf.Clamp(_currentStamina, _minStamina, _maxStamina);

        if (_staminaBar != null)
        {        
            _staminaBar.fillAmount = _currentStamina / _maxStamina;
        }
    }
    void UpdateMaxStamina()
    {
        _maxStamina = 100 + (_statistics.statPoints[BaseStatType.ENDURANCE] * 5f);

        if (_currentStamina == _maxStamina - 5f)
        {
            _currentStamina = _maxStamina;
        }

        UpdateStaminaBackgroundSize();
    }

    public float GetMaxStamina()
    {
        return _maxStamina;
    }

    public float GetCurrentStamina()
    {
        return _currentStamina;
    }

    public float GetRollCost()
    {
        return _rollCost;
    }

    void UpdateStaminaBackgroundSize()
    {
        if (_staminaBackground != null && _staminaBar != null)
        {
            RectTransform bgTransform = _staminaBackground;
            RectTransform barTransform = _staminaBar.rectTransform;

            float baseWidth = 100f;
            float newWidth = baseWidth * (_maxStamina / 100f);

            bgTransform.sizeDelta = new Vector2(newWidth, bgTransform.sizeDelta.y);
            barTransform.sizeDelta = new Vector2(newWidth, barTransform.sizeDelta.y);
        }
    }

}
