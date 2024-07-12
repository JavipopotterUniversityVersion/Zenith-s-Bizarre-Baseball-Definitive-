using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Value/Upgrade")]
public class UpgradeInstance : ScriptableObject
{
    [SerializeField] Upgrade _upgrade;
    public Upgrade Upgrade => _upgrade;

    UnityEvent onSetUpgrade = new UnityEvent();
    public UnityEvent OnSetUpgrade => onSetUpgrade;

    public void CopyUpgrade(UpgradeInstance upgrade) => SetUpgrade(upgrade.Upgrade);
    public void SetUpgrade(Upgrade upgrade)
    {
        _upgrade = upgrade;
        onSetUpgrade.Invoke();
    }
}

[System.Serializable]
public class Upgrade
{
    [SerializeField] string _name;
    public string Name => _name;
    [SerializeField] UnityEngine.Sprite _icon;
    public UnityEngine.Sprite Icon => _icon;
    [TextArea(3, 10)]
    [SerializeField] string _description;
    public string Description => _stringProcessor.Process(_description);
    [SerializeField] StringProcessor _stringProcessor;

    [SerializeField] Processor _condition;
    public bool CanAppear => _condition.Result(1) != 0;

    [SerializeField] Processor _upgrade;
    public bool ApplyUpgrade() => _upgrade.ResultBool(1);
}