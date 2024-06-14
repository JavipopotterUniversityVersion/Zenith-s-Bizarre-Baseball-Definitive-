using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeContainer : MonoBehaviour
{
    [SerializeField] UpgradeInstance _upgradeInstance;
    [SerializeField] UnityEvent<Upgrade> _onSetUpgrade = new UnityEvent<Upgrade>();

    private void Awake() {
        _upgradeInstance.OnSetUpgrade.AddListener(UpdateUpgrade);
    }

    void UpdateUpgrade() {
        _onSetUpgrade.Invoke(_upgradeInstance.Upgrade);
    }

    private void OnDestroy() {
        _upgradeInstance.OnSetUpgrade.RemoveListener(UpdateUpgrade);
    }
}
