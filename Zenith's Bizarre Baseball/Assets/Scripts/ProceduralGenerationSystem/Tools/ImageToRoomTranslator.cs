using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class ImageToRoomTranslator : MonoBehaviour
{
    [SerializeField] Texture2D _image;
    [SerializeField] TileData[] _tileDatas;
    [SerializeField] PrefabData[] _prefabDatas;
    [SerializeField] Vector2Int offset;
    List<GameObject> _objects = new List<GameObject>();

    [ContextMenu("Clear and Translate")]
    public void ClearAndTranslate()
    {
        Clear();
        Translate();
    }
    
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
                    if (prefabData.prefabSettings.ContainsKey(color))
                    {
                        Vector3 position = prefabData.tilemap.CellToWorld(new Vector3Int(x + offset.x, y + offset.y, 0));
                        position += prefabData.tilemap.cellSize / 2;
                        position += (Vector3)prefabData.prefabSettings[color].offset;

                        Quaternion rotation = Quaternion.Euler(0, 0, prefabData.prefabSettings[color].rotation);

                        GameObject obj = PrefabUtility.InstantiatePrefab(prefabData.Prefab, prefabData.tilemap.transform) as GameObject;

                        prefabData.prefabSettings[color].Process(obj);
                        obj.transform.SetPositionAndRotation(position, rotation);

                        _objects.Add(obj);
                    }
                }

                foreach (TileData tileData in _tileDatas)
                {
                    foreach (TileSettings tileSetting in tileData.tileSettings)
                    {
                        if (tileSetting.ContainsColor(color))
                        {
                            tileData.tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), tileSetting.tile);
                        }
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
            foreach (GameObject obj in _objects)
            {
                DestroyImmediate(obj, false);
            }
        }

        _objects.Clear();
    }


    [Serializable]
    class TileData
    {
        public Tilemap tilemap;
        public TileSettings[] tileSettings;
    }

    [Serializable]
    class TileSettings
    {
        public TileBase tile;
        public Color[] colorKeys;
        public bool ContainsColor(Color color) => colorKeys.Contains(color);
    }

    [Serializable]
    class PrefabData
    {
        public Tilemap tilemap;
        [SerializeField] GameObject _prefab;
        public GameObject Prefab => _prefab;
        [FormerlySerializedAs("prefabs")]
        public SerializableDictionary<Color, PrefabInstance> prefabSettings;
    }

    [Serializable]
    class PrefabInstance
    {
        public Vector2 offset;
        public float rotation;

        [SerializeField] IRef<IGameObjectProcessor>[] processors;
        public void Process(GameObject gameObject) => IGameObjectProcessor.Process(gameObject, processors);
    }
}