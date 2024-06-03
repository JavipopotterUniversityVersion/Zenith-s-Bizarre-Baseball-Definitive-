using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searchable : MonoBehaviour
{
    [SerializeField] private SearchableType searchableType;
    public SearchableType SearchableType => searchableType;

    private void Awake() {
        if(SearchManager.Instance == null) Instantiate(new GameObject ("SearchManager")).AddComponent<SearchManager>();
    }

    private void OnEnable()
    {
        SearchManager.Instance.RegisterSearchable(this);
    }

    private void OnDisable()
    {
        SearchManager.Instance.UnregisterSearchable(this);
    }

    public Transform GetClosestSearchable(SearchableType searchableType)
    {
        return SearchManager.Instance.GetClosestSearchable(transform, searchableType);
    }
}

public enum SearchableType
{
    Player,
    Enemy,
    ball
}