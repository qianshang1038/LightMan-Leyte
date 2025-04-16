using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    #region 自身属性字段

    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();

    [SerializeField] Transform roomsParent;

    private int nextRoomID = 0;

    // 房间Completed的回调，目前还没想到在哪里使用
    // SceneEventManager中订阅
    public event EventHandler OnRoomCompleted;

    #endregion

    #region 生命周期函数

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
                // 直接使用计数器生成 ID
                room.roomID = nextRoomID++;
                AddRoom(room.roomID, room);
            }
        }
    }

    /// <summary>
    /// 添加有特殊需求的房间(注册房间)信息。 如果是无关紧要的房间可以作为场景的一部分无需添加到RoomManager中
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

    // 获取根据信息特定的Room
    public Room GetRoom(int roomID)
    {
        return rooms.ContainsKey(roomID) ? rooms[roomID] : null;
    }

    // 检查房间是否可以进入。
    // 其实和Door逻辑有一些重复，但或许可以做一些触发类效果
    public bool CanEnterRoom(int roomID)
    {
        Room room = GetRoom(roomID);
        return room != null && room.roomState != RoomState.Locked;
    }

    // 检查房间是否已经完成
    public bool CheckIsRoomCompleted(int roomID)
    {
        return rooms[roomID].IsCompleted();
    }

}
