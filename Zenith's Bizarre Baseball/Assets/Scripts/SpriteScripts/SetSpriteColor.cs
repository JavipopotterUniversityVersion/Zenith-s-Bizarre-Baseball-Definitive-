using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteColor : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] SerializableDictionary<string, Color> _colorDictionary;
    [SerializeField] float _lerpDuration = 0.5f;

    public void SetColor(string colorName)
    {
        if(_colorDictionary.ContainsKey(colorName)) StartCoroutine(SetColorRoutine(colorName));
        else Debug.LogWarning($"Color {colorName} not found in dictionary.");
    }

    IEnumerator SetColorRoutine(string colorName)
    {
        Color startColor = _spriteRenderer.color;
        Color endColor = _colorDictionary[colorName];
        float elapsedTime = 0;

        while(elapsedTime < _lerpDuration)
        {
            _spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / _lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _spriteRenderer.color = endColor;
    }

    public void SetRenderer(SpriteRenderer spriteRenderer) => _spriteRenderer = spriteRenderer;
}
