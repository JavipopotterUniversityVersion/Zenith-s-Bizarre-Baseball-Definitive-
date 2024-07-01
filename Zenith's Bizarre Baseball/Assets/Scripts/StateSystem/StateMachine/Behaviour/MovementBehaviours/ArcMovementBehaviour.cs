using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcMovementBehaviour : MonoBehaviour, IBehaviour
{
    MovementController movementController;
    [SerializeField] AnimationCurve curve;

    [SerializeField]
    ObjectProcessor duration;

    [SerializeField]
    ObjectProcessor height;
    

    [SerializeField] bool _random = true;

    [Header("Outer Range")]
    [SerializeField] Vector2 rangeX;
    [SerializeField] Vector2 rangeY;

    [Header("Inner Range")]
    [SerializeField] Vector2 innerRangeX;
    [SerializeField] Vector2 innerRangeY;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, (Vector3)rangeX);
    }

    private void Awake() {
        movementController = GetComponentInParent<MovementController>();
    }

    public void ExecuteBehaviour()
    {
        Vector2 start = movementController.transform.position;

        Vector2 end = new Vector2(Random.Range(Random.Range(innerRangeX.x, rangeX.x), Random.Range(innerRangeX.y, rangeX.y)), 
        Random.Range(Random.Range(innerRangeY.x, rangeY.x), Random.Range(innerRangeY.y, rangeY.y)));


        if(!_random)
        {
            end += (Vector2) GetComponentInParent<TargetHandler>().Target.position;
        }
        else
        {
            end += (Vector2) movementController.transform.position;
        }

        StartCoroutine(Curve(start, end));
    }

    public void Curve(Transform end) => Curve(end.position);

    public void Curve(Vector2 end)
    {
        Vector2 start = movementController.transform.position;
        StartCoroutine(Curve(start, end));
    }

    IEnumerator Curve(Vector2 start, Vector2 end)
    {
        float timePassed = 0;
        float duration = this.duration.Result();
        float height = this.height.Result();

        while (timePassed < duration)
        {
            float t = timePassed / duration;
            Vector2 position = Vector2.Lerp(start, end, t);
            position.y += curve.Evaluate(t) * height;

            movementController.transform.position = position;

            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    private void OnValidate() {
        name = $"ArcMovementBehaviour {duration}s";
    }
}
