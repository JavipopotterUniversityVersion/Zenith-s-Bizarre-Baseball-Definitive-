using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftRandomMovement : MonoBehaviour, IBehaviour
{
    float timer;
    MovementController movementController;
    Vector2 randomDirection = Vector2.zero;
    [SerializeField] Vector2 directionChangeTime;
    [SerializeField] float maxDistanceToInitialPosition;
    Vector2 initialPosition;
    [Range(0, 1)]
    [SerializeField] float lerpFactor = 1f;
    [SerializeField] float multiplier = 1f;
    float maxTime;

    private void Awake() {
        movementController = GetComponentInParent<MovementController>();
        maxTime = Mathf.Abs(Random.Range(directionChangeTime.x, directionChangeTime.y));
        initialPosition = transform.position;
    }

    public void ExecuteBehaviour()
    {
        if(timer >= maxTime)
        {
            timer = 0;
            maxTime = Mathf.Abs(Random.Range(directionChangeTime.x, directionChangeTime.y));
            EvaluateDirection();
        }
        else
        {
            Vector2 lerpDirection = Vector2.Lerp(movementController.Rb.velocity.normalized,
            randomDirection, timer / maxTime * lerpFactor);

            movementController.Move(lerpDirection * multiplier);
            timer += Time.deltaTime;
        }
    }

    void EvaluateDirection()
    {
        
        if(Vector2.Distance(transform.position, initialPosition) > maxDistanceToInitialPosition)
        {
            randomDirection = (initialPosition - (Vector2)transform.position).normalized;
        }
        else
        {
            float angle = Random.Range(0, 360);
            randomDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

    private void OnValidate() {
        name = $"Soft Random Movement";
    }
}
