using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionOnAwake : MonoBehaviour
{
    [SerializeField] Vector2 _range;
    private void Awake() => SetRandomPosition();
    void SetRandomPosition()
    {
        transform.position += new Vector3(Random.Range(-_range.x, _range.x), Random.Range(-_range.y, _range.y), 0);
    }
}
