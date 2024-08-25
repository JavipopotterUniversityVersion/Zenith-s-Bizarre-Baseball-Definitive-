using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SendFloatEventBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] ObjectProcessor _value;
    [SerializeField] UnityEvent<float> _onValueSent;

    public void ExecuteBehaviour() => _onValueSent.Invoke(_value.Result());
}
