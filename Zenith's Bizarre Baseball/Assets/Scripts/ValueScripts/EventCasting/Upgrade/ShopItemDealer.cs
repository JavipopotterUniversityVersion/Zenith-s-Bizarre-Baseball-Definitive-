using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class ShopItemDealer : MonoBehaviour
{
    [SerializeField] ObjectProcessor _numberOfItems;
    [SerializeField] ShopItemData[] _upgradeDatas;
    [SerializeField] ObjectProcessor _costProcessor;
    [Space(20)]
    [SerializeField] Int _moneyType;
    [SerializeField] ObjectProcessor _purchaseCondition;

    ShopItemReceiver[] _itemReceivers;
    [SerializeField] string _costIcon = "<sprite=\"UpgradeOrbs\" index=0>";

    private void Start() 
    {
        _itemReceivers = GetComponentsInChildren<ShopItemReceiver>();

        List<ShopItemData> availableUpgrades = _upgradeDatas.ToList();
        for (int i = 0; i < _numberOfItems.Result(); i++)
        {
            ShopItemData upgradeData = ShopItemData.GetRandomItem(availableUpgrades);
            availableUpgrades.Remove(upgradeData);

            float cost = _costProcessor.Result(upgradeData.Cost);

            _itemReceivers[i].SetItem(upgradeData.Upgrade.Upgrade,
             _purchaseCondition, cost + _costIcon, () => 
            {
                _moneyType.SubtractValue((int) cost);
            });
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
        [SerializeField] float _cost;
        public float Cost => _cost;

        public static ShopItemData GetRandomItem(List<ShopItemData> upgradeDatas) => GetRandomUpgrade(upgradeDatas.ToArray()) as ShopItemData;
    }
}
