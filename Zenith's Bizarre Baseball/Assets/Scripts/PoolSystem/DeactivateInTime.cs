using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateInTime : MonoBehaviour
{
    [SerializeField] float timeToDeactivate;
    float timePassed;

    private void OnEnable()
    {
        timePassed = 0;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= timeToDeactivate)
        {
            gameObject.SetActive(false);
        }
    }
}
