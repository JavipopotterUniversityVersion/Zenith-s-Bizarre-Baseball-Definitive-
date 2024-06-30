using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

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

    public Transform GetClosestSearchable(Vector3 position, Identifiable searchableType)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Searchable searchable in searchables)
        {
            if (searchable.IdentifiableType.DerivesFrom(searchableType))
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

    public Transform GetClosestSearchable(Transform transform, Identifiable searchableType)
    {
        return GetClosestSearchable(transform.position, searchableType);
    }

    public T[] GetAllSearchables<T>(Identifiable searchableType)
    {
        return searchables.Where(searchable => searchable.IdentifiableType == searchableType).Select(searchable => searchable.GetComponent<T>()).ToArray();
    }
}
