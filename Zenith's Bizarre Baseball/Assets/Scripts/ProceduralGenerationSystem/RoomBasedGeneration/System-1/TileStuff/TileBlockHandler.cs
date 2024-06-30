using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBlockHandler : MonoBehaviour, IBehaviour
{
    [SerializeField] string referenceName = "Walls";

    [ContextMenu("Set Tiles")]
    public void SetTiles()
    {
        Tilemap targetTilemap = GetComponentInParent<GenericReference>().GetReference(referenceName) as Tilemap;
        TileBlock[] tileBlocks = GetComponentsInChildren<TileBlock>();

        foreach (var tileBlock in tileBlocks)
        {
            tileBlock.SetTile(targetTilemap);
        }

        gameObject.SetActive(false);
    }

    public void ExecuteBehaviour() => SetTiles();
}
