using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSeter : MonoBehaviour
{
    public void SetXScale(float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public void SetYScale(float y)
    {
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
    }
}
