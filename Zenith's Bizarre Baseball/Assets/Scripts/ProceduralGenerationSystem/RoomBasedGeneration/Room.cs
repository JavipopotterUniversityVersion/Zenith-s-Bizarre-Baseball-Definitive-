using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags] public enum RoomAccess
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8
}

public class Room : MonoBehaviour
{
    [SerializeField] RoomAccess _totalAccess;
    public RoomAccess totalAccess => _totalAccess;

    private void Update() {
        foreach(RoomAccess access in GetAllAccess())
        {
            Vector2 debugDirection = AccessValueToVector2(access);
            Debug.DrawRay(transform.position, new Vector2(debugDirection.x, debugDirection.y) * 10, Color.red); 
        }
    }

    private void OnDrawGizmos() {
        //Gizmos.DrawCube(transform.position, new Vector3(5, 5, 5));
    }

    Vector2Int AccessValueToVector2(RoomAccess accessValue)
    {
        switch (accessValue)
        {
            case RoomAccess.North:
                return new Vector2Int(0, 1);
            case RoomAccess.East:
                return new Vector2Int(1, 0);
            case RoomAccess.South:
                return new Vector2Int(0, -1);
            case RoomAccess.West:
                return new Vector2Int(-1, 0);
            default:
                return Vector2Int.zero;
        }
    }

    public RoomAccess[] GetAllAccess()
    {
        List<RoomAccess> accessList = new List<RoomAccess>();
        foreach(RoomAccess access in Enum.GetValues(typeof(RoomAccess)))
        {
            if(_totalAccess.HasFlag(access)) accessList.Add(access);
        }
        return accessList.ToArray();
    }

    public void SetAccess(RoomAccess roomAccess) => _totalAccess = roomAccess;
}
