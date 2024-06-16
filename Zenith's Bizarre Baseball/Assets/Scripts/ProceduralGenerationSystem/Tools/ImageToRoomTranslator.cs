using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ImageToRoomTranslator : MonoBehaviour
{
    [SerializeField] Texture2D _image;
    [SerializeField] TileData[] _tileDatas;
    [SerializeField] Vector2Int offset;

    [ContextMenu("Translate")]
    public void Translate()
    {
        for (int x = 0; x < _image.width; x++)
        {
            for (int y = 0; y < _image.height; y++)
            {
                Color color = _image.GetPixel(x, y);

                foreach (TileData tileData in _tileDatas)
                {
                    if (tileData.tiles.ContainsKey(color))
                    {
                        tileData.tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), tileData.tiles[color]);
                        break;
                    }
                }
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (TileData tileData in _tileDatas)
        {
            tileData.tilemap.ClearAllTiles();
        }
    }

    [Serializable]
    class TileData
    {
        public Tilemap tilemap;
        public SerializableDictionary<Color, TileBase> tiles;
    }
}