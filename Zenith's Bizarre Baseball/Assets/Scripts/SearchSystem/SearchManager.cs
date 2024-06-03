using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SearchManager : MonoBehaviour
{
    static SearchManager _instance;
    public static SearchManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    
    }

    private List<Searchable> searchables = new List<Searchable>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void RegisterSearchable(Searchable searchable)
    {
        searchables.Add(searchable);
    }

    public void UnregisterSearchable(Searchable searchable)
    {
        searchables.Remove(searchable);
    }

    public Transform GetClosestSearchable(Vector3 position, SearchableType searchableType)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Searchable searchable in searchables)
        {
            if (searchable.SearchableType == searchableType)
            {
                float distance = Vector3.Distance(position, searchable.transform.position);
                if (distance < closestDistance)
                {
                    closest = searchable.transform;
                    closestDistance = distance;
                }
            }
        }

        return closest;
    }

    public Transform GetClosestSearchable(Transform transform, SearchableType searchableType)
    {
        return GetClosestSearchable(transform.position, searchableType);
    }
}
