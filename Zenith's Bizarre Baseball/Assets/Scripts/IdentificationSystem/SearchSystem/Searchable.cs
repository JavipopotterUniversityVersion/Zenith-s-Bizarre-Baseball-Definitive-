using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searchable : MonoBehaviour
{
    [SerializeField] private Identifiable identifiableType;
    public Identifiable IdentifiableType => identifiableType;

    public void SetIdentifiableType(Identifiable identifiable)
    {
        identifiableType = identifiable;
    }

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

    public Transform GetClosestSearchable(Identifiable identifiable)
    {
        return SearchManager.Instance.GetClosestSearchable(transform, identifiable);
    }
}