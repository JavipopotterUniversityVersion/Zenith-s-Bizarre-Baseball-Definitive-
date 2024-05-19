using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] string debugMessage;
    public void ExecuteBehaviour() => print(debugMessage);

    private void OnValidate() => name = $"Debug: {debugMessage}";
}
