using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] float _rotation;

    private void OnDrawGizmos() {
        transform.rotation = Quaternion.Euler(0, 0, _rotation);
    }
}
