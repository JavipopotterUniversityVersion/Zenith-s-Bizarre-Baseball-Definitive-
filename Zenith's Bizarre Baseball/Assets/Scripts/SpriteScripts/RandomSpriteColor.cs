using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteColor : MonoBehaviour
{
    [SerializeField] Gradient _gradient;
    [SerializeField] SpriteRenderer[] _spriteRenderers;

    private void Awake() => SetRandomColor();

    void SetRandomColor()
    {
        Color color = _gradient.Evaluate(Random.Range(0f, 1f));
        
        foreach(SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = color;
        }
    }
}
