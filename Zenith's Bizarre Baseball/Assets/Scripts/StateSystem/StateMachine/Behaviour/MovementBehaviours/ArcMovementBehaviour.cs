using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcMovementBehaviour : MonoBehaviour, IBehaviour
{
    MovementController movementController;
    [SerializeField] AnimationCurve curve;

    [SerializeField]
    float duration = 1f;

    [SerializeField]
    float height = 1f;

    [SerializeField] bool _random = true;
    [SerializeField] Vector2 rangeX;
    [SerializeField] Vector2 rangeY;

    private void Awake() {
        movementController = GetComponentInParent<MovementController>();
    }

    public void ExecuteBehaviour()
    {
        Vector2 start = movementController.transform.position;
        Vector2 end = new Vector2(Random.Range(rangeX.x, rangeX.y), Random.Range(rangeY.x, rangeY.y));


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