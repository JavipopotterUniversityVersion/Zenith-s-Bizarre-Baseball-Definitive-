using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLiberator : MonoBehaviour
{
    public void LiberateChildren()
    {
        foreach (LiberableChild child in GetComponentsInChildren<LiberableChild>(true)) 
        {
            child.transform.SetParent(null);
        }
    }
}
