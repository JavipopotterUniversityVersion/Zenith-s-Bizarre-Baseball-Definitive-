using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Node))]
public class MapPaster : MonoBehaviour
{
    private void Start() 
    {
        Tilemap tilemap = GetComponent<GenericReference>().GetReference("Walls") as Tilemap;
        GetComponent<Node>().Generator.OnFinishedGeneration.AddListener(() => StartCoroutine(PasteOnMap(tilemap)));
    }

    IEnumerator PasteOnMap(Tilemap tilemap)
    {
        yield return new WaitForSeconds(0.05f);
        TilemapSingleton.Instance.PasteOnMap(tilemap);
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
    }
}
