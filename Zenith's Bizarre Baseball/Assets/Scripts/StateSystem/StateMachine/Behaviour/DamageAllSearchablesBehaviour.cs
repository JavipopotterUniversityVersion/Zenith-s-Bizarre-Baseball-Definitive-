using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageAllSearchablesBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private ObjectProcessor damage;
    [SerializeField] private Identifiable searchableType;
    List<HealthHandler> healthHandlers;
    [SerializeField] private bool ignoreConditions;

    public void ExecuteBehaviour()
    {
        healthHandlers = SearchManager.Instance.GetAllSearchables<HealthHandler>(searchableType).ToList();
        healthHandlers.ForEach(h => h.TakeDamage(damage.Result(), ignoreConditions));
    }
}
