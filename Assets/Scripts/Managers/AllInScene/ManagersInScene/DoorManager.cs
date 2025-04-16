using System.Collections.Generic;
using UnityEngine;
using scene;
using System;

public class DoorManager : Singleton<DoorManager>
{
    #region 自身属性字段

    [SerializeField] Transform doorsParent;
    private Dictionary<int, Door> doors = new Dictionary<int, Door>();
    private int nextDoorID; // 用于生成唯一 ID

    // SceneEventManager中订阅
    public event EventHandler OnDoorUnlocked;
    public event EventHandler OnDoorLocked;

    #endregion

    #region 生命周期函数

    private void Awake()
    {
        nextDoorID = 0;
    }

    private void Start()
    {
        SceneEventManager.Instance.OnDoorOpened += OnDoorOpened_DoorManager;
        InitializeDoors(doorsParent);
    }

    private void OnDoorOpened_DoorManager(object sender, System.EventArgs doorID)
    {
    }

    private void OnDestroy()
    {
        
    }

    #endregion

    private void InitializeDoors(Transform doorsParent)
    {
        foreach (Transform doorTransform in doorsParent)
        {
            Door door = doorTransform.GetComponent<Door>();
            // 始终增加nextDoorID并且进行分配
            if (door != null)
            {
                door.doorID = nextDoorID++;
                AddDoor(door.doorID, door);
            }
        }
    }

    // 添加Door
    private void AddDoor(int doorID, Door door)
    {
        if (!doors.ContainsKey(doorID))
        {
            doors.Add(doorID, door);
        }
        else
        {
            Logger.LogError("One doorID used by multiple doors!");
        }
    }

    // 输入指定Door进行解锁
    public void UnlockDoor(int doorID)
    {
        if (doors.ContainsKey(doorID))
        {
            doors[doorID].Unlock();
            OnDoorUnlocked?.Invoke(doors[doorID], EventArgs.Empty);
        }
    }
    
    // 输入指定Door进行上锁
    public void LockDoor(int doorID)
    {
        if (doors.ContainsKey(doorID))
        {
            doors[doorID].Unlock();
            OnDoorLocked?.Invoke(doors[doorID], EventArgs.Empty);
        }
    }

    // 同上方法
    public void UnlockSpecialDoor(int doorID)
    {
        UnlockDoor(doorID);
        OnDoorLocked?.Invoke(doorID, EventArgs.Empty);  
        Logger.Log("特殊门已解锁：" + doorID);
    }
    
    // 检索对应Door是否上锁
    public bool IsDoorUnlocked(int doorID)
    {
        return doors.ContainsKey(doorID) && doors[doorID].IsUnlocked();
    }

    // 每一关的DoorManager都要摧毁
    protected override bool ShouldDestroyOnLoad() => true;
}
