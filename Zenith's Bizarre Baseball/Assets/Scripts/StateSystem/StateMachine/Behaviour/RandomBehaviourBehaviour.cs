using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBehaviourBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject[] behavioursContainers;
    IBehaviour[] behaviours;

    private void Awake()
    {
        behaviours = new IBehaviour[behavioursContainers.Length];
        for (int i = 0; i < behavioursContainers.Length; i++)
        {
            behaviours[i] = behavioursContainers[i].GetComponent<IBehaviour>();
        }
    }

    public void ExecuteBehaviour() => behaviours[Random.Range(0, behaviours.Length)].ExecuteBehaviour();

    private void OnValidate() 
    {
        name = "RandomBehaviour";
    }
}
