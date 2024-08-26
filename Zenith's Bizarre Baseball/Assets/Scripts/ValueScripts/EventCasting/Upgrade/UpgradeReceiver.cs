using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeReceiver : MonoBehaviour
{
    [Header("ON RECEIVE")][Space(3)]
    [SerializeField] UnityEvent<string> _onReceiveName = new UnityEvent<string>();
    [SerializeField] UnityEvent<string> _onReceiveDescription = new UnityEvent<string>();
    [SerializeField] UnityEvent<UnityEngine.Sprite> _onReceiveIcon = new UnityEvent<UnityEngine.Sprite>();
    [SerializeField] UnityEvent<Upgrade> _onReceiveUpgrade = new UnityEvent<Upgrade>();

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
        _onReceiveUpgrade.Invoke(_upgrade);
    }

    public void GetUpgrade() 
    {
       _upgrade.ApplyUpgrade();
    }

    [Serializable]
    class TrueFalseEvents
    {
        [SerializeField] UnityEvent _success;
        [SerializeField] UnityEvent _failure;
        public void Invoke(bool value)
        {
            if(value) _success.Invoke();
            else _failure.Invoke();
        }
    }
}
