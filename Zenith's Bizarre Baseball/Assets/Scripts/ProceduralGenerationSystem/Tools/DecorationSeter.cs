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
    Tilemap tilemap;
    Node room;
    [SerializeField] List<Limit> decorationLimits = new List<Limit>();

    private void Start() 
    {
        tilemap = GetComponent<GenericReference>().GetReference("Walls") as Tilemap;
        room = GetComponent<Node>();
        room.Generator.OnFinishedGeneration.AddListener(() => StartCoroutine(PutDecoration()));
    }

    IEnumerator PutDecoration()
    {
        yield return new WaitForSeconds(0.01f);
        Put();
    }

    void Put()
    {
        tilemap.CompressBounds();
        foreach(var bounds in tilemap.cellBounds.allPositionsWithin)
        {
            Vector2 worldPosition = tilemap.CellToWorld(bounds);
            if(Limit.Contains(worldPosition, room.Limits))
            {
                if(!tilemap.HasTile(bounds))
                {
                    if(GetCorrectObject(worldPosition, out GameObject returnedObject))
                    {
                        decorationLimits.Add(returnedObject.GetComponent<LimitInstance>().InstanceLimit);
                    }
                }
            }
        }
    }

    public bool GetCorrectObject(Vector2 position, out GameObject returnedObject)
    {
        GameObject deco = Instantiate(possibleDecorations[UnityEngine.Random.Range(0, possibleDecorations.Length)].gameObject, position, 
        Quaternion.identity);

        Limit limit = deco.GetComponent<LimitInstance>().InstanceLimit;

        int numberOfTries = 0;
        while(numberOfTries < 3 && Limit.OverlapsInclusive(limit, decorationLimits.ToArray()))
        {
            Destroy(deco);

            deco = Instantiate(possibleDecorations[UnityEngine.Random.Range(0, possibleDecorations.Length)].gameObject, position, 
            Quaternion.identity);

            numberOfTries++;
        }

        returnedObject = deco;

        if(numberOfTries >= 3)
        {
            Destroy(deco);
            return false;
        }
        
        return true;
    }

    [Serializable]
    class DecorationData
    {
        public GameObject gameObject;
    }
}
