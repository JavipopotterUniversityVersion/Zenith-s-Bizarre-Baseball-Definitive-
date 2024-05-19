using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinMovementBehaviour : MonoBehaviour, IBehaviour
{
    MovementController movementController;
    float randomOffset;

    private void Awake() {
        movementController = GetComponentInParent<MovementController>();
        randomOffset = Random.Range(0, 20f);
    }

    public void ExecuteBehaviour()
    {
        Vector2 perlin = new Vector2(Mathf.PerlinNoise(Time.time * randomOffset, 0), Mathf.PerlinNoise(0, Time.time * randomOffset));
        movementController.Move(perlin);
    }
}
