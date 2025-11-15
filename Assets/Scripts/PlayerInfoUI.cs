using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] GameObject playerInfoUI;

    [Header("References")]
    [SerializeField] private HealthController _pHealth;
    [SerializeField] private PStamina _pStamina;
    [SerializeField] private PMovement _pMovement;
    [SerializeField] private PlayerAttack _pAttack;
    [SerializeField] private PMana _pMana;

    [HideInInspector]
    [SerializeField] private Statistics _statistics;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _currentLVL;
    [SerializeField] private TextMeshProUGUI _currentXP;
    [SerializeField] private TextMeshProUGUI _maxHpText;
    [SerializeField] private TextMeshProUGUI _critChanceText;
    [SerializeField] private TextMeshProUGUI _staminaText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _pManaText;
    


    void Start()
    {
        _statistics = StatisticsProvider.Instance.GetStatistics();
        Debug.Log($"Starting level: {_statistics.currentlvl}");
        playerInfoUI.SetActive(false);

        _statistics.XPChanged.AddListener(ShowPlayerInfo);
        _statistics.LevelUp.AddListener(ShowPlayerInfo);
    }

    public void ShowPlayerInfo()
    {
              
        _currentLVL.text=  $"LVL: {_statistics.currentlvl}";
        _currentXP.text = $"XP: {_statistics.CurrentXP} / {_statistics.GetCurrentLevelXPRequirement()}";

        if (_pHealth != null)
        {
            _maxHpText.text = $"Max HP: <color=#FF0000>{_pHealth.GetMaxHP()}";
        }
        if( _pMana != null)
        {
            _pManaText.text = $"Max Mana: <color=#0000FF>{_pMana.GetMaxMana()}";
        }
        if (_pStamina != null)
        {
            _staminaText.text = $"Max Stamina: <color=#00FF00>{_pStamina.GetMaxStamina()}";
        }
        if (_pAttack != null)
        {
            _critChanceText.text = $"Crit Chance: <color=#00FFFF>{_pAttack.GetCriticalChance()}%</color>";
        }
        if (_pMovement != null)
        {
            _speedText.text = $"Movement Speed: <color=#FFFF00>{(int)_pMovement.GetMoveSpeed()}%";
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            playerInfoUI.SetActive(!playerInfoUI.activeSelf);
            if (playerInfoUI.activeSelf)
            {
                ShowPlayerInfo();
            }
            
        }
    }
}
