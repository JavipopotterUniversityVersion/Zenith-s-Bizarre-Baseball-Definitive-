using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovementBehaviour : MonoBehaviour, IBehaviour
{
 
    private MovementController movementController;
    private Vector2 direction  =  Vector2.zero;
    void Awake()
    {
        movementController = GetComponentInParent<MovementController>();
    }
    public void ExecuteBehaviour()
    {
      ///    Debug.Log("dsd"+direction);
        int randomDirection = UnityEngine.Random.Range(1,5);
        if (randomDirection == 1) direction = Vector2.up;
        if (randomDirection == 2) direction = Vector2.down;
        if (randomDirection == 3) direction = Vector2.left;
        if (randomDirection == 4) direction = Vector2.right;
        movementController.Move(direction);
      
    }
    private void OnValidate() => name = $"Random Movement Behaviour";
    
}