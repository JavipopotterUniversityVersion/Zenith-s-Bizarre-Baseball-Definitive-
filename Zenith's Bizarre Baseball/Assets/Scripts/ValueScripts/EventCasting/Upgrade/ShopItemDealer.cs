using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class ShopItemDealer : MonoBehaviour
{
    [SerializeField] ShopItemData[] _upgradeDatas;
    [SerializeField] ObjectProcessor _numberOfItems;

    ShopItemReceiver[] _itemReceivers;

    private void Start() 
    {
        _itemReceivers = GetComponentsInChildren<ShopItemReceiver>();

        List<ShopItemData> availableUpgrades = _upgradeDatas.ToList();
        for (int i = 0; i < _numberOfItems.Result(); i++)
        {
            ShopItemData upgradeData = ShopItemData.GetRandomItem(availableUpgrades);
            availableUpgrades.Remove(upgradeData);

            _itemReceivers[i].SetItem(upgradeData.Upgrade.Upgrade, upgradeData.Cost, upgradeData.CostName);
        }

        ActivateReceivers(_numberOfItems.Result());
    }

    void ActivateReceivers(float index)
    {
        for(int i = 0; i < _itemReceivers.Length; i++)
        {
            _itemReceivers[i].gameObject.SetActive(i < index);
        }
    }

    [Serializable] 
    class ShopItemData : UpgradeData
    {
        [SerializeField] ObjectProcessor _cost;
        public ObjectProcessor Cost => _cost;

        [SerializeField] string _costName;
        public string CostName => _costName;

        public static ShopItemData GetRandomItem(List<ShopItemData> upgradeDatas) => GetRandomUpgrade(upgradeDatas.ToArray()) as ShopItemData;
    }
}
