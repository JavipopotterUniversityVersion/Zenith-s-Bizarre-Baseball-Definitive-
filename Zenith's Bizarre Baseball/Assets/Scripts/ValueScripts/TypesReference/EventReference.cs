using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReference : MonoBehaviour
{
    [SerializeField] Event _event;
    public void Invoke() => _event.Invoke();
}
