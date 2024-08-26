using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInBetweenCondition : MonoBehaviour, ICondition
{
    TargetHandler _targetHandler;
    LayerMask _obstacleLayer;

    private void Awake() {
        _targetHandler = GetComponentInParent<TargetHandler>();
        _obstacleLayer = LayerMask.GetMask("Walls");
    }

    public bool CheckCondition()
    {
        return Physics2D.Raycast(transform.position, _targetHandler.Target.position - transform.position, 4, _obstacleLayer);
    }
}
