using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCondition : MonoBehaviour, ICondition
{
    [SerializeField] float healthThreshold;
    HealthHandler health;

    private void Start()
    {
        health = GetComponentInParent<HealthHandler>();
    }

    public bool CheckCondition() => health.CurrentHealth <= healthThreshold;

    private void OnValidate() {
        name = $"Health <= {healthThreshold}";
    }
}
