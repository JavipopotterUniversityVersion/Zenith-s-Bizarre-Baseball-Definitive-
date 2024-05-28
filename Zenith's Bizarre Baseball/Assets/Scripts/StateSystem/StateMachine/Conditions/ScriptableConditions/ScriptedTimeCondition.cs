using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeCondition", menuName = "Conditions/TimeCondition")]
public class ScriptedTimeCondition : ScriptableICondition
{
    [SerializeField] float _time;

    public void SetTime(float time)
    {
        _time = Time.time + time;
    }

    public override bool CheckCondition() => Time.time >= _time;
}