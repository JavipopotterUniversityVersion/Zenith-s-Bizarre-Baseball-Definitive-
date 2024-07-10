using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotOnAwake : MonoBehaviour
{
    [SerializeField] Vector2 _range;

    private void Awake() => SetRandomRotation();

    void SetRandomRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(_range.x, _range.y));
    }
}
