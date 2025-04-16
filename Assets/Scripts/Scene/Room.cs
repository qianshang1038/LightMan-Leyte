using UnityEngine;

// Room有四种状态 除了第一种都算作Unlocked
/// <summary>
/// 默认使用Locked/Unlocked来进行状态表示。如果这个Room没有任何剩余价值，才可标记为Completed
/// </summary>
public enum RoomState
{
    Locked,
    Unlocked,
    Completed
}

public class Room : MonoBehaviour
{
    #region 面向对象字段

    public RoomState roomState = RoomState.Unlocked;
    
    // roomID将会在RoomManager中统一分配作为标识符
    public int roomID = 0;

    #endregion

    #region 外部可以使用的方法

    // 获取当前room状态
    public int GetRoomState()
    {
        Logger.Log("Current room's state is: " + roomState);
        return (int)roomState;
    }

    // 改变当前room状态
    public void ChangeRoomState(RoomState targetState)
    {
        roomState = targetState;
        Logger.Log("Current room's state changed to: " + targetState);
    }

    // 检查当前是否为上锁状态
    public bool IsLocked()
    {
        return roomState == RoomState.Locked;
    }    
    
    public bool IsCompleted()
    {
        return roomState == RoomState.Completed;
    }

    #endregion
}
