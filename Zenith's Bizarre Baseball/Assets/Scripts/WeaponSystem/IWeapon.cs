using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeapon
{
    UnityEvent OnUse { get; set;}
    public void Use();
}
