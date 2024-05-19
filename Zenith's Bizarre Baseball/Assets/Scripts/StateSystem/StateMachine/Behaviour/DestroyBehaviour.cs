using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBehaviour : MonoBehaviour, IBehaviour
{
    public void ExecuteBehaviour()
    {
       
        Destroy(GetComponentInParent<StateHandler>().transform.parent.gameObject);
    }
    private void OnValidate()
    {
        name = "Destroy this gameobject";
    }
}
