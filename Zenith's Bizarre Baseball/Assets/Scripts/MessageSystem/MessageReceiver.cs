using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageReceiver : MonoBehaviour
{
    [SerializeField] SerializableDictionary<Identifiable, UnityEvent> _events;

    public void InvokeEvent(Identifiable identifiable)
    {
        if(_events.TryGetValue(identifiable, out UnityEvent unityEvent)) unityEvent.Invoke();
    }
}
