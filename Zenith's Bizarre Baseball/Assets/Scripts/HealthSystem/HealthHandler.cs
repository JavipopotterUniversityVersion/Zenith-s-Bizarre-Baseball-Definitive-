using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthHandler : MonoBehaviour, IReadable
{
    [SerializeField] UnityEvent<float> onHealthChanged = new UnityEvent<float>();
    public UnityEvent<float> OnHealthChanged => onHealthChanged;

    [SerializeField] Condition[] getDamageConditions;
    [SerializeField] UnityEvent onFailedDamage;
    public UnityEvent OnFailedDamage => onFailedDamage;

    [SerializeField] UnityEvent onGetDamage = new UnityEvent();
    public UnityEvent OnGetDamage => onGetDamage;

    [SerializeField] UnityEvent<HealthHandler, float> onDamageTaken = new UnityEvent<HealthHandler, float>();
    public UnityEvent<HealthHandler, float> OnDamageTaken => onDamageTaken;

    [SerializeField] UnityEvent onHeal = new UnityEvent();
    public UnityEvent OnHeal => onHeal;

    [SerializeField] UnityEvent onDie = new UnityEvent();
    public UnityEvent OnDie => onDie;

    [SerializeField] ObjectProcessor _maxHealth;
    float _currentHealth = 12;
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth.Result());
            onHealthChanged.Invoke(_currentHealth);

            if(_currentHealth == 0) onDie.Invoke();
        }
    }

    public float Read() => CurrentHealth;

    private void Awake()
    {
        ResetHealth();
        Condition.InitializeAll(getDamageConditions);
    }

    public void ResetHealth() => CurrentHealth = _maxHealth.Result();
    public void TakeDamage(float damage, bool ignoreConditions = false)
    {
        if(ignoreConditions || Condition.CheckAllConditions(getDamageConditions))
        {
            CurrentHealth -= damage;
            onGetDamage.Invoke();
            onDamageTaken.Invoke(this, damage);
        }
        else
        {
            onFailedDamage.Invoke();
        }
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        onHeal.Invoke();
    }

    public void SetHealthRaw(float health) => _currentHealth = health;
    public float GetMaxHealth() => _maxHealth.Result();
}
