using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyIdentifiablesInArea : MonoBehaviour, ICondition
{
    [SerializeField] OverlapBoxCondition _overlapBoxCondition;
    [SerializeField] Identifiable _targetIdentifiable;
    Transform _lastObj;

    private void Awake() {
        _overlapBoxCondition.OnOverlap.AddListener(OnOverlap);
    }

    private void OnDestroy() {
        _overlapBoxCondition.OnOverlap.RemoveListener(OnOverlap);
    }

    public bool CheckCondition()
    {
        return _lastObj != null;
    }

    void OnOverlap(Transform obj)
    {
        if(obj.TryGetComponent(out Searchable searchable) 
        && (searchable.IdentifiableType.DerivesFrom(_targetIdentifiable)
        || searchable.IdentifiableType == _targetIdentifiable))
        {
            _lastObj = obj;
        }
    }
}
