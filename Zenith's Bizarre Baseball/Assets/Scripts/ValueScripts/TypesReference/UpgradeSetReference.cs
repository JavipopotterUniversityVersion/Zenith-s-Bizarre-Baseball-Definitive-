using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "UpgradeSetReference", menuName = "TypesReference/UpgradeSetReference")]
public class UpgradeSetReference : ScriptableObject
{
    [SerializeField] UpgradeSet _optionSet;
    public UpgradeSet OptionSet => _optionSet;

    UnityEvent _onReferenceCall = new UnityEvent();
    public UnityEvent OnReferenceCall => _onReferenceCall;

    public Upgrade[] Upgrades => _optionSet.Upgrades;

    public void SetOptionSet(UpgradeSet optionSet)
    {
        _optionSet = optionSet;
        _onReferenceCall.Invoke();
    }
}