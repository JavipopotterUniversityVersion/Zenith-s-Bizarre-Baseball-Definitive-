using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeReceiver : MonoBehaviour
{
    [SerializeField] UnityEvent<string> _onReceiveName = new UnityEvent<string>();
    [SerializeField] UnityEvent<string> _onReceiveDescription = new UnityEvent<string>();
    [SerializeField] UnityEvent<UnityEngine.Sprite> _onReceiveIcon = new UnityEvent<UnityEngine.Sprite>();
    CondEventVector _upgradeEvent;

    public void SetUpgrade(Upgrade upgrade)
    {
        _onReceiveName.Invoke(upgrade.Name);
        _onReceiveDescription.Invoke(upgrade.Description);
        _onReceiveIcon.Invoke(upgrade.Icon);
        _upgradeEvent = upgrade.UpgradeEvent;
    }

    public void GetUpgrade()
    {
        if(_upgradeEvent.CheckCondition()) _upgradeEvent.Invoke();
    }
}
