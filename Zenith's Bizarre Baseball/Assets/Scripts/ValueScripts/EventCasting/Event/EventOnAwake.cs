using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnAwake : MonoBehaviour
{
    [SerializeField] UnityEvent _onAwake = new UnityEvent();

    private void Awake() => _onAwake?.Invoke();
}
