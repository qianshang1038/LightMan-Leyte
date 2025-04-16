using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseDialogBtn : MonoBehaviour
{
    //跟物体的btn绑定
    public void BtnInteract()
    {
        Debug.Log("dialog");

        //先get到怪物名 触发相应对话
        BaseMonster2D monster =MonsterMgr.Instance.GetCurrentMonsterObject().GetComponent<BaseMonster2D>();
        DialogInteract(monster);

        Player2DController.Instance.PlayOver();
        transform.parent.parent.GetComponent<Button>().onClick?.Invoke();
    }

    public abstract void DialogInteract(BaseMonster2D monster);
}
