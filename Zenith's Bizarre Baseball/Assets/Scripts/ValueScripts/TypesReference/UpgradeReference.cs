using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UpgradeReference", menuName = "Value/UpgradeReference")]
public class UpgradeReference : ScriptableObject
{
    [SerializeField] Upgrade _value;
    public Upgrade Value => _value;

    UnityEvent _onRefChange = new UnityEvent();
    public UnityEvent OnRefChange => _onRefChange;

    public void SetUpgrade(Upgrade upgrade)
    {
        _value = upgrade;
        _onRefChange.Invoke();
    }
}