using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorAdder : MonoBehaviour
{
    [SerializeField] Identifiable identifiable;
    Transform target;

    private void Awake() {
        target = GetComponentInParent<Searchable>().transform;
    }

    public void AddRotator() {
        var targets = SearchManager.Instance.GetAllSearchables<Transform>(identifiable);

        foreach (Transform target in targets) 
        {
            print(target);
            if(target.TryGetComponent<RotatorAroundObject>(out var rotator))
            {
                rotator.SetTarget(this.target);
            }
            else
            {
                var rotatorAroundObject = target.gameObject.AddComponent<RotatorAroundObject>();
                rotatorAroundObject.SetTarget(this.target);
            }
        }
    }
}
