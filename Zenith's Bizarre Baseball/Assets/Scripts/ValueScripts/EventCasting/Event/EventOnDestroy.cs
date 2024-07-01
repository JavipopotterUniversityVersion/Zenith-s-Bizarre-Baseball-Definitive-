using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnDestroy : MonoBehaviour
{
    [SerializeField] UnityEvent _onDestroy = new UnityEvent();

    private void OnDestroy() => _onDestroy?.Invoke();
}
