using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthHandler : MonoBehaviour
{
    UnityEvent<float> onHealthChanged = new UnityEvent<float>();
    public UnityEvent<float> OnHealthChanged => onHealthChanged;

    [SerializeField] UnityEvent onGetDamage = new UnityEvent();
    public UnityEvent OnGetDamage => onGetDamage;

    [SerializeField] UnityEvent onDie = new UnityEvent();
    public UnityEvent OnDie => onDie;

    [SerializeField] Float maxHealth;
    [SerializeField] float _currentHealth = 12;
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth.Value);
            onHealthChanged.Invoke(_currentHealth);

            if(_currentHealth == 0) onDie.Invoke();
        }
    }

    private void Awake() => ResetHealth();
    public void ResetHealth() => CurrentHealth = maxHealth.Value;

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        onGetDamage.Invoke();
    }
}
