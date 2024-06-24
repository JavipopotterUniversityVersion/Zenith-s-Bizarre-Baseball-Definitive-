using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SearchTargetBehaviour : MonoBehaviour, IBehaviour, ICondition
{
    [SerializeField] bool _found = false;
    TargetHandler _targetHandler;
    [SerializeField] SearchableType _targetSearchableType;
    [SerializeField] UnityEvent<Transform> _onTargetFound = new UnityEvent<Transform>();

    private void Awake() {
        _targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void ExecuteBehaviour()
    {
        if (_found) return;

        Transform target = SearchManager.Instance.GetClosestSearchable(transform.position, _targetSearchableType);
        _onTargetFound.Invoke(target);

        _targetHandler.SetTarget(target);
        _found = true;
    }

    public bool CheckCondition() => _found;

    public void SetTargetSearchable(string type)
    {
        _targetSearchableType = (SearchableType) System.Enum.Parse(typeof(SearchableType), type);
        _found = false;
    }

    private void OnValidate() {
        gameObject.name = $"SearchTarget";
    }
}
