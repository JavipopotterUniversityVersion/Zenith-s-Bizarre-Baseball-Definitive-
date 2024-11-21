using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LowHealthEffectHandler : MonoBehaviour
{
    [SerializeField] VolumeProfile _volume;
    [SerializeField] float _lowHealthThreshold = 5;
    [SerializeField] float _lowHealthIntensity = 0.5f;
    [SerializeField] Float _currentHealth;
    [SerializeField] float _vignetteLapse = 0.5f;
    Vignette _vignette;
    float _vignetteTimer = 0;
    bool _flag = false;
    bool _active = false;
    [SerializeField] Bool _aliveFlag;

    private void Awake() 
    {
        if(_volume.TryGet(out Vignette vignette))
        {
            _vignette = vignette;
        }
        else
        {
            Debug.LogError("Vignette not found in volume profile");
        }

        _currentHealth.OnValueChanged.AddListener(CheckHealth);
        _aliveFlag.OnValueChanged.AddListener(OnDeath);
    }

    private void OnDestroy()
    {
        _currentHealth.OnValueChanged.RemoveListener(CheckHealth);
        _aliveFlag.OnValueChanged.RemoveListener(OnDeath);
        _vignette.intensity.value = 0;
    }

    private void Update() 
    {
        if(_active)
        {
            if(_flag)
            {
                _vignette.intensity.value = Mathf.Lerp(0, _lowHealthIntensity, _vignetteTimer);
                _vignetteTimer -= Time.deltaTime;

                if(_vignetteTimer <= 0) _flag = false;
            }
            else
            {
                _vignette.intensity.value = Mathf.Lerp(_lowHealthIntensity, 0, _vignetteTimer);
                _vignetteTimer += Time.deltaTime;

                if(_vignetteTimer >= _vignetteLapse) _flag = true;
            }
        }
    }

    void CheckHealth()
    {
        if(_currentHealth.Value <= _lowHealthThreshold && _aliveFlag.Value) _active = true;
        else
        {
            _active = false;
            _vignette.intensity.value = 0;
        }
    }

    void OnDeath()
    {
        if(!_aliveFlag.Value)
        {
            _active = false;
            _vignette.intensity.value = 0;
        }
    }
}