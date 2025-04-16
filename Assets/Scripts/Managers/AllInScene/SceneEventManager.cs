using System;
using scene;

public class SceneEventManager : Singleton<SceneEventManager>
{
    public event EventHandler OnDoorOpened;

    public RoomManager roomManager;
    public DoorManager doorManager;
    // public ItemManager itemManager;

    #region 生命周期函数

    private void Start()
    {
        // 使用单例模式初始化管理器，确保共享状态
        roomManager = RoomManager.Instance;
        doorManager = DoorManager.Instance;
        // itemManager = ItemManager.Instance;

        doorManager.OnDoorLocked += SceneEventManager_OnDoorLocked;
        doorManager.OnDoorUnlocked += SceneEventManager_OnDoorUnlocked;
        roomManager.OnRoomCompleted += SceneEventManager_OnRoomCompleted;
    }


    private void OnDestroy()
    {
        doorManager.OnDoorLocked -= SceneEventManager_OnDoorLocked;
        roomManager.OnRoomCompleted -= SceneEventManager_OnRoomCompleted;
    }

    #endregion

    #region 订阅后执行方法

    // Door关闭了，获得关闭的Door
    private void SceneEventManager_OnDoorLocked(object door, EventArgs e)
    {
        Door doorObject = door as Door;
        // 参数转化成功！
        if (doorObject != null)
        {
            Logger.Log("Door locked: " + doorObject.doorID);
        }
        else
        {
            Logger.LogError("Failed to convert to Door type");
        }
    }

    // Door打开了，获得打开的Door
    private void SceneEventManager_OnDoorUnlocked(object door, EventArgs e)
    {
        Door doorObject = door as Door;
        if (doorObject != null)
        {
            Logger.Log("Door unlocked: " + doorObject.doorID);
        }
        else
        {
            Logger.LogError("Failed to convert to Door type");
        }
    }

    // Room完成了，获得完成的Room
    private void SceneEventManager_OnRoomCompleted(object room, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion

}