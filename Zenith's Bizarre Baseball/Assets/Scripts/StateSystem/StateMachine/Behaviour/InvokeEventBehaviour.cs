using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEventBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] UnityEvent OnExecuteBehaviour;
    
    [ContextMenu("ExecuteBehaviour")]
    public void ExecuteBehaviour() => OnExecuteBehaviour?.Invoke();
}
