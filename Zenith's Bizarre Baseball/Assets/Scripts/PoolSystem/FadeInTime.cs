using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTime : MonoBehaviour
{
    [SerializeField] Curve _fadeCurve;
    [SerializeField] ObjectProcessor _fadeDuration;
    SpriteRenderer sr;
    float _fadeTime = 0;

    [SerializeField] Gradient _gradient;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        _fadeTime += Time.deltaTime;
        float t = _fadeTime / _fadeDuration.Result();
        float curveValue = _fadeCurve.Value.Evaluate(t);
        sr.color = _gradient.Evaluate(curveValue);
    }
}
