using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.U2D;
using Unity.VisualScripting;

public class InstanceExplosion : ICollidable, IBehaviour
{
    [SerializeField] InstanceWithProbability[] instances;
    [SerializeField] Vector2 _angleRange;
    [SerializeField] Vector2 _force;
    [SerializeField] Vector2 _angularForce;

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
                    GameObject obj = Instantiate(instance.Prefab, transform.position, Quaternion.identity);
                    instance.Processor.ToList().ForEach(p => p.I.Process(obj));

                    if(obj.TryGetComponent(out Rigidbody2D rb))
                    {
                        float angle = UnityEngine.Random.Range(_angleRange.x, _angleRange.y) * Mathf.Deg2Rad;
                        rb.velocity = (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized
                        + (Vector2)transform.up.normalized).normalized * UnityEngine.Random.Range(_force.x, _force.y);
                        rb.angularVelocity = UnityEngine.Random.Range(_angularForce.x, _angularForce.y);
                    }
                }
            }
        }
    }

    private void OnValidate() {
        name = "Instance Explosion of " + instances[0].Prefab.name;
    }
}

[Serializable]
public class InstanceWithProbability
{
    public GameObject Prefab;
    public Vector2Int numberRange;
    public int Number => UnityEngine.Random.Range(numberRange.x, numberRange.y);
    [Range(0,1)] public float Probability;

    [SerializeField] IRef<IGameObjectProcessor>[] processor;
    public IRef<IGameObjectProcessor>[] Processor => processor;
}

[Serializable]
public struct ConditionalGameObjectProcessor
{
    public Condition[] conditions;
    public IRef<IGameObjectProcessor>[] processors;
}

[Serializable]
public class IRef<T> : ISerializationCallbackReceiver where T : class
{
    public UnityEngine.Object target;
    public T I { get => target as T; }
    public static implicit operator bool(IRef<T> ir) => ir.target != null;
    void OnValidate()
    {
        if (!(target is T)) 
        {
            if (target is GameObject go)
            {
                target = null;
                foreach (Component c in go.GetComponents<Component>())
                { 
                    if (c is T){
                        target = c;
                        break;
                    }
                }
            }
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() => OnValidate();
    void ISerializationCallbackReceiver.OnAfterDeserialize() { }

    public static T[] ToArray(IRef<T>[] refs) => refs.Select(r => r.I).ToArray();

    public static IRef<T>[] ToRef(T[] ts) => ts.Select(t => new IRef<T> { target = t as UnityEngine.Object }).ToArray();
    public static IRef<T> ToRef(T t) => new IRef<T> { target = t as UnityEngine.Object };

    public void Set(T t) => target = t as UnityEngine.Object;
}