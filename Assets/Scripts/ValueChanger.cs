using System;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ValueChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI valueLabel;
    [SerializeField] Button decrementValueButton;
    [SerializeField] Button incrementValueButton;

    private Vector2Int _minMaxRange = new Vector2Int(0, 99);

    [field:SerializeField] public int Value { get; private set; }

    public UnityEvent<int, int> ValueChanged;
    
    public void Init(Vector2Int minMaxRange, int value, string title = "")
    {
        if (title != "")
        {
            label.text = title;
        }
        _minMaxRange = minMaxRange;
        Value = Math.Clamp(value, _minMaxRange.x, _minMaxRange.y);
        RefreshUI();
    }

    private void Start()
    {
        incrementValueButton.onClick.AddListener(IncrementValue);
        decrementValueButton.onClick.AddListener(DecrementValue);  
    }
    private void OnDestroy()
    {
        incrementValueButton?.onClick.RemoveAllListeners();
        decrementValueButton?.onClick.RemoveAllListeners();

    }


    void IncrementValue()
    {
        Value++;
        RefreshUI();
        ValueChanged?.Invoke(Value, 1);
    }

    void DecrementValue()
    {
        Value--;
        RefreshUI();
        ValueChanged?.Invoke(Value, -1);
    }

    void RefreshUI()
    {
        valueLabel.text = Value.ToString();
        incrementValueButton.interactable = Value < _minMaxRange.y;
        decrementValueButton.interactable = Value > _minMaxRange.x;

    }
}

