using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthHandler))]
public class InmunityHandler : MonoBehaviour
{
    HealthHandler healthHandler;
    [SerializeField] float inmunityTime = 1;

    private void Awake()
    {
        healthHandler = GetComponent<HealthHandler>();
        healthHandler.OnGetDamage.AddListener(SetInmunity);
    }

    public void SetInmunity() => StartCoroutine(Inmunity());

    IEnumerator Inmunity()
    {
        healthHandler.SetInmunity(true);
        yield return new WaitForSecondsRealtime(inmunityTime);
        healthHandler.SetInmunity(false);
    }

    private void OnDestroy() {
        healthHandler.OnGetDamage.RemoveListener(SetInmunity);
    }
}
