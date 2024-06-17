using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileWaver : MonoBehaviour
{
    Vector3Int[,] tilePositionsArray;
    bool[,] wavedArray;
    [SerializeField] Vector2 startPoint;
    Tilemap tilemap;
    [SerializeField] float waveInterval = 0.1f;
    [SerializeField] float fadeTime = 1f;

    Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };

    private void Awake() 
    {
        tilemap = GetComponent<Tilemap>();
        SetTilesPositions();

        wavedArray = new bool[tilePositionsArray.GetLength(0), tilePositionsArray.GetLength(1)];

        StartCoroutine(WaveTile(startPoint));
    }

    IEnumerator WaveTile(Vector2 indexPos)
    {
        yield return new WaitForSeconds(waveInterval);
        wavedArray[(int)indexPos.x, (int)indexPos.y] = true;

        float time = 0;
        while (time < fadeTime)
        {
            Color color = Color.Lerp(Color.black, Color.clear, time / fadeTime);
            tilemap.SetColor(tilePositionsArray[(int)indexPos.x, (int)indexPos.y], color);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        
        foreach (var dir in directions)
        {
            Vector2 newPos = indexPos + dir;
            if (IsInBounds(newPos) && !wavedArray[(int)newPos.x, (int)newPos.y])
            {
                StartCoroutine(WaveTile(newPos));
            }
        }
    }

    bool IsInBounds(Vector2 pos)
    {
        return pos.x >= 0 && pos.x < tilePositionsArray.GetLength(0) && pos.y >= 0 && pos.y < tilePositionsArray.GetLength(1);
    }

    void SetTilesPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;
        tilePositionsArray = new Vector3Int[bounds.size.x, bounds.size.y];

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(bounds.x + x, bounds.y + y, bounds.z);
                tilePositionsArray[x, y] = pos;
            }
        }
    }
}
