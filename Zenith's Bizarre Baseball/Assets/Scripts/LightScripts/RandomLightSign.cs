using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RandomLightSign : MonoBehaviour
{
    SpriteRenderer _sr;
    Light2D _light2D;
    [SerializeField] LightSign[] _lightSigns;

    private void Awake() 
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _light2D = GetComponentInChildren<Light2D>();
    }

    private void Start() 
    {
        int randomIndex = UnityEngine.Random.Range(0, _lightSigns.Length);
        _sr.sprite = _lightSigns[randomIndex].sprite;
        _light2D.intensity = _lightSigns[randomIndex].intensity;
        _light2D.color = _lightSigns[randomIndex].color;
    }

    [Serializable]
    public class LightSign
    {
        public UnityEngine.Sprite sprite;
        public Color color = Color.white;
        public float intensity = 1;
    }
}
