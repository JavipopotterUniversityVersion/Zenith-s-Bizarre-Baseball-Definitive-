using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCurveInfo", menuName = "FeedBack/AnimationCurveInfo", order = 1)]
public class TimeModifier : ScriptableObject
{
    [SerializeField] CameraEffects cameraEffects;
    [SerializeField] AnimationCurve curve;

    public void PlayTimeModifier(float duration) => cameraEffects.DramaticHitStop(curve, duration);
}
