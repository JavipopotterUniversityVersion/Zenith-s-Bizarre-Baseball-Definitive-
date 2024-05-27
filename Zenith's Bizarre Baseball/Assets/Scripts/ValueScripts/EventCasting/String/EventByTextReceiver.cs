using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventByTextReceiver : MonoBehaviour
{
    [SerializeField] String _string;
    [SerializeField] StringEvent[] _stringEvents;

    private void Awake()
    {
        _string.OnStringCall.AddListener(UpdateText);
    }

    void UpdateText()
    {
        int i = 0;
        bool found = false;
        while (i < _stringEvents.Length && !found)
        {
            if(_stringEvents[i].key == _string.Value == _stringEvents[i].negated)
            {
                _stringEvents[i].value.Invoke();
                found = true;
            }
            i++;
        }
    }

    private void OnDestroy() {
        _string.OnStringCall.RemoveListener(UpdateText);
    }
}

[Serializable]
public struct StringEvent
{
    public string key;
    public UnityEvent value;
    public bool negated;
}