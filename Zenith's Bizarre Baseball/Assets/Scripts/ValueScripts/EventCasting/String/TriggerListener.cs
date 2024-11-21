using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerListener : MonoBehaviour
{
    [SerializeField] String _trigger;
    [SerializeField] UnityEvent _onTrigger;
    
    private void Awake()
    {
        _trigger.OnStringCall.AddListener(() => _onTrigger.Invoke());
    }

    private void OnDestroy()
    {
        _trigger.OnStringCall.RemoveListener(() => _onTrigger.Invoke());
    }
}
