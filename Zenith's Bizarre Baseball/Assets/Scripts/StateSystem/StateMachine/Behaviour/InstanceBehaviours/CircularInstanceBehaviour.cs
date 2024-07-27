using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CircularInstanceBehaviour : MonoBehaviour, IBehaviour, IReadable
{
    [SerializeField] Transform _center;
    public void SetCenter(Transform center) => _center = center;

    [SerializeField] ObjectProcessor _radius;
    [SerializeField] InstanceUnit[] _instances;
    [SerializeField] ObjectProcessor _numberOfInstances;

    [SerializeField] ObjectProcessor _timeBetweenInstances;

    int _instanceCount;

    private void Awake() 
    {
        if(_center == null)  _center = transform;
    }
    
    public void ExecuteBehaviour() => StartCoroutine(InstanceWithTime());

    IEnumerator InstanceWithTime()
    {

        _instanceCount = 0;

        float radius = _radius.Result();

        int numberOfInstances = (int)_numberOfInstances.Result();
        float angle = 360f / numberOfInstances;

        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < numberOfInstances; i++)
        {
            float x = _center.position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad);
            float y = _center.position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad);
            positions.Add(new Vector2(x, y));
        }

        for (int i = 0; i < numberOfInstances; i++)
        {
            InstanceUnit instanceUnit = InstanceUnit.GetRandomUnit(_instances);
            
            GameObject instance = Instantiate(instanceUnit.Instance, _center.position, Quaternion.identity);
            instanceUnit.Processors.ToList().ForEach(processor => processor.Process(instance));
            
            Vector2 randomPosition = positions[Random.Range(0, positions.Count)];
            positions.Remove(randomPosition);
            instance.transform.position = randomPosition;


            _instanceCount++;

            yield return new WaitForSecondsRealtime(_timeBetweenInstances.Result());
        }
    }

    public float Read() => _instanceCount;

    private void OnValidate() {
        name = "Circular Instance";
    }
}
