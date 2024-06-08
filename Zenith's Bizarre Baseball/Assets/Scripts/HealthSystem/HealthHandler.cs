using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthHandler : Readable
{
    [SerializeField] UnityEvent<float> onHealthChanged = new UnityEvent<float>();
    public UnityEvent<float> OnHealthChanged => onHealthChanged;

    [SerializeField] Condition[] getDamageConditions;
    [SerializeField] UnityEvent onGetDamage = new UnityEvent();
    public UnityEvent OnGetDamage => onGetDamage;

    [SerializeField] UnityEvent onDie = new UnityEvent();
    public UnityEvent OnDie => onDie;

    [SerializeField] Float maxHealth;
    [SerializeField] Float currentHealthReference;
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

    public override float Read() => CurrentHealth;

    private void Awake()
    {
        Condition.InitializeAll(getDamageConditions);
        if(currentHealthReference != null)
        {
            _currentHealth = currentHealthReference.Value;
            currentHealthReference.OnValueChanged.AddListener(GetRef);
            SetRef();
        }
    }

    void GetRef() => _currentHealth = CurrentHealth;
    void SetRef() => onHealthChanged.AddListener(currentHealthReference.SetRawValue);

    public void ResetHealth() => CurrentHealth = maxHealth.Value;

    public void TakeDamage(float damage)
    {
        if(Condition.CheckAllConditions(getDamageConditions))
        {
            CurrentHealth -= damage;
            onGetDamage.Invoke();
        }
    }
}
