using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] float angle = 90;
    [SerializeField] Transform target;

    public void ExecuteBehaviour()
    {
        target.transform.eulerAngles += new Vector3(0, 0, angle);
    }

    public void SetAngle(float angle) => this.angle = angle;
    private void OnValidate() 
    {
        name = "Rotate " + target.name + " " + angle + "°";
    }
}
