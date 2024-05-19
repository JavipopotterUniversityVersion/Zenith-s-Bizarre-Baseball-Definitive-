using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChangeSizeBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] Vector3 finalSize
        ;
    [SerializeField] GameObject targetObject;
    [SerializeField] float time = 2;
    float timer = 0;
    Vector3 initialScale;

    void Awake()
    {
        initialScale = targetObject.transform.localScale;
    }

    public void ExecuteBehaviour()
    {
        if (timer < time)
        {
            targetObject.transform.localScale = Vector3.Lerp(initialScale, finalSize, timer / time);
        }
        else
        {
            timer = 0;
        }

        timer += Time.deltaTime;

    }
    private void OnValidate()
    {
        name = "Change object size";
    }
}
