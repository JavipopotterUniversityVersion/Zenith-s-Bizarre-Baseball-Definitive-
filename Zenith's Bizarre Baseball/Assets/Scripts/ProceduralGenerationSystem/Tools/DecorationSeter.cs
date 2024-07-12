using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;


public class DecorationSeter : MonoBehaviour
{
    [SerializeField] DecorationData[] possibleDecorations;
    NodeGenerator nodeGenerator;
    Tilemap tilemap;
    [SerializeField] List<Limit> decorationLimits = new List<Limit>();
    [SerializeField] Tile extensionTile;

    private void Start() 
    {
        tilemap = TilemapSingleton.Instance.map;
        nodeGenerator = GetComponent<NodeGenerator>();
        nodeGenerator.OnFinishedGeneration.AddListener(() => StartCoroutine(PutDecoration()));
    }

    IEnumerator PutDecoration()
    {
        yield return new WaitForSeconds(0.1f);
        Decorate();
    }

    void Decorate()
    {
        ExtendBounds();
        foreach(var bounds in tilemap.cellBounds.allPositionsWithin)
        {
            Vector2 worldPosition = tilemap.CellToWorld(bounds);
            if(HasAnAdjacentTile(bounds, 5))
            {
                if(GetCorrectObject(worldPosition, out GameObject returnedObject))
                {
                    decorationLimits.Add(returnedObject.GetComponent<LimitInstance>().InstanceLimit);
                }
            }
        }
    }

    void ExtendBounds()
    {
        Vector3Int min = tilemap.cellBounds.min;
        Vector3Int max = tilemap.cellBounds.max;

        tilemap.SetTile(new Vector3Int(min.x - 5, min.y - 5, 0), extensionTile);
        tilemap.SetTile(new Vector3Int(max.x + 5, max.y + 5, 0), extensionTile);
    }


    bool HasAnAdjacentTile(Vector3Int position, int distance = 1)
    {
        if(tilemap.HasTile(position)) return false;

        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right,
            new Vector3Int(1, 1, 0),
            new Vector3Int(-1, -1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(-1, 1, 0)
        };

        foreach(Vector3Int direction in directions)
        {
            for(int i = 1; i <= distance; i++)
            {
                if(tilemap.HasTile(position + direction * i)) return true;
            }
        }

        return false;
    }

    bool CanBePlaced(DecorationData decoration, Vector3Int position, ref GameObject returnedObject)
    {
        Vector2Int size = decoration.size;

        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                if(tilemap.HasTile(position + new Vector3Int(x, y, 0) * 2)) return false;
            }
        }

        GameObject deco = Instantiate(decoration.gameObject, tilemap.CellToWorld(position), Quaternion.identity);
        Limit limit = deco.GetComponent<LimitInstance>().InstanceLimit;

        returnedObject = deco;

        if(Limit.OverlapsInclusive(limit, decorationLimits.ToArray()))
        {
            Destroy(deco);
            return false;
        }

        return true;
    }

    public bool GetCorrectObject(Vector2 position, out GameObject returnedObject)
    {
        DecorationData selectedDecoration = possibleDecorations[UnityEngine.Random.Range(0, possibleDecorations.Length)];

        GameObject obj = null;

        int numberOfTries = 0;
        while(numberOfTries < 3 && CanBePlaced(selectedDecoration, tilemap.WorldToCell(position), ref obj) == false) numberOfTries++;

        returnedObject = obj;

        return numberOfTries < 3;
    }

    [Serializable]
    class DecorationData
    {
        public GameObject gameObject;
        public Vector2Int size;
    }
}
