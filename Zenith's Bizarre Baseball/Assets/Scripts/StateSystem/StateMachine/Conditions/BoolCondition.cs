using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolCondition : MonoBehaviour, ICondition
{
    [SerializeField] Bool condition;

    public bool CheckCondition() => condition.Value;

    private void OnValidate() {
        name = condition.name + " -> " + condition.Value.ToString();
    }
}