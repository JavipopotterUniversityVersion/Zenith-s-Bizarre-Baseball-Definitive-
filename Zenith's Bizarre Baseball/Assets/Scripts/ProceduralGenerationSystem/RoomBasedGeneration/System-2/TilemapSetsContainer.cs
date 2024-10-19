using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSetsContainer : MonoBehaviour
{
    List<Tilemap> _sets = new List<Tilemap>();
    public List<Tilemap> Sets => _sets;
}
