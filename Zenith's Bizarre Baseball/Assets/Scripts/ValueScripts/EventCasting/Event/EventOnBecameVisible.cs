using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnBecameVisible : MonoBehaviour
{
    [SerializeField] UnityEvent _onBecameVisible;
    private void OnBecameVisible()
    {
        _onBecameVisible.Invoke();
    }
}
