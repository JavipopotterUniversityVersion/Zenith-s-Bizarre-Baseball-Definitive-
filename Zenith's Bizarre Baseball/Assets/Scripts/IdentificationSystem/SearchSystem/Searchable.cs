using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Searchable : MonoBehaviour
{
    [SerializeField] private Identifiable identifiableType;
    public Identifiable IdentifiableType => identifiableType;

    [Space(20)] [Header("Events")]
    [SerializeField] IdentifiableChangeEvent[] identifiableChangeEvents;

    public void SetIdentifiableType(Identifiable identifiable)
    {
        identifiableType = identifiable;

        foreach (IdentifiableChangeEvent identifiableChangeEvent in identifiableChangeEvents)
        {
            identifiableChangeEvent.Invoke(identifiable);
        }
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

    [Serializable] 
    class IdentifiableChangeEvent
    {
        [SerializeField] Identifiable _identifiableToCompare;
        public Identifiable Identifiable => _identifiableToCompare;
        public ChangeType changeType;
        [SerializeField] UnityEvent _onChange = new UnityEvent();
        public UnityEvent OnChange => _onChange;

        public void Invoke(Identifiable identifiable)
        {
            bool result = false;
            switch (changeType)
            {
                case ChangeType.DerivesFrom:
                    result = _identifiableToCompare.DerivesFrom(identifiable);
                    break;
                case ChangeType.DerivesTo:
                    result = _identifiableToCompare.DerivesTo(identifiable);
                    break;
                case ChangeType.Is:
                    result = _identifiableToCompare == identifiable;
                    break;
                case ChangeType.IsNot:
                    result = _identifiableToCompare != identifiable;
                    break;
            }

            if(result) 
            {
                OnChange.Invoke();
            }
        }
    }
}

public enum ChangeType
{
    DerivesFrom, DerivesTo, Is, IsNot
}