using System.Collections.Generic;
using UnityEngine;
using scene;
using System;

public class DoorManager : Singleton<DoorManager>
{
    #region ���������ֶ�

    [SerializeField] Transform doorsParent;
    private Dictionary<int, Door> doors = new Dictionary<int, Door>();
    private int nextDoorID; // ��������Ψһ ID

    // SceneEventManager�ж���
    public event EventHandler OnDoorUnlocked;
    public event EventHandler OnDoorLocked;

    #endregion

    #region �������ں���

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
            // ʼ������nextDoorID���ҽ��з���
            if (door != null)
            {
                door.doorID = nextDoorID++;
                AddDoor(door.doorID, door);
            }
        }
    }

    // ���Door
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

    // ����ָ��Door���н���
    public void UnlockDoor(int doorID)
    {
        if (doors.ContainsKey(doorID))
        {
            doors[doorID].Unlock();
            OnDoorUnlocked?.Invoke(doors[doorID], EventArgs.Empty);
        }
    }
    
    // ����ָ��Door��������
    public void LockDoor(int doorID)
    {
        if (doors.ContainsKey(doorID))
        {
            doors[doorID].Unlock();
            OnDoorLocked?.Invoke(doors[doorID], EventArgs.Empty);
        }
    }

    // ͬ�Ϸ���
    public void UnlockSpecialDoor(int doorID)
    {
        UnlockDoor(doorID);
        OnDoorLocked?.Invoke(doorID, EventArgs.Empty);  
        Logger.Log("�������ѽ�����" + doorID);
    }
    
    // ������ӦDoor�Ƿ�����
    public bool IsDoorUnlocked(int doorID)
    {
        return doors.ContainsKey(doorID) && doors[doorID].IsUnlocked();
    }

    // ÿһ�ص�DoorManager��Ҫ�ݻ�
    protected override bool ShouldDestroyOnLoad() => true;
}
