using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourCollidable : ICollidable
{
    [SerializeField] GameObject[] behaviourContainers;
    IBehaviour[] behaviours;

    private void Awake() {
        behaviours = new IBehaviour[behaviourContainers.Length];
        for(int i = 0; i < behaviourContainers.Length; i++)
        {
            behaviours[i] = behaviourContainers[i].GetComponent<IBehaviour>();
        }
    }

    public override void OnCollide(Collider2D collision)
    {
        foreach(IBehaviour behaviour in behaviours)
        {
            behaviour.ExecuteBehaviour();
        }
    }
}
