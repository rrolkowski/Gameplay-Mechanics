using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour
{
    [Header("Health Potion")]
    [SerializeField] private HealthController _healthController;
    [SerializeField] Image _healthPotion;
    [SerializeField] int _refillHealthValue = 50;
    [SerializeField] float _maxHealthCharges;
    private float _currentHealthCharges;

    [Header("Health Potion")]
    [SerializeField] private PMana _pMana;
    [SerializeField] Image _manaPotion;
    [SerializeField] int _refillManaValue = 50;
    [SerializeField] float _maxManaCharges;
    private float _currentManaCharges;

    private void Start()
    {
        //Health
        _currentHealthCharges = _maxHealthCharges;
        _healthPotion.fillAmount = _currentHealthCharges / _maxHealthCharges;

        //Mana
        _currentManaCharges = _maxManaCharges;
        _manaPotion.fillAmount = _currentManaCharges / _maxManaCharges;
    }

    public void OnUseHealthPotion(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UseHealthPotion();
        }
    }

    public void OnUseManaPotion(InputAction.CallbackContext ctx) 
    {
        if(ctx.performed)
        {
            UseManaPotion();
        }
    }


    public void UseHealthPotion()
    {
        if (_healthController != null)
        {
            if (_currentHealthCharges <= 0)
                return;

            _healthController.Heal(_refillHealthValue);
            _currentHealthCharges -= 1;
            _healthPotion.fillAmount = _currentHealthCharges / _maxHealthCharges;
        }
    }

    public void UseManaPotion()
    {
        if(_pMana != null)
        {
            if(_currentManaCharges <= 0)
                return;

            _pMana.RefillMana(_refillManaValue);
            _currentManaCharges -= 1;
            _manaPotion.fillAmount = _currentManaCharges / _maxManaCharges;
        }
    }

    
}
