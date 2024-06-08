using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] BehaviourPerformer[] behaviourPerformer;
    [SerializeField] UnityEvent onUse;
    public UnityEvent OnUse { get => onUse; set => onUse = value; }

    public void Use()
    {
        BehaviourPerformer.Perform(behaviourPerformer);
        OnUse.Invoke();
    }
}