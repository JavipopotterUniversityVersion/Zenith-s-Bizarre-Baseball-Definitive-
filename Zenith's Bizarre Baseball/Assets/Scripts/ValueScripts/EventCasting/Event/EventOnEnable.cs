using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
    [SerializeField] UnityEvent _onEnableEvent;
    private void OnEnable() {
        _onEnableEvent.Invoke();
    }
}
