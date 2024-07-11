using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColorer : MonoBehaviour
{
    public void ColorChildren(Color color)
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = color;
        }
    }
}
