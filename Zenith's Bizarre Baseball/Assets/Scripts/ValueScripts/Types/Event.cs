using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Event", menuName = "Value/Event")]
public class Event : ScriptableObject
{
    [SerializeField] UnityEvent _onEvent;
    public void Invoke() => _onEvent.Invoke();
}