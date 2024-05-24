using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnValueChangePerformer : MonoBehaviour
{
    [SerializeField] TrueCondition value;
    [SerializeField] BehaviourPerformer[] performers;

    private void OnEnable()
    {
        value.OnValueChange.AddListener(Perform);
    }

    private void OnDisable()
    {
        value.OnValueChange.RemoveListener(Perform);
    }

    void Perform()
    {
        foreach (BehaviourPerformer performer in performers)
        {
            performer.Perform();
        }
    }
}
