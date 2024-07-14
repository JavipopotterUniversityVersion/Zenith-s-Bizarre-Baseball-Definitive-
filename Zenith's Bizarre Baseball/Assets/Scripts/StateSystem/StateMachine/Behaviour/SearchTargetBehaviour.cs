using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SearchTargetBehaviour : MonoBehaviour, IBehaviour, ICondition
{
    [SerializeField] bool _found = false;
    TargetHandler _targetHandler;
    [SerializeField] Identifiable _targetSearchableType;
    [SerializeField] int _searchableIndex = 0;
    [SerializeField] UnityEvent<Transform> _onTargetFound = new UnityEvent<Transform>();
    [SerializeField] bool _setTargetAsTarget = true;

    private void Awake() {
        _targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void ExecuteBehaviour()
    {
        if (_found) return;

        Transform target = SearchManager.Instance.GetClosestSearchable(transform.position, _targetSearchableType, _searchableIndex);

        _onTargetFound.Invoke(target);

        if(_setTargetAsTarget) _targetHandler.SetTarget(target);
        _found = true;
    }

    public bool CheckCondition() => _found;

    public void SetTargetSearchable(Identifiable type)
    {
        _targetSearchableType = type;
        _found = false;
    }

    private void OnValidate() {
        gameObject.name = $"SearchTarget";
    }
}
