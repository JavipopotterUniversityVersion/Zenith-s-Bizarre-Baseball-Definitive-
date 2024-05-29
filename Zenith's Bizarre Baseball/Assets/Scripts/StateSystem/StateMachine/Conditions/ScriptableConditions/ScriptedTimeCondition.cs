using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeCondition", menuName = "Conditions/TimeCondition")]
public class ScriptedTimeCondition : ScriptableICondition
{
    [SerializeField] float _time;
    float _currentTime = 0;

    public void SetTime(float time)
    {
        _time = time;
        _currentTime = _time;
    }

    public override bool CheckCondition()
    {
        _currentTime -= Time.deltaTime;
        
        if(_currentTime <= 0)
        {
            _currentTime = _time;
            return true;
        }

        return false;
    }
}