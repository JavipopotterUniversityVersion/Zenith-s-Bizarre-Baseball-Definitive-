using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnBecameInvisible : MonoBehaviour
{
    [SerializeField] UnityEvent _onBecameInvisible;
    private void OnBecameInvisible()
    {
        _onBecameInvisible?.Invoke();
    }
}
