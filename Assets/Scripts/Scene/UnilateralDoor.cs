using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using scene;

public class UnilateralDoor : Door
{
    Vector3 doorToPlayer;

    [SerializeField]
    public OpenableDirection openableDirection;

    public enum OpenableDirection
    {
        Up, Down, Left, Right
    }

    protected override bool TryUnlock()
    {
        doorToPlayer = Player.Instance.transform.position - gameObject.transform.position;
        
        // 判断是否可以从这一侧打开
        if (!JudgeOpenable(doorToPlayer))
        {
            return false;
        }

        // 判断其他

        return base.TryUnlock();
    }

    public override void InteractInfo(bool b)
    {
        base.InteractInfo(b);
    }

    private bool JudgeOpenable(Vector3 doorToPlayer)
    {
        bool openable = false;
        switch (openableDirection)
        {
            case OpenableDirection.Up:
                if (doorToPlayer.z > 0)
                {
                    openable = true;
                }
                break;
            case OpenableDirection.Down:
                if (doorToPlayer.z < 0)
                {
                    openable = true;
                }
                break;
            case OpenableDirection.Left:
                if (doorToPlayer.x < 0)
                {
                    openable = true;
                }
                break;
            case OpenableDirection.Right:
                if (doorToPlayer.x > 0)
                {
                    openable = true;
                }
                break; 
        }
        if (openable == false) Logger.Log($"不能从 {doorToPlayer} 一侧打开");
        return openable;
    }
}
