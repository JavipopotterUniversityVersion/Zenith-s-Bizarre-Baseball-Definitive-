using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnSpeedCondition : MonoBehaviour, ICondition
{
    MovementController _controller;
    private void Awake()
    {
        _controller = GetComponent<MovementController>();
    }
    public bool CheckCondition()
    {    
      _controller.ReturnOriginalSpeed(); //Llamada al m�todo new speed para igualar la velocidad a SlowedSpeed
      return true;
    }
}
