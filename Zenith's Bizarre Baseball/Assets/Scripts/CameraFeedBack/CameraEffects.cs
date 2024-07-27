using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CameraEffects", menuName = "FeedBack/CameraEffects", order = 1)]
public class CameraEffects : ScriptableObject
{
    UnityEvent<float> shakeEvent = new UnityEvent<float>();
    public UnityEvent<float> ShakeEvent => shakeEvent;

    UnityEvent<float> onHitStop = new UnityEvent<float>();
    public UnityEvent<float> OnHitStop => onHitStop;

    UnityEvent<AnimationCurve, float> onDramaticHitStop = new UnityEvent<AnimationCurve, float>();
    public UnityEvent<AnimationCurve, float> OnDramaticHitStop => onDramaticHitStop;
    
    public void Shake(float shakeValue) => shakeEvent.Invoke(shakeValue);
    public void HitStop(float hitStopValue) => onHitStop.Invoke(hitStopValue);
    public void DramaticHitStop(AnimationCurve curve, float duration) => onDramaticHitStop.Invoke(curve, duration);
    public void SetTimeScale(float timeScale) => Time.timeScale = timeScale;
}
