using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UpgradeReceiver))]
public class ShopItemReceiver : MonoBehaviour
{
    UpgradeReceiver _upgradeReceiver;

    ObjectProcessor _cost;
    UnityAction _purchaseAction;

    [SerializeField] UnityEvent<string> _onReceiveCostName = new UnityEvent<string>();
    [SerializeField] UnityEvent _onPurchase = new UnityEvent();
    [SerializeField] UnityEvent _onPurchaseFailed = new UnityEvent();

    private void Awake() => _upgradeReceiver = GetComponent<UpgradeReceiver>();
    public void SetItem(Upgrade upgrade, ObjectProcessor cost, string costName, UnityAction purchaseAction)
    {
        _purchaseAction = purchaseAction;
        _cost = cost;

        _upgradeReceiver.SetUpgrade(upgrade);
        _onReceiveCostName.Invoke(costName);
    }

    public void Purchase()
    {
        if(_cost.ResultBool())
        {
            _purchaseAction.Invoke();
            _onPurchase.Invoke();
        }
        else
        {
            _onPurchaseFailed.Invoke();
        }
    }
}
