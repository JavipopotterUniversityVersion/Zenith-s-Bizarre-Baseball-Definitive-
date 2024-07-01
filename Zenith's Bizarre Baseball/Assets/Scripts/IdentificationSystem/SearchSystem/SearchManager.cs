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

    public Transform GetClosestSearchable(Vector3 position, Identifiable searchableType, int index = 0)
    {
        Transform closest = null;

        List<Searchable> searchablesofType = searchables.Where(searchable => searchable.IdentifiableType.DerivesFrom(searchableType)).ToList();

        searchablesofType.Sort((a, b) => Vector3.Distance(a.transform.position, position).CompareTo(Vector3.Distance(b.transform.position, position)));

        if(index >= searchablesofType.Count) closest = searchablesofType[searchablesofType.Count - 1].transform;
        else closest = searchablesofType[index].transform;

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
