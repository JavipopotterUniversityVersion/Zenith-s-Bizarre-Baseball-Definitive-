using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class UpgradeDealer : MonoBehaviour
{
    [SerializeField] UpgradeData[] _upgradeDatas;
    [SerializeField] ObjectProcessor _numberOfUpgrades;

    UpgradeReceiver[] _upgradeReceivers;

    private void Awake() 
    {
        _upgradeReceivers = GetComponentsInChildren<UpgradeReceiver>();

        List<UpgradeData> availableUpgrades = _upgradeDatas.ToList();
        for (int i = 0; i < _numberOfUpgrades.Result(); i++)
        {
            UpgradeData upgradeData = UpgradeData.GetRandomUpgrade(availableUpgrades);
            availableUpgrades.Remove(upgradeData);

            _upgradeReceivers[i].SetUpgrade(upgradeData.Upgrade.Upgrade);
        }

        ActivateReceivers(_numberOfUpgrades.Result());
    }

    void ActivateReceivers(float index)
    {
        for(int i = 0; i < _upgradeReceivers.Length; i++)
        {
            _upgradeReceivers[i].gameObject.SetActive(i < index);
        }
    }
}

[Serializable] 
public class UpgradeData
{
    [SerializeField] UpgradeInstance _upgrade;
    public UpgradeInstance Upgrade => _upgrade;

    [SerializeField] [Range(0, 1)] float _chance;

    public static UpgradeData GetRandomUpgrade(List<UpgradeData> upgradeDatas) => GetRandomUpgrade(upgradeDatas.ToArray());

    public static UpgradeData GetRandomUpgrade(UpgradeData[] upgradeDatas)
    {
        List<UpgradeData> availableUpgrades = upgradeDatas.ToList();
        
        float totalChance = availableUpgrades.Sum(x => x._chance);

        float randomValue = UnityEngine.Random.Range(0, totalChance);

        float currentChance = 0;
        foreach (UpgradeData upgradeData in availableUpgrades)
        {
            currentChance += upgradeData._chance;
            if (randomValue <= currentChance) return upgradeData;
        }

        return availableUpgrades[0];
    }
}
