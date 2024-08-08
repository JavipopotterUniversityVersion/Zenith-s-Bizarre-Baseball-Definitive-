 using UnityEngine;

public class InstantiateBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float velocity = 0.0f, rotation=0.0f;
    [SerializeField] Transform _instancePoint;
    [SerializeField] IRef<IGameObjectProcessor>[] _instanceProcessors;

    private void Awake() {
        if(_instancePoint == null)
            _instancePoint = transform;
    }

    public void SetInstancePoint(Transform transform) => _instancePoint = transform;

    public void ExecuteBehaviour()
    {
        Quaternion addRotation = Quaternion.Euler(0,0,rotation);
        GameObject bullet = Instantiate(prefab, _instancePoint.position, Quaternion.identity * addRotation * transform.rotation);
        if(bullet.TryGetComponent(out Rigidbody2D rb)) rb.velocity = transform.up * velocity;

        foreach (IRef<IGameObjectProcessor> processor in _instanceProcessors)
        {
            processor.I.Process(bullet);
        }
    }

    private void OnValidate()
    {
        if(prefab != null)
            name = $"Instantiate {prefab.name}";
    }
}