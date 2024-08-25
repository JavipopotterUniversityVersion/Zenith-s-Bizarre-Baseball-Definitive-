using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeInTime : MonoBehaviour
{
    [SerializeField] Curve _fadeCurve;
    [SerializeField] ObjectProcessor _fadeDuration;
    SpriteRenderer sr;
    float _fadeTime = 0;

    [SerializeField] Gradient _gradient;

    [SerializeField] UnityEvent onFadeComplete;
    bool _isFading = true;

    [SerializeField] bool _loop;


    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        _fadeTime += Time.deltaTime;
        float t = _fadeTime / _fadeDuration.Result();
        float curveValue = _fadeCurve.Value.Evaluate(t);
        sr.color = _gradient.Evaluate(curveValue);

        if(t >= 1 && _isFading)
        {
            _isFading = false;
            onFadeComplete.Invoke();

            if(_loop)
            {
                _fadeTime = 0;
                _isFading = true;
            }
        }
    }

    public void Reset()
    {
        _fadeTime = 0;
        _isFading = true;
    }
}
