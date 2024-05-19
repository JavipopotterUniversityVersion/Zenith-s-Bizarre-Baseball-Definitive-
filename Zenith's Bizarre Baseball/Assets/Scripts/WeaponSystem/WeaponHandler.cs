using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] GameObject selectedWeaponContainer;
    [SerializeField] UnityEvent onUse;
    IWeapon selectedWeapon;

    private void Awake() => selectedWeapon = selectedWeaponContainer.GetComponent<IWeapon>();
    
    public void Use()
    {
        selectedWeapon.Use();
        onUse.Invoke();
    }
}
