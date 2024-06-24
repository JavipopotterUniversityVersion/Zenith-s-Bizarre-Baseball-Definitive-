using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimationTimeCondition : MonoBehaviour, ICondition
{
    [SerializeField] AnimationClip animationClip;
    float timePassed;

    public bool CheckCondition()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= animationClip.length)
        {
            timePassed = 0;
            return true;
        }
        return false;
    }

    private void OnValidate() 
    {
        if(animationClip != null) gameObject.name = $"Wait {animationClip.length * 60/ 100}s (Animation)";
    }
}
