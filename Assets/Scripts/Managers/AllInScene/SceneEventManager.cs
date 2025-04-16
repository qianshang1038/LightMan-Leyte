using System;
using scene;

public class SceneEventManager : Singleton<SceneEventManager>
{
    public event EventHandler OnDoorOpened;

    public RoomManager roomManager;
    public DoorManager doorManager;
    // public ItemManager itemManager;

    #region �������ں���

    private void Start()
    {
        // ʹ�õ���ģʽ��ʼ����������ȷ������״̬
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

    #region ���ĺ�ִ�з���

    // Door�ر��ˣ���ùرյ�Door
    private void SceneEventManager_OnDoorLocked(object door, EventArgs e)
    {
        Door doorObject = door as Door;
        // ����ת���ɹ���
        if (doorObject != null)
        {
            Logger.Log("Door locked: " + doorObject.doorID);
        }
        else
        {
            Logger.LogError("Failed to convert to Door type");
        }
    }

    // Door���ˣ���ô򿪵�Door
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

    // Room����ˣ������ɵ�Room
    private void SceneEventManager_OnRoomCompleted(object room, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion

}