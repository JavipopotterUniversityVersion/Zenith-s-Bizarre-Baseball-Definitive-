using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class InstanceExplosion : ICollidable, IBehaviour
{
    [SerializeField] InstanceWithProbability[] instances;
    [SerializeField] Vector2 _angleRange;
    [SerializeField] Vector2 _force;

    public override void OnCollide(Collider2D collision) => Explode();

    public void ExecuteBehaviour() => Explode();

    public void Explode()
    {
        foreach (InstanceWithProbability instance in instances)
        {
            if(UnityEngine.Random.value <= instance.Probability)
            {
                for (int i = 0; i < instance.Number; i++)
                {
                    if(Instantiate(instance.Prefab, transform.position, Quaternion.identity).TryGetComponent(out Rigidbody2D rb))
                    {
                        float angle = UnityEngine.Random.Range(_angleRange.x, _angleRange.y) * Mathf.Deg2Rad;
                        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * UnityEngine.Random.Range(_force.x, _force.y);
                    }
                }
            }
        }
    }
}

[Serializable]
public class InstanceWithProbability
{
    public GameObject Prefab;
    public Vector2Int numberRange;
    public int Number => UnityEngine.Random.Range(numberRange.x, numberRange.y);
    [Range(0,1)] public float Probability;
}