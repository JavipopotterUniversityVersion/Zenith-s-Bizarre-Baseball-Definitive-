using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageAllSearchablesBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private float damage = 1;
    [SerializeField] private SearchableType searchableType;
    List<HealthHandler> healthHandlers;

    private void Awake() 
    {
        healthHandlers = SearchManager.Instance.GetAllSearchables<HealthHandler>(searchableType).ToList();
    }

    public void ExecuteBehaviour() => healthHandlers.ForEach(h => h.TakeDamage(damage));
    public void SetDamage(float damage) => this.damage = damage;
}
