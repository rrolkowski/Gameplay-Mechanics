using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    private Statistics _statistics;

    [Header("BASE DMG")]   
    [SerializeField] private int damage = 20;

    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject _oneHandSword;
    [SerializeField] GameObject _twoHandSword;

    private bool canDamage = false;
    private float shrineCritChance;

    private HashSet<HealthController> hitEnemies = new HashSet<HealthController>();
    private void Awake()
    {
        _statistics = GetComponent<StatisticsProvider>().GetStatistics();
    }


    private void Start()
    {
        _statistics = StatisticsProvider.Instance.GetStatistics();
        _oneHandSword.SetActive(true);
        _twoHandSword.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            _oneHandSword.SetActive(!_oneHandSword.activeSelf);
            _twoHandSword.SetActive(!_twoHandSword.activeSelf);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (_oneHandSword.activeSelf)
            {
                _animator.SetTrigger("AttackOneHandTrigger");
                hitEnemies.Clear();
            }
            if(_twoHandSword.activeSelf)
            {
                _animator.SetTrigger("AttackTwoHandTrigger");
                hitEnemies.Clear();
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other is SphereCollider)
        {
            return;
        }

        HealthController healthController = other.GetComponent<HealthController>();
        if (healthController == null) return;

        if (!hitEnemies.Contains(healthController))
        {
            bool isCritical;
            float finalDMG = GetDamage(out isCritical);
            healthController.TakeDamage(finalDMG, isCritical);
            hitEnemies.Add(healthController);
        }
    }
    public void StartDamagePhase()
    {
        hitEnemies.Clear();
        canDamage = true;
    }

    public void EndDamagePhase()
    {
        canDamage = false;
    }

    public float GetDamage(out bool isCritical)
    {
        float baseDamage = damage + (damage * _statistics.statPoints[BaseStatType.STRENGTH] * 0.1f);

        if (_twoHandSword.activeSelf)
        {
            baseDamage += 20;
        }

        float critChance = GetCriticalChance();

        if (Random.Range(0f, 100f) < critChance)
        {
            baseDamage *= 2;
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }

        return baseDamage;
    }

    public float GetCriticalChance()
    {
        float baseCritChance = 10f + (_statistics.statPoints[BaseStatType.PRECISION] * 2f);
        return shrineCritChance > 0 ? shrineCritChance : baseCritChance;
    }

    public void ShrineCritChance(float newCritChance)
    {
        shrineCritChance = newCritChance;
    }

}
