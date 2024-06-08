using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    [SerializeField] Float _lifeTime;

    private void Start() => Destroy(gameObject, _lifeTime.Value);
}
