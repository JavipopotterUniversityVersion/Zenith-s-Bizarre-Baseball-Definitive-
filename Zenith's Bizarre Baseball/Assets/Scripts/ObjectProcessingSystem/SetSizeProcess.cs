using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSizeProcess : MonoBehaviour, IGameObjectProcessor
{
    [SerializeField] ObjectProcessor size;

    public void Process(GameObject gameObject)
    {
        float size = this.size.Result();
        gameObject.transform.localScale = new Vector3(size, size, 0);
    }
}