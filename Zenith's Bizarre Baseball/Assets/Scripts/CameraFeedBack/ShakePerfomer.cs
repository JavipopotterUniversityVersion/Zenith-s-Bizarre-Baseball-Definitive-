using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ShakePerfomer : MonoBehaviour
{
    [SerializeField] CameraEffects cameraEffects;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    [SerializeField] Float shakeTime;

    private void Awake()
    {
        cinemachineBasicMultiChannelPerlin = 
        GetComponent<CinemachineVirtualCamera>().
        GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        SubscribeToCameraEffects();
    }

    void SubscribeToCameraEffects() => cameraEffects.ShakeEvent.AddListener(Shake);
    void UnsubscribeToCameraEffects() => cameraEffects.ShakeEvent.RemoveListener(Shake);

    void Shake(float shakeValue) => StartCoroutine(ShakeRoutine(shakeValue));

    IEnumerator ShakeRoutine(float shakeValue)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeValue;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = shakeValue;

        yield return new WaitForSecondsRealtime(shakeTime.Value);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }
        
    private void OnDestroy() => UnsubscribeToCameraEffects();
}
