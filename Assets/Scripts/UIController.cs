using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] ValueChanger valueChangerPrefab;
    [SerializeField] Transform valueChangerParent;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private Button confirmButton;

    [Header("Scrpits")]
    [SerializeField] private PlayerInfoUI playerInfoUI;

    private CanvasGroup _canvasGroup;
    private bool _visible;

    private Dictionary<BaseStatType, int> _savedStatPoints = new();

    private Statistics _statistics;

    private void Start()
    {
        _statistics = StatisticsProvider.Instance.GetStatistics();
        _statistics.LevelUp.AddListener(OnLeveledlUp);
        _canvasGroup = GetComponent<CanvasGroup>();
        confirmButton.onClick.AddListener(ConfirmStats);
        CacheStats();
        SpawnStatChangers();
        // UpdateUI();     
    }
    private void ConfirmStats()
    {
        CacheStats();
        _statistics.StatPointsChanged?.Invoke();
        confirmButton.interactable = false;
        Debug.Log("CONFIRM");
        _visible = false;
        Refresh();

        if (playerInfoUI != null)
        {
            playerInfoUI.ShowPlayerInfo();
        }
    }

    private void OnLeveledlUp()
    {
        _visible = true;
        Refresh();
    }
    private void CacheStats()
    {
        _savedStatPoints.Clear();
        foreach (var stat in _statistics.statPoints)
        {
            _savedStatPoints.Add(stat.Key, stat.Value);

        }
    }
    private void Update()
    {
        _pointsText.text = $"Available points: {_statistics.BaseStatisticPointsToDistribute}";
        _pointsText.fontSize = 18f;
        UpdateUI();
    }

    void SpawnStatChangers()
    {
        foreach(Transform child in valueChangerParent) Destroy(child.gameObject);

        foreach(var stat in _statistics.statPoints)
        {
            ValueChanger statValueChanger = Instantiate(valueChangerPrefab, valueChangerParent);
            statValueChanger.Init(new Vector2Int(_savedStatPoints[stat.Key], stat.Value + _statistics.BaseStatisticPointsToDistribute), stat.Value, stat.Key.ToString());
            statValueChanger.ValueChanged.AddListener((value, sign) => BaseStatChanged(stat.Key, value, sign));          
        }
    }


    private void BaseStatChanged(BaseStatType statType, int statValue, int sign)
    {
        _statistics.BaseStatisticPointsToDistribute -= sign;
        _statistics.statPoints[statType] = statValue;

        Refresh();
    }

    private void UpdateUI()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _visible = !_visible;
            Refresh();
        }
    }

    private void Refresh()
    {    
        _canvasGroup.alpha = _visible ? 1 : 0;
        _canvasGroup.blocksRaycasts = _visible;
        _canvasGroup.interactable = _visible;

        //if (_visible) return;
        if (!_visible)
        {
            Debug.Log("UI is not visible.");
            return;
        }


        bool canConfirm = false;

        foreach (var stat in _statistics.statPoints)
        {
           if(_savedStatPoints[stat.Key] != stat.Value)
            {
                canConfirm = true;
                break;
            }
        }

        confirmButton.interactable = canConfirm;
        _pointsText.text = $"Stat Points: {_statistics.BaseStatisticPointsToDistribute}";
        SpawnStatChangers();
    }

}
