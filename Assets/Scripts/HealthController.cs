using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public static Action<int> OnDied; 

    public ObjectType objectType;

    private Statistics _statistics;

    [Header("HP Settings")]
    [SerializeField] float _currentHP;
    [SerializeField] float _maxHP;

    [Header("References")]
    [SerializeField ] Image healthSlider;
    [SerializeField] GameObject DmgCanvas;

    [Header("Set for Player Only!")]
    [Tooltip("Set for Player Only!")]
    [SerializeField] RectTransform _healthBackground = null;

    private float _damageMultiplier = 1f;


    private void Start()
    {
        _statistics = GetComponent<StatisticsProvider>().GetStatistics();
        _maxHP = _statistics.baseMaxHealth;
        _currentHP = _maxHP;
        healthSlider.fillAmount = _currentHP / _maxHP;

        _statistics.StatPointsChanged.AddListener(UpdateVitality);
    }

    private void OnDestroy()
    {
        _statistics.StatPointsChanged.RemoveListener(UpdateVitality);
    }
    public void TakeDamage(float damage, bool isCritical)
    {
        if (damage < 0) return;
        damage *= _damageMultiplier;

        _currentHP -= damage;
        healthSlider.fillAmount = _currentHP / _maxHP;

        if (DmgCanvas)
        {
            ShowDMG(damage, isCritical);
        }

        EvaluateDeath();
    }

    public void ShowDMG(float damage, bool isCritical)
    {
        var dmgInfo = Instantiate(DmgCanvas, transform.position, Quaternion.identity, transform);
        var textComponent = dmgInfo.GetComponentInChildren<TextMeshProUGUI>();

        if (isCritical)
        {
            textComponent.color = Color.cyan;
            textComponent.fontSize *= 2.5f;
        }
        else if (objectType == ObjectType.Player)
        {
            textComponent.color = Color.red;
        }
        else
        {
            textComponent.color = Color.white;
        }

        textComponent.text = damage.ToString();
    }

    public void Heal(float heal)
    {
        if(heal < 0) return;
        _currentHP += heal;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        healthSlider.fillAmount = _currentHP / _maxHP;
    }

    private void EvaluateDeath()
    {
        if (_currentHP > 0) return;

        if (objectType == ObjectType.Dummy) 
            return;

        if (objectType == ObjectType.Enemy)
            QuestManager.Instance?.RegisterKill();

        Destroy(gameObject);

        OnDied?.Invoke(_statistics.xpWorth);

    }
    public void ResetHealth()
    {
        _currentHP = _maxHP;
        healthSlider.fillAmount = _currentHP / _maxHP;
    }
    public float GetMaxHP()
    {
        return _maxHP;
    }

    public float GetCurrentHP()
    {
        return _currentHP;
    }

    public void UpdateVitality()
    {
        if(objectType == ObjectType.Player)
        {
           _maxHP = _statistics.baseMaxHealth + (_statistics.statPoints[BaseStatType.VITALITY] * 5f);
          healthSlider.fillAmount = _currentHP / _maxHP;

            UpdateVitalityBar();

        }
        else
        {
            _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
            healthSlider.fillAmount = _currentHP / _maxHP;
        }
    }
    void UpdateVitalityBar()
    {
        if (_healthBackground != null && healthSlider != null)
        {
            RectTransform bgTransform = _healthBackground;
            RectTransform barTransform = healthSlider.rectTransform;

            float baseWidth = 100f;
            float newWidth = baseWidth * (_maxHP / 100f);

            bgTransform.sizeDelta = new Vector2(newWidth, bgTransform.sizeDelta.y);
            barTransform.sizeDelta = new Vector2(newWidth, barTransform.sizeDelta.y);
        }
    }

    public void UpdateDamageMultiplier(float multiplier)
    {
        _damageMultiplier = multiplier;
    }
}

public enum ObjectType
{
    NotAssigned,
    Player,
    Enemy,
    Dummy,
}