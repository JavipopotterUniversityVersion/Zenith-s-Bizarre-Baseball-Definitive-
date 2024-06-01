using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FaceTargetBehaviour : MonoBehaviour, IBehaviour
{
    TargetHandler targetHandler;
    float lastDirection;
    [SerializeField] UnityEvent onFaceLeft = new UnityEvent();
    [SerializeField] UnityEvent onFaceRight = new UnityEvent();

    private void Awake() {
        targetHandler = GetComponentInParent<TargetHandler>();
    }

    public void ExecuteBehaviour()
    {
        if(targetHandler.Target != null)
        {
            float direction = Mathf.Sign(targetHandler.GetTargetDirection().x);

            if(direction != lastDirection)
            {
                if(direction < 0) onFaceLeft.Invoke();
                else onFaceRight.Invoke();
            }
        }
    }

    private void OnValidate() {
        gameObject.name = "Face Target";
    }
}
