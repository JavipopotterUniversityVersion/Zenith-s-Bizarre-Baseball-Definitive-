using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnBecameInvisible : MonoBehaviour
{
    [SerializeField] UnityEvent _onBecameInvisible;
    [SerializeField] bool _active = true;
    public void SetActive(bool active) => _active = active;
    private void OnBecameInvisible()
    {
        if (_active) _onBecameInvisible?.Invoke();
    }
}
