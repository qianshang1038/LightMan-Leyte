using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    #region ���������ֶ�

    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();

    [SerializeField] Transform roomsParent;

    private int nextRoomID = 0;

    // ����Completed�Ļص���Ŀǰ��û�뵽������ʹ��
    // SceneEventManager�ж���
    public event EventHandler OnRoomCompleted;

    #endregion

    #region �������ں���

    private void Awake()
    {
        InitializeRooms(roomsParent);
    }

    private void Start()
    {
        
    }

    #endregion

    private void InitializeRooms(Transform roomsParent)
    {
        foreach (Transform roomTransform in roomsParent)
        {
            Room room = roomTransform.GetComponent<Room>();
            if (room != null)
            {
                // ֱ��ʹ�ü��������� ID
                room.roomID = nextRoomID++;
                AddRoom(room.roomID, room);
            }
        }
    }

    /// <summary>
    /// �������������ķ���(ע�᷿��)��Ϣ�� ������޹ؽ�Ҫ�ķ��������Ϊ������һ����������ӵ�RoomManager��
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="room"></param>
    public void AddRoom(int roomID, Room room)
    {
        if (!rooms.ContainsKey(roomID))
        {
            rooms.Add(roomID, room);
        }
        else
        {
            Logger.LogError("Same ID used by multiple rooms!");
        }
    }

    // ��ȡ������Ϣ�ض���Room
    public Room GetRoom(int roomID)
    {
        return rooms.ContainsKey(roomID) ? rooms[roomID] : null;
    }

    // ��鷿���Ƿ���Խ��롣
    // ��ʵ��Door�߼���һЩ�ظ��������������һЩ������Ч��
    public bool CanEnterRoom(int roomID)
    {
        Room room = GetRoom(roomID);
        return room != null && room.roomState != RoomState.Locked;
    }

    // ��鷿���Ƿ��Ѿ����
    public bool CheckIsRoomCompleted(int roomID)
    {
        return rooms[roomID].IsCompleted();
    }

}
