using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class RandomTileFader : MonoBehaviour, ICondition
{
    [SerializeField] AnimationCurve waveCurve;
    [SerializeField] float waveInterval = 1f;
    Tilemap tilemap;
    List<Vector3Int> tilePositions = new List<Vector3Int>();

    [SerializeField] Gradient gradient;
    [SerializeField] Vector2 _delayRange = new Vector2(0.1f, 0.5f);
    bool waved = false;

    private void Awake() {
        tilemap = GetComponent<Tilemap>();

        tilePositions = GetAllTilePositions().ToList();
        foreach (var pos in tilePositions)
        {
            tilemap.SetColor(pos, gradient.Evaluate(0));
        }
    }

    public bool CheckCondition() => waved;

    private void OnValidate() => Awake();

    [ContextMenu("Wave All Tiles")]
    public void WaveAllTiles()
    {
        waved = true;
        Wave();
    }

    void Wave()
    {
        while (tilePositions.Count > 0)
        {
            Vector3Int pos = tilePositions[Random.Range(0, tilePositions.Count)];
            tilePositions.Remove(pos);
            StartCoroutine(Fade(pos));
        }
    }

    IEnumerator Fade(Vector3Int pos)
    {
        yield return new WaitForSeconds(Random.Range(_delayRange.x, _delayRange.y));
        float time = 0;
        while (time < waveInterval)
        {
            Color color = gradient.Evaluate(waveCurve.Evaluate(time / waveInterval));
            tilemap.SetColor(pos, color);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        tilemap.SetColor(pos, Color.clear);
    }

    Vector3Int[] GetAllTilePositions()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        List<Vector3Int> tilePositions = new List<Vector3Int>();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(bounds.x + x, bounds.y + y, bounds.z);
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    tilePositions.Add(pos);
                }
            }
        }

        return tilePositions.ToArray();
    }
}
