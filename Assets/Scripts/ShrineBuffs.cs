using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ShrineBuffs : MonoBehaviour
{
    private BuffType _selectedBuff;

    [Header("Shrine Settings")]
    [SerializeField] private float _buffTime = 5f;
    [Tooltip("Value addition")]
    [SerializeField] private float _increasedCritChance = 90f;
    [Tooltip("Value multiplier")]
    [SerializeField] private float _increasedMovementSpeed = 2f;

    [Header("References")]
    [SerializeField] private PlayerAttack _playerattack;
    [SerializeField] private PlayerInfoUI _playerInfoUI;
    [SerializeField] private PMovement _pMovement;
    [SerializeField] private IconUI _iconUI;

    private bool _isActive = false;

    private void Start()
    {
        _selectedBuff = (Random.value > 0.5f) ? BuffType.CritChance : BuffType.MoveSpeed;
        _iconUI.ShowShrineBuffIcon(_selectedBuff);
    }

    private void OnMouseDown()
    {
        if (!_isActive)
        {
            _isActive = true;

            Debug.Log($"Shrine Klikniêty! Wylosowany buff: {_selectedBuff}");

            _iconUI.ActivateBuffUI(_selectedBuff, _buffTime);
            ApplyBuff();

            if (_playerInfoUI != null)
            {
                _playerInfoUI.ShowPlayerInfo();
            }
        }    
    }
    private void ApplyBuff()
    {
        if (_selectedBuff == BuffType.CritChance)
        {
            StartCoroutine(ApplyCritBuff());
        }
        else if (_selectedBuff == BuffType.MoveSpeed)
        {
            StartCoroutine(ApplySpeedBuff());
        }
    }
    IEnumerator ApplyCritBuff()
    {
        if (!_isActive)  
            
        _isActive = true;
        float originalCritChance = _playerattack.GetCriticalChance();
        float boostedCritChance = originalCritChance + _increasedCritChance;

        _playerattack.ShrineCritChance(boostedCritChance);
        Debug.Log("Crit CHance: +90%");

        yield return new WaitForSeconds(_buffTime);

        _playerattack.ShrineCritChance(originalCritChance);
        Debug.Log("End Buff");

        if (_playerInfoUI != null)
        {
            _playerInfoUI.ShowPlayerInfo();
        }
    }

    IEnumerator ApplySpeedBuff()
    {
        float originalMoveSpeed = _pMovement.GetRawMoveSpeed();
        float boostedMoveSpeed = originalMoveSpeed * _increasedMovementSpeed;

        _pMovement.SetMoveSpeed(boostedMoveSpeed);
        Debug.Log("Movmement Speed: +50%");

        yield return new WaitForSeconds(_buffTime);

        _pMovement.SetMoveSpeed(originalMoveSpeed);
        Debug.Log("End Buff");

        if (_playerInfoUI != null)
        {
            _playerInfoUI.ShowPlayerInfo();
        }
    }
}

public enum BuffType
{
    CritChance,
    MoveSpeed
}

