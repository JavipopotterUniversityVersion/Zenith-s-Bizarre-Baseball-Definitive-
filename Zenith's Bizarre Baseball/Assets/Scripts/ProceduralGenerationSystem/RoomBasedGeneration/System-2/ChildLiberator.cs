using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    // public void OnDrawGizmos() 
    // {
    //     print(GetComponentsInChildren<LiberableChild>().Length);
    //     foreach (LiberableChild child in GetComponentsInChildren<LiberableChild>(true)) 
    //     {
    //         if(hideChildren)
    //         {
    //             child.hideFlags = HideFlags.HideInHierarchy;
    //             print(child.hideFlags + " " + child.name);
    //         }
    //         else
    //         {
    //             child.hideFlags = HideFlags.None;
    //             print("shown");
    //         }
    //     }
    // }
}
