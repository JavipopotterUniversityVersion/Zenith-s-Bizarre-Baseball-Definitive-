using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinkerGadget : MonoBehaviour
{
    List<HealthHandler> _linkedTargets = new List<HealthHandler>();
    LineRenderer _lineRenderer;

    [SerializeField] Identifiable _targetLinkers;
    [SerializeField] Identifiable _targetLinked;

    private void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetLinkers()
    {
        var linkers = SearchManager.Instance.GetAllSearchables<Transform>(_targetLinkers);

        foreach (var linker in linkers)
        {
            var comp = linker.AddComponent<LinkerComponent>();
            comp.SetLinkerGadget(this);
            comp.SetTarget(_targetLinked);
            linker.GetComponent<DamageCollidable>().SetDamage("0");

            if(linker.GetComponentInChildren<TrailRenderer>() is TrailRenderer trailRenderer)
            {
                trailRenderer.colorGradient = _lineRenderer.colorGradient;
            }
        }
    }

    public void AddLinkedTarget(HealthHandler target)
    {
        if(!_linkedTargets.Contains(target))
        {
            _linkedTargets.Add(target);
            target.OnDamageTaken.AddListener(OnGetDamage);
            target.OnDie.AddListener(() => UnlinkTarget(target));
            _lineRenderer.positionCount = _linkedTargets.Count;
        }
    }

    public void OnGetDamage(HealthHandler source, float damage)
    {
        var tempLinkedTargets = new List<HealthHandler>(_linkedTargets);
        foreach (var target in tempLinkedTargets)
        {
            if(target != source) target.TakeDamage(damage);
        }
    }

    void UnlinkTarget(HealthHandler target)
    {
        _linkedTargets.Remove(target);
        _lineRenderer.positionCount = _linkedTargets.Count;
    }

    private void Update() {
        for (int i = 0; i < _linkedTargets.Count; i++)
        {
            _lineRenderer.SetPosition(i, _linkedTargets[i].transform.position);
        }
    }
}
