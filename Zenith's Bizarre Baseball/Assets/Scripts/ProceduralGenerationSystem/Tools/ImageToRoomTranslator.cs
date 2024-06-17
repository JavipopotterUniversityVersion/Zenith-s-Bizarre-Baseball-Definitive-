using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ImageToRoomTranslator : MonoBehaviour
{
    [SerializeField] Texture2D _image;
    [SerializeField] TileData[] _tileDatas;
    [SerializeField] PrefabData[] _prefabDatas;
    [SerializeField] Vector2Int offset;

    [ContextMenu("Translate")]
    public void Translate()
    {
        for (int x = 0; x < _image.width; x++)
        {
            for (int y = 0; y < _image.height; y++)
            {
                Color color = _image.GetPixel(x, y);

                foreach (PrefabData prefabData in _prefabDatas)
                {
                    if (prefabData.prefabs.ContainsKey(color))
                    {
                        Vector3 position = prefabData.tilemap.CellToWorld(new Vector3Int(x + offset.x, y + offset.y, 0));
                        position += prefabData.tilemap.cellSize / 2;
                        position += (Vector3)prefabData.prefabs[color].offset;

                        Quaternion rotation = Quaternion.Euler(0, 0, prefabData.prefabs[color].rotation);

                        GameObject obj = PrefabUtility.InstantiatePrefab(prefabData.prefabs[color].Prefab, prefabData.tilemap.transform) as GameObject;

                        prefabData.prefabs[color].Process(obj);
                        obj.transform.SetPositionAndRotation(position, rotation);
                        break;
                    }
                }

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

        foreach (PrefabData prefabData in _prefabDatas)
        {
            foreach (Transform child in prefabData.tilemap.transform)
            {
                DestroyImmediate(child.gameObject, false);
            }
        }
    }

    [Serializable]
    class TileData
    {
        public Tilemap tilemap;
        public SerializableDictionary<Color, TileBase> tiles;
    }

    [Serializable]
    class PrefabData
    {
        public Tilemap tilemap;
        public SerializableDictionary<Color, PrefabInstance> prefabs;
    }

    [Serializable]
    class PrefabInstance
    {
        [SerializeField] GameObject _prefab;
        public GameObject Prefab => _prefab;

        public Vector2 offset;
        public float rotation;

        [SerializeField] IRef<IGameObjectProcessor>[] processors;
        public void Process(GameObject gameObject) => IGameObjectProcessor.Process(gameObject, processors);
    }
}