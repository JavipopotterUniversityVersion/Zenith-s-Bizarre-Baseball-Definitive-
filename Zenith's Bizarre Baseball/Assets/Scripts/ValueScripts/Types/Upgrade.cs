using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Value/Upgrade")]
public class Upgrade : ScriptableObject
{
    [SerializeField] ScriptableCondition[] _appearCondition;
    public bool CanAppear => ScriptableCondition.CheckAllConditions(_appearCondition);

    [SerializeField] UnityEngine.Sprite _icon;
    public UnityEngine.Sprite Icon => _icon;

    [SerializeField] string _name;
    public string Name => _name;

    [TextArea(3, 10)]
    [SerializeField] string _description;
    public string Description => _stringProcessor.Process(_description);
    [SerializeField] StringProcessor _stringProcessor;

    [SerializeField] CondEventVector _upgradeEvent = new CondEventVector();
    public CondEventVector UpgradeEvent => _upgradeEvent;
}
