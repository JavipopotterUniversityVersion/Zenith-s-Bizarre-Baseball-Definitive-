using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatReadable : MonoBehaviour, IReadable
{
    float _value;
    public float Value => _value;

    public float Read() => _value;

    public void SetValue(float value) => _value = value;
    public void SumValue(float value) => _value += value;
}
