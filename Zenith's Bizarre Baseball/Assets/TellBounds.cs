using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TellBounds : MonoBehaviour
{
    private void Update() {
        Debug.Log($"Bounds: {GetComponent<Collider2D>().bounds}");
    }
}
