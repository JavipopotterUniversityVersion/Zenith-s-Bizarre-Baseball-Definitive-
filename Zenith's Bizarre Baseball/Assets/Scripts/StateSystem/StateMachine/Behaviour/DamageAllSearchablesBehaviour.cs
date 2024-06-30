using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageAllSearchablesBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private ObjectProcessor damage;
    [SerializeField] private Identifiable searchableType;
    List<HealthHandler> healthHandlers;

    private void Awake() 
    {
        healthHandlers = SearchManager.Instance.GetAllSearchables<HealthHandler>(searchableType).ToList();
    }

    public void ExecuteBehaviour() => healthHandlers.ForEach(h => h.TakeDamage(damage.Result()));
}
