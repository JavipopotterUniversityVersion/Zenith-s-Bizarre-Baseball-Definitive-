using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeClassContainer : MonoBehaviour
{
    Upgrade upgrade;
    [SerializeField] UnityEvent<Upgrade> _onCallUpgrade = new UnityEvent<Upgrade>();

    public void SetUpgrade(Upgrade upgrade) => this.upgrade = upgrade;
    public void CallUpgrade() => _onCallUpgrade.Invoke(upgrade);

    public void SetAndCallUpgrade(Upgrade upgrade)
    {
        SetUpgrade(upgrade);
        CallUpgrade();
    }
}
