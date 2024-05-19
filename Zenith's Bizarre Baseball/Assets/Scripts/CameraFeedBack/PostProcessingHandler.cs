using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingHandler : MonoBehaviour
{
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] float intensity = 1;

    public void PlayAberration(float duration) => StartCoroutine(ExecuteAberration(duration));
    IEnumerator ExecuteAberration(float duration)
    {
        volumeProfile.TryGet(out UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration);
        chromaticAberration.intensity.Override(intensity);
        yield return new WaitForSeconds(duration);
        chromaticAberration.intensity.Override(0);
    }

    public void PlayFilmGrain(float duration) => StartCoroutine(ExecuteFilmGrain(duration));
    IEnumerator ExecuteFilmGrain(float duration)
    {
        volumeProfile.TryGet(out UnityEngine.Rendering.Universal.FilmGrain filmGrain);
        filmGrain.active = true;
        yield return new WaitForSeconds(duration);
        filmGrain.active = false;
    }

    public void PlayColorAdjustments(float duration) => StartCoroutine(ExecuteColorAdjustments(duration));
    IEnumerator ExecuteColorAdjustments(float duration)
    {
        volumeProfile.TryGet(out UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments);
        colorAdjustments.active = true;
        yield return new WaitForSeconds(duration);
        colorAdjustments.active = false;
    }

    private void OnDestroy() => Reset();

    private void Awake() => Reset();

    private void Reset()
    {
        volumeProfile.TryGet(out UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration);
        chromaticAberration.intensity.Override(0);
        volumeProfile.TryGet(out UnityEngine.Rendering.Universal.FilmGrain filmGrain);
        filmGrain.active = false;
        volumeProfile.TryGet(out UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments);
        colorAdjustments.active = false;
    }
}
