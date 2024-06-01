using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventByTextReceiver : MonoBehaviour
{
    [SerializeField] String _string;
    [SerializeField] StringEvent[] _stringEvents;

    string[] GetEvents(string input) => input.Split(',');

    private void Awake()
    {
        _string.OnStringCall.AddListener(() => Search(_string.Value));
    }

    public void ExecuteEvents(string[] eventsToSearch)
    {
        int eventsFound = 0;

        int i = 0;
        while (i < _stringEvents.Length && eventsFound < eventsToSearch.Length)
        {
            
            if(_stringEvents[i].CheckConditions())
            {
                int a = 0;
                bool found = false;

                while(a < eventsToSearch.Length && !found)
                {
                    eventsToSearch[a] = eventsToSearch[a].Trim();
                    if(_stringEvents[i].key == eventsToSearch[a] == _stringEvents[i].negated)
                    {
                        _stringEvents[i].value.Invoke();

                        found = true;
                        eventsFound++;
                    }

                    a++;
                }
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
public struct StringEvent
{
    [SerializeField] ScriptableCondition[] conditions;
    public readonly bool CheckConditions() => ScriptableCondition.CheckAllConditions(conditions);

    public string key;
    public UnityEvent value;
    public bool negated;
}