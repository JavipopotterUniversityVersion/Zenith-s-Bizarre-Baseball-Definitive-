using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeReceiver : MonoBehaviour
{
    [SerializeField] UnityEvent<string> _onReceiveName = new UnityEvent<string>();
    [SerializeField] UnityEvent<string> _onReceiveDescription = new UnityEvent<string>();
    [SerializeField] UnityEvent<UnityEngine.Sprite> _onReceiveIcon = new UnityEvent<UnityEngine.Sprite>();

    Upgrade _upgrade;

    public void SetUpgrade(Upgrade upgrade)
    {
        _upgrade = upgrade;
        UpdateData();
    }

    public void UpdateData()
    {
        _onReceiveName.Invoke(_upgrade.Name);
        _onReceiveDescription.Invoke(_upgrade.Description);
        _onReceiveIcon.Invoke(_upgrade.Icon);
    }

    public void GetUpgrade() => _upgrade.ApplyUpgrade();
}
