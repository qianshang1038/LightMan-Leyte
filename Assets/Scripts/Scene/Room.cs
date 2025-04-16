using UnityEngine;

// Room������״̬ ���˵�һ�ֶ�����Unlocked
/// <summary>
/// Ĭ��ʹ��Locked/Unlocked������״̬��ʾ��������Roomû���κ�ʣ���ֵ���ſɱ��ΪCompleted
/// </summary>
public enum RoomState
{
    Locked,
    Unlocked,
    Completed
}

public class Room : MonoBehaviour
{
    #region ��������ֶ�

    public RoomState roomState = RoomState.Unlocked;
    
    // roomID������RoomManager��ͳһ������Ϊ��ʶ��
    public int roomID = 0;

    #endregion

    #region �ⲿ����ʹ�õķ���

    // ��ȡ��ǰroom״̬
    public int GetRoomState()
    {
        Logger.Log("Current room's state is: " + roomState);
        return (int)roomState;
    }

    // �ı䵱ǰroom״̬
    public void ChangeRoomState(RoomState targetState)
    {
        roomState = targetState;
        Logger.Log("Current room's state changed to: " + targetState);
    }

    // ��鵱ǰ�Ƿ�Ϊ����״̬
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
