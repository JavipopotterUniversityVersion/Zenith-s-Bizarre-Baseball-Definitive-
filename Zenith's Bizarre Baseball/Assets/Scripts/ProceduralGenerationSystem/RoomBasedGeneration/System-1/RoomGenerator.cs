using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] RoomSetting initialRoom;
    RoomSettingStack roomSettings;
    [SerializeField] RoomSetting[] Rooms;
    RoomSetting[,] createdRooms = new RoomSetting[100, 100];
    List<Vector2Int> roomPositions = new List<Vector2Int>();
    List<Vector2Int> lastRoomsPositions = new List<Vector2Int>();
    [SerializeField] Vector2Int roomSize = new Vector2Int();
    [SerializeField] int maxRoomExtension = 40;
    int extensionCounter = 0;


    [SerializeField] UnityEvent onLoading = new UnityEvent();
    public UnityEvent OnLoading => onLoading;

    [SerializeField] UnityEvent onLoaded = new UnityEvent();
    public UnityEvent OnLoaded => onLoaded;

    [SerializeField]
    [Range(0, 1)]
    float linearity = 0.5f;

    [SerializeField] float timeBetweenRooms = 0.25f;

    private void Awake()
    {
        //pantalla de carga
        roomSettings = new RoomSettingStack(Rooms);
        transform.position = new Vector2(createdRooms.GetLength(0) / 2 * roomSize.x, createdRooms.GetLength(1) / 2 * roomSize.y);
        GenerateRoom((int)transform.position.x / roomSize.x, (int)transform.position.y / roomSize.y, initialRoom, RoomAccess.North);
        StartCoroutine(GenerateRooms());
    }

    private void Start() => onLoading?.Invoke();

    RoomSetting GenerateRandomRoom(int x, int y, RoomAccess accessValue) //Sala aleatoria
    {
        RoomSetting newRoom = ReturnRandomRoom(x, y, roomSettings);

        newRoom.Room.SetAccess(ReturnRandomAccess(accessValue));
        lastRoomsPositions.Add(new Vector2Int(x, y));
        createdRooms[x, y] = newRoom;
        // roomPositions.Add(new Vector2Int(x, y));
        extensionCounter++;
        return newRoom;
    }

    RoomSetting GenerateStrictRoom(int x, int y, RoomAccess accessValue, RoomSettingStack roomSettings)
    {
        RoomSetting newRoom = ReturnRandomRoom(x, y, roomSettings);
        newRoom.Room.SetAccess(accessValue);
        createdRooms[x, y] = newRoom;
        // roomPositions.Add(new Vector2Int(x, y));
        return newRoom;
    }

    RoomSetting ReturnRandomRoom(int x, int y, RoomSettingStack roomSettingStack)
    {
        RoomSetting randomRoom = roomSettingStack.RandomRoomSetting();

        GameObject newRoomGameObject = Instantiate(randomRoom.RoomPrefab, new Vector2(x * roomSize.x, y * roomSize.y), Quaternion.identity);
        RoomSetting newRoom = new RoomSetting(newRoomGameObject, randomRoom.Probability, randomRoom.MinNumOfInstances);
        return newRoom;
    }

    RoomAccess ReturnRandomAccess(RoomAccess accesValue)
    {
        if(Random.value < linearity) return OppossiteAccess(accesValue) | accesValue;
        
        RoomAccess newAccess = (RoomAccess)Random.Range(1, 15);
        while (accesValue == newAccess)
        {           
            newAccess = (RoomAccess)Random.Range(1, 15);
        }
        accesValue |= newAccess;
        return accesValue;
    }
    
    void GenerateRoom(int x, int y, RoomSetting room, RoomAccess roomAccess) // Sala Inicial
    {
        GameObject newRoomGameObject = Instantiate(room.RoomPrefab, new Vector2(x * roomSize.x, y * roomSize.y), Quaternion.identity);
        RoomSetting newRoom = new RoomSetting(newRoomGameObject, room.Probability, room.MinNumOfInstances);
        newRoom.Room.SetAccess(roomAccess);
        lastRoomsPositions.Add(new Vector2Int(x, y));
        createdRooms[x, y] = newRoom;
        extensionCounter++;
    }

    RoomAccess OppossiteAccess(RoomAccess accessValue)
    {
        switch (accessValue)
        {
            case RoomAccess.North:
                return RoomAccess.South;
            case RoomAccess.East:
                return RoomAccess.West;
            case RoomAccess.South:
                return RoomAccess.North;
            case RoomAccess.West:
                return RoomAccess.East;
            default:
                return RoomAccess.None;
        }
    }

    bool CheckPosition(int x, int y) => createdRooms[x, y] == null;

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

    IEnumerator GenerateRooms()
    {
        while(lastRoomsPositions.Count > 0)
        {
            Vector2Int[] roomPositions = lastRoomsPositions.ToArray();
            lastRoomsPositions.Clear();

            foreach(Vector2Int _roomPosition in roomPositions)
            {
                Room selectedRoom = createdRooms[_roomPosition.x, _roomPosition.y].Room;
                if(extensionCounter < maxRoomExtension)
                {
                    foreach (RoomAccess access in selectedRoom.GetAllAccess())
                    {
                        Vector2Int direction = AccessValueToVector2(access);
                        Vector2Int nextRoomPosition = _roomPosition + direction;
                        RoomAccess nextRoomAccess = OppossiteAccess(access);

                        if (CheckPosition(nextRoomPosition.x, nextRoomPosition.y))
                        {
                            GenerateRandomRoom(nextRoomPosition.x, nextRoomPosition.y, nextRoomAccess);
                        }
                        else if(!createdRooms[nextRoomPosition.x, nextRoomPosition.y].Room.totalAccess.HasFlag(nextRoomAccess)) //POSIBLE ACTUALIZACI�N
                        {
                            RoomAccess fixedAccess = createdRooms[nextRoomPosition.x, nextRoomPosition.y].Room.totalAccess | nextRoomAccess;
                            Destroy(createdRooms[nextRoomPosition.x, nextRoomPosition.y].Room.gameObject);
                            GenerateStrictRoom(nextRoomPosition.x, nextRoomPosition.y, fixedAccess, roomSettings);
                        }
                    }
                }
                else
                {
                    foreach (RoomAccess access in selectedRoom.GetAllAccess())
                    {
                        Vector2Int direction = AccessValueToVector2(access);
                        Vector2Int nextRoomPosition = _roomPosition + direction;
                        RoomAccess nextRoomAccess = OppossiteAccess(access);

                        if (CheckPosition(nextRoomPosition.x, nextRoomPosition.y))
                        {
                            GenerateStrictRoom(nextRoomPosition.x, nextRoomPosition.y, nextRoomAccess, roomSettings);
                            this.roomPositions.Add(nextRoomPosition);
                        }
                        else if (!createdRooms[nextRoomPosition.x, nextRoomPosition.y].Room.totalAccess.HasFlag(nextRoomAccess))//POSIBLE ACTUALIZACI�N
                        {
                            RoomAccess fixedAccess = createdRooms[nextRoomPosition.x, nextRoomPosition.y].Room.totalAccess | nextRoomAccess;
                            Destroy(createdRooms[nextRoomPosition.x, nextRoomPosition.y].Room.gameObject);
                            GenerateStrictRoom(nextRoomPosition.x, nextRoomPosition.y, fixedAccess, roomSettings);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(timeBetweenRooms);
        }
        
        foreach(RoomSetting roomSetting in roomSettings.RoomSettings)
        {
            if(roomSettings.RoomSettingInstances[roomSetting] < roomSetting.MinNumOfInstances)
            {
                for(int i = 0; i < roomSetting.MinNumOfInstances - roomSettings.RoomSettingInstances[roomSetting]; i++)
                {
                    Vector2Int randomRoomPosition;
                    randomRoomPosition = roomPositions[Random.Range(0, roomPositions.Count)];
                    
                    RoomAccess access = createdRooms[randomRoomPosition.x, randomRoomPosition.y].Room.totalAccess;
                    Destroy(createdRooms[randomRoomPosition.x, randomRoomPosition.y].Room.gameObject);
                    GenerateRoom(randomRoomPosition.x, randomRoomPosition.y, roomSetting, access);

                    roomPositions.Remove(randomRoomPosition);
                }
            }
        }
        ///desactivar Pantalla de carga
        onLoaded?.Invoke();
    }
}

[System.Serializable]
public class RoomSetting
{
    [SerializeField] GameObject roomPrefab;
    public GameObject RoomPrefab => roomPrefab;

    Room room;
    public Room Room => room;

    [Range(0,1)] 
    [SerializeField] float probability;
    public float Probability => probability;

    [SerializeField] int minNumOfInstances;
    public int MinNumOfInstances => minNumOfInstances;

    public RoomSetting(GameObject roomPrefab, float probability, int minNumOfInstances)
    {
        this.roomPrefab = roomPrefab;
        this.probability = probability;
        this.minNumOfInstances = minNumOfInstances;
        room = roomPrefab.GetComponent<Room>();
    }

    public void SetProbability(float probability) => this.probability = probability;
}

[System.Serializable]
public class RoomSettingStack
{
    [SerializeField] RoomSetting[] roomSettings;
    public RoomSetting[] RoomSettings => roomSettings;

    Dictionary<RoomSetting, int> roomSettingInstances = new Dictionary<RoomSetting, int>();
    public Dictionary<RoomSetting, int> RoomSettingInstances => roomSettingInstances;

    public RoomSettingStack(RoomSetting[] roomSettings)
    {
        this.roomSettings = roomSettings;

        foreach(RoomSetting roomSetting in roomSettings)
        {
            roomSettingInstances.Add(roomSetting, 0);
        }
    }

    public RoomSetting RandomRoomSetting()
    {
        float randomValue = Random.value;
        float probabilitySum = 0;
        foreach(RoomSetting roomSetting in roomSettings)
        {
            probabilitySum += roomSetting.Probability;
            if(randomValue <= probabilitySum)
            {
                roomSettingInstances[roomSetting]++;
                return roomSetting;
            }
        }
        roomSettingInstances[roomSettings[roomSettings.Length - 1]]++;
        return roomSettings[roomSettings.Length - 1];
    }
}