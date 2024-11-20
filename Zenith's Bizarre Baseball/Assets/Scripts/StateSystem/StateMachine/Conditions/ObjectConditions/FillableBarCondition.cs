using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class FillableBarCondition : MonoBehaviour, ICondition
{
    [SerializeField] float _maxScale;
    [SerializeField] Gradient _fillGradient;
    [SerializeField] ObjectProcessor _fillSpeed;
    [SerializeField] [Range(0,1)] float _threshold;
    [SerializeField] UnityEvent _onTryBatWhileExhausted;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CheckCondition()
    {
        bool value = transform.localScale.x > (_threshold * _maxScale);
        if(!value) _onTryBatWhileExhausted.Invoke();
        return value;
    }

    public void Fill() => SetScale(_maxScale);

    public void SubstractPercent(float percent)
    {
        float newScale = transform.localScale.x - (_maxScale * percent);
        SetScale(newScale);
    }

    private void Update() 
    {
        float scaleToAdd = _fillSpeed.Result() * Time.deltaTime;
        float newScale = transform.localScale.x + scaleToAdd;
        SetScale(newScale);
    }

    void SetScale(float newScale)
    {
        spriteRenderer.color = _fillGradient.Evaluate(newScale/_maxScale);
        newScale = Mathf.Clamp(newScale, 0, _maxScale);

        transform.localScale = new Vector3(newScale, transform.localScale.y, transform.localScale.z);
    }
}
