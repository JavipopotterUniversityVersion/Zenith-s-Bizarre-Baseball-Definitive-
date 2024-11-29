using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(RoomNode))]
public class MapPaster : MonoBehaviour
{
    private void Start() 
    {
        Tilemap tilemap = GetComponent<GenericReference>().GetReference("Walls") as Tilemap;
        Tilemap holeMap = GetComponent<GenericReference>().GetReference("Hole") as Tilemap;
        RoomNode node = GetComponent<RoomNode>();
        if(node.Generator != null) 
        {
            node.Generator.OnFinishedGeneration.AddListener(() => {
                StartCoroutine(PasteOnMap(tilemap));
                StartCoroutine(PasteOnMap(holeMap));
        });
        }
    }

    IEnumerator PasteOnMap(Tilemap tilemap)
    {
        yield return new WaitForSeconds(0.05f);
        TilemapSingleton.Instance.PasteOnMap(tilemap);
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
    }
}
