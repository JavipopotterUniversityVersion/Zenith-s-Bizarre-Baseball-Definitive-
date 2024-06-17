using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPaster : MonoBehaviour
{
    [SerializeField] Tilemap tilemapToCopy;
    [SerializeField] Tilemap tilemapToPaste;
    [SerializeField] string referenceName = "Floor";
    [SerializeField] bool isRemover = false;

    private void Awake() {
        GenericReference genericReference = GetComponentInParent<GenericReference>();
        if(genericReference != null && tilemapToPaste == null) tilemapToPaste = genericReference.GetReference(referenceName) as Tilemap;
    }

    [ContextMenu("Paste Tilemap")]
    public void PasteTilemap()
    {
        BoundsInt bounds = tilemapToCopy.cellBounds;
        TileBase[] allTiles = tilemapToCopy.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(bounds.x + x, bounds.y + y, bounds.z);
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    if(!isRemover) tilemapToPaste.SetTile(pos, tile);
                    else tilemapToPaste.SetTile(pos, null);
                }
            }
        }

        tilemapToCopy.ClearAllTiles();
    }
}
