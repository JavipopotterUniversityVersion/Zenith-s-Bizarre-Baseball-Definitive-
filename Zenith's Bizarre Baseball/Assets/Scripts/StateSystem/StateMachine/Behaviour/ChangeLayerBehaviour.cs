using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChangeDistanceItem : MonoBehaviour, IBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] int targetLayer;
    public void ExecuteBehaviour()
    {
        print("change to " + targetLayer);
        item.layer = targetLayer;
    }
}
