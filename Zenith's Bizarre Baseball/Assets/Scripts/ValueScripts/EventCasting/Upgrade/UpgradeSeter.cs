using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeSeter : MonoBehaviour
{
    [SerializeField] UpgradeSetReference _optionSetReference;
    UpgradeReceiver[] _upgradeReceivers;

    private void Awake() {
        _optionSetReference.OnReferenceCall.AddListener(SetOptions);
        _upgradeReceivers = GetComponentsInChildren<UpgradeReceiver>(true);
    }

    void SetOptions()
    {
        List<Upgrade> availableUpgrades = _optionSetReference.Upgrades.ToList();

        for(int i = 0; i < availableUpgrades.Count; i++)
        {
            _upgradeReceivers[i].SetUpgrade(availableUpgrades[i]);
        }

        ActivateReceivers(availableUpgrades.Count);
    }

    public void ActivateReceivers(int index)
    {
        for(int i = 0; i < _upgradeReceivers.Length; i++)
        {
            _upgradeReceivers[i].gameObject.SetActive(i < index);
        }
    }

    private void OnDestroy() {
        _optionSetReference.OnReferenceCall.RemoveListener(SetOptions);
    }
}
