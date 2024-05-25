using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthHandler))]
public class InmunityHandler : MonoBehaviour
{
    HealthHandler healthHandler;
    [SerializeField] Float inmunityTime;
    LayerMask _originalLayer;

    private void Awake()
    {
        _originalLayer = gameObject.layer;
        healthHandler = GetComponent<HealthHandler>();
        healthHandler.OnGetDamage.AddListener(SetInmunity);
    }

    public void SetInmunity() => StartCoroutine(Inmunity());

    IEnumerator Inmunity()
    {
        gameObject.layer = LayerMask.NameToLayer("Inmune");
        yield return new WaitForSecondsRealtime(inmunityTime.Value);
        gameObject.layer = _originalLayer;
    }

    private void OnDestroy() {
        healthHandler.OnGetDamage.RemoveListener(SetInmunity);
    }
}
