using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Curve", menuName = "Value/Curve")]
public class Curve : ScriptableObject
{
    [SerializeField] AnimationCurve _value;
    public AnimationCurve Value => _value;
    
    public float Evaluate(float time) => _value.Evaluate(time);
}
