using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Threading.Tasks;


public class DecorationSeter : MonoBehaviour
{
    [SerializeField] DecorationData[] possibleDecorations;
    NodeGenerator nodeGenerator;
    Tilemap tilemap;
    [SerializeField] List<Limit> decorationLimits = new List<Limit>();
    [SerializeField] Tile extensionTile;
    [SerializeField] int margin;
    LimitInstance limitHelper;

    private void Start() 
    {
        tilemap = TilemapSingleton.Instance.map;
        limitHelper = GetComponentInChildren<LimitInstance>();
        nodeGenerator = GetComponent<NodeGenerator>();
        nodeGenerator.OnFinishedGeneration.AddListener(() => StartCoroutine(PutDecoration()));
    }

    public Node GetNearestNode(Vector2 position)
    {
        Node nearestNode = null;
        float nearestDistance = Mathf.Infinity;

        foreach(Node node in nodeGenerator.Nodes)
        {
            float distance = Vector2.Distance(node.transform.position, position);
            if(distance < nearestDistance)
            {
                nearestNode = node;
                nearestDistance = distance;
            }
        }

        return nearestNode;
    }

    IEnumerator PutDecoration()
    {
        yield return new WaitForSeconds(0.1f);

        int i = 0;
        
        ExtendBounds();
        List<Vector2> positions = new List<Vector2>();

        foreach(var bounds in tilemap.cellBounds.allPositionsWithin)
        {
            Vector2 worldPosition = tilemap.CellToWorld(bounds);
            if(HasAnAdjacentTile(bounds, margin))
            {
                AddPositionByOriginDistance(worldPosition, positions);
            }
            i++;
        }

        PlaceDecoration(positions);
    }

    void AddPositionByOriginDistance(Vector2 position, List<Vector2> positions)
    {
        for(int i = 0; i < positions.Count; i++)
        {
            if(position.magnitude < positions[i].magnitude)
            {
                positions.Insert(i, position);
                return;
            }
        }

        positions.Add(position);
    }

    async void PlaceDecoration(List<Vector2> positions)
    {
        int half = positions.Count / 2;
        for(int i = 0; i < positions.Count; i++)
        {
            Vector2 pos = positions[i];
            if(GetCorrectObject(pos, out GameObject returnedObject))
            {
                if(GetNearestNode(pos).TryGetComponent(out TrueCondition discovered))
                {
                    returnedObject.SetActive(false);
                    discovered.OnValueChange.AddListener(() => returnedObject.SetActive(discovered.CheckCondition()));
                }
                else
                {
                    Debug.LogWarning("Javi hermano, por qué este nodo no tiene condición de descubrimiento? espabila que la vida te va a comer " + GetNearestNode(pos).name);
                }

                decorationLimits.Add(returnedObject.GetComponent<LimitInstance>().Limit);
            }
            
            if(i < half)
            {
                if(i % 16 == 0) await Task.Delay(1);
            }
            else if(i < half * 2)
            {
                if(i % 8 == 0) await Task.Delay(1);
            }

        }
    }

    void ExtendBounds()
    {
        Vector3Int min = tilemap.cellBounds.min;
        Vector3Int max = tilemap.cellBounds.max;

        tilemap.SetTile(new Vector3Int(min.x - margin, min.y - margin, 0), extensionTile);
        tilemap.SetTile(new Vector3Int(max.x + margin, max.y + margin, 0), extensionTile);
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

    bool CanBePlaced(DecorationData decoration, Vector3Int position)
    {
        Vector2Int size = decoration.size;

        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                if(tilemap.HasTile(position + new Vector3Int(x, y, 0) * 2)) return false;
            }
        }

        LimitInstance decoLimit = decoration.gameObject.GetComponent<LimitInstance>();
        limitHelper.Limit.CopyLimits(decoLimit.Limit);
        limitHelper.transform.position = tilemap.CellToWorld(position);

        if(Limit.OverlapsInclusive(limitHelper.Limit, decorationLimits.ToArray())) return false;

        return true;
    }

    public bool GetCorrectObject(Vector2 position, out GameObject returnedObject)
    {
        DecorationData selectedDecoration = DecorationData.GetRandomDecoration(possibleDecorations);

        int numberOfTries = 0;
        while(numberOfTries < 3 && CanBePlaced(selectedDecoration, tilemap.WorldToCell(position)) == false) numberOfTries++;

        if(numberOfTries < 3)
        {
            returnedObject = Instantiate(selectedDecoration.gameObject, position, Quaternion.identity);
            return true;
        }

        returnedObject = null;
        return false;
    }

    [Serializable]
    class DecorationData
    {
        public GameObject gameObject;
        public Vector2Int size;
        [SerializeField] [Range(0, 1)] float probability;

        public static DecorationData GetRandomDecoration(DecorationData[] decorations)
        {
            if(decorations.Length == 0) Debug.LogError("No decorations to choose from");

            float sum = decorations.Sum(d => d.probability);
            float random = UnityEngine.Random.Range(0, sum);
            float current = 0;

            foreach(DecorationData decoration in decorations)
            {
                current += decoration.probability;
                if(random < current) return decoration;
            }

            return decorations[0];
        }
    }
}
