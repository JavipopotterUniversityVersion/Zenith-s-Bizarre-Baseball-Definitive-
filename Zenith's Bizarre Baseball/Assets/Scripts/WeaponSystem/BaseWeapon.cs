using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] UnityEvent onUse;
    public UnityEvent OnUse { get => onUse; set => onUse = value; }

    [SerializeField] UnityEvent onHit;
    public UnityEvent OnHit { get => onHit; set => onHit = value; }

    public void Use()
    {
        OnUse.Invoke();
    }
}