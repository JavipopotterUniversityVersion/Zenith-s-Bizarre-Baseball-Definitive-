using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{
    [SerializeField] Int _statValue;
    [SerializeField] string _statName;
    [SerializeField] int _statCost;
    [SerializeField] int _statMaxValue;
    [SerializeField] TextMeshProUGUI _statNameText;
    [SerializeField] Button _addButton;
    [SerializeField] Button _substractButton;
    Int _statPoints;
    [SerializeField] Slider _attachedSlider;
    [SerializeField] UnityEvent _onStatChange;

    private void Awake() 
    {
        _statPoints = Resources.Load<Int>("StatPoints");
        _statNameText.text = _statName;
        _addButton.onClick.AddListener(AddStat);
        _substractButton.onClick.AddListener(SubstractStat);
    }

    private void OnDestroy() {
        _addButton.onClick.RemoveListener(AddStat);
        _substractButton.onClick.RemoveListener(SubstractStat);
    }

    private void OnEnable() => UpdateStatButton();

    public void AddStat()
    {
        if (_statPoints.Value >= _statCost && _statValue.Value < _statMaxValue)
        {
            _statPoints.SubtractValue(_statCost);
            _statValue.AddValue(1);
            _onStatChange.Invoke();
            UpdateStatButton();
        }
    }

    public void SubstractStat()
    {
        if (_statValue.Value > 0)
        {
            _statPoints.AddValue(_statCost);
            _statValue.SubtractValue(1);
            _onStatChange.Invoke();
            UpdateStatButton();
        }
    }

    void UpdateStatButton()
    {
        _attachedSlider.maxValue = _statMaxValue;
        _attachedSlider.value = _statValue.Value;
    }

    private void OnValidate() {
        name = "Stat: " + _statName;
        _statNameText.text = _statName;
    }
}
