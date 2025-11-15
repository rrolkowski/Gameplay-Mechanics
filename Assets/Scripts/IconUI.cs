using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    [Header("Weapon Icons")]
    [SerializeField] private GameObject _oneHand;
    [SerializeField] private GameObject _twoHand;

    [Header("Sword Objects")]
    [SerializeField] private GameObject _oneHandWeapon;
    [SerializeField] private GameObject _twoHandWeapon;

    [Header("Shrine Buff Icons")]
    [SerializeField] private Image _shrineBuffIcon;
    [SerializeField] private GameObject _shrineBuffContainer;

    [Header("UI Buff Icon")]
    [SerializeField] private Image _uiBuffIcon;
    [SerializeField] private TextMeshProUGUI _uiBuffTimer;
    [SerializeField] private GameObject _uiBuffContainer;

    [Header("Buff Sprites")]
    [SerializeField] private Sprite _critBuffSprite;
    [SerializeField] private Sprite _speedBuffSprite;

    private void Awake()
    {
        _oneHand.SetActive(true);
        _twoHand.SetActive(false);

        _shrineBuffContainer.SetActive(false);
        _uiBuffContainer.SetActive(false);
    }

    private void Update()
    {

        if (_oneHandWeapon.activeSelf)
        {
            _oneHand.SetActive(true);
            _twoHand.SetActive(false);
        }
        if (_twoHandWeapon.activeSelf)
        {
            _oneHand.SetActive(false);
            _twoHand.SetActive(true);
        }
    }

    public void ShowShrineBuffIcon(BuffType buffType)
    {
        _shrineBuffIcon.sprite = (buffType == BuffType.CritChance) ? _critBuffSprite : _speedBuffSprite;
        _shrineBuffContainer.SetActive(true);
        Debug.Log("Pokazano ikonê Shrine Buffa");
    }

    public void ActivateBuffUI(BuffType buffType, float buffDuration)
    {
        _shrineBuffContainer.SetActive(false);

        _uiBuffIcon.sprite = (buffType == BuffType.CritChance) ? _critBuffSprite : _speedBuffSprite;
        _uiBuffContainer.SetActive(true);

        StartCoroutine(UpdateBuffTimer(buffDuration));
    }

    private IEnumerator UpdateBuffTimer(float duration)
    {
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            _uiBuffTimer.text = $"{timeLeft:F1}s";
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
        }

        _uiBuffContainer.SetActive(false);
    }
}
