using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UpgradeReceiver))]
public class ShopItemReceiver : MonoBehaviour
{
    UpgradeReceiver _upgradeReceiver;

    [SerializeField] UnityEvent<ObjectProcessor> _onReceiveCost = new UnityEvent<ObjectProcessor>();
    [SerializeField] UnityEvent<string> _onReceiveCostName = new UnityEvent<string>();

    private void Awake() => _upgradeReceiver = GetComponent<UpgradeReceiver>();
    public void SetItem(Upgrade upgrade, ObjectProcessor cost, string costName)
    {
        _upgradeReceiver.SetUpgrade(upgrade);
        _onReceiveCost.Invoke(cost);
        _onReceiveCostName.Invoke(costName);
    }
}
