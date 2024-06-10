using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BehaviourByTextInput : MonoBehaviour
{
    [SerializeField] String _string;
    [SerializeField] StringPerform[] _stringEvents;

    string[] GetEvents(string input) => input.Split(',');

    private void Awake()
    {
        _string.OnStringCall.AddListener(() => Search(_string.Value));
    }

    public void ExecuteEvents(string[] receivedEvents)
    {
        int eventsFound = 0;

        int i = 0;
        while (i < _stringEvents.Length && eventsFound < receivedEvents.Length)
        {
            
                int a = 0;
                bool found = false;

                while(a < receivedEvents.Length && !found)
                {
                    receivedEvents[a] = receivedEvents[a].Trim();
                    if(_stringEvents[i].key == receivedEvents[a] != _stringEvents[i].negated)
                    {
                        _stringEvents[i].Perform();
                        _stringEvents[i].OnKeyFound.Invoke(receivedEvents[a]);

                        found = true;
                        eventsFound++;
                    }

                    a++;
                }

            i++;
        }
    }

    public void Search(string input) => ExecuteEvents(GetEvents(input));

    private void OnDestroy() {
        _string.OnStringCall.RemoveListener(() => Search(_string.Value));
    }
}

[Serializable]
public struct StringPerform
{
    public string key;
    public bool negated;
    [SerializeField] BehaviourPerformer[] _performer;

    [SerializeField] UnityEvent<string> onKeyFound;
    public UnityEvent<string> OnKeyFound => onKeyFound;

    public readonly void Perform() => BehaviourPerformer.Perform(_performer);
}