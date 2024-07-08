using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class TileBlockHandler : MonoBehaviour, IBehaviour
{
    [SerializeField] string referenceName = "Walls";
    [SerializeField] UnityEvent onSetTiles = new UnityEvent();

    [ContextMenu("Set Tiles")]
    public void SetTiles()
    {
        Tilemap targetTilemap = GetComponentInParent<GenericReference>().GetReference(referenceName) as Tilemap;
        TileBlock[] tileBlocks = GetComponentsInChildren<TileBlock>();

        foreach (var tileBlock in tileBlocks)
        {
            tileBlock.SetTile(targetTilemap);
        }

        onSetTiles?.Invoke();
    }

    public void ExecuteBehaviour() => SetTiles();
}
