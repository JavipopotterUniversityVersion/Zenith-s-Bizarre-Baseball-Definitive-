using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DuplicateCollidable))]
public class DuplicatorGadget : MonoBehaviour
{
    DuplicateCollidable _duplicateCollidable;
    [SerializeField] Identifiable _target;

    private void Awake() {
        _duplicateCollidable = GetComponent<DuplicateCollidable>();
    }

    public void DuplicateTargets()
    {
        var targets = SearchManager.Instance.GetAllSearchables<Transform>(_target);
        if(targets.Length >= 15) return;

        foreach (var target in targets)
        {
            _duplicateCollidable.Process(target.gameObject);
        }
    }
}
