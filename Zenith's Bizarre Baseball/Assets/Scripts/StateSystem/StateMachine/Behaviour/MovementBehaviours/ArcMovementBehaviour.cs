using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcMovementBehaviour : MonoBehaviour, IBehaviour
{
    enum ArcAreaType { Square, Circle }
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

    [SerializeField] ArcAreaType areaType = ArcAreaType.Square;

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

        float x;
        float y;

        if(areaType == ArcAreaType.Circle)
        {
            float angle = Random.Range(0, 360);
            float radius = Random.Range(rangeX.x, rangeX.y);

            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        }
        else
        {
            x = Random.Range(rangeX.x, rangeX.y);
            y = Random.Range(rangeY.x, rangeY.y);
        }

        Vector2 end = new Vector2(x, y);


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
