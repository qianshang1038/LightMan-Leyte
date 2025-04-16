using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseDialogBtn : MonoBehaviour
{
    //�������btn��
    public void BtnInteract()
    {
        Debug.Log("dialog");

        //��get�������� ������Ӧ�Ի�
        BaseMonster2D monster =MonsterMgr.Instance.GetCurrentMonsterObject().GetComponent<BaseMonster2D>();
        DialogInteract(monster);

        Player2DController.Instance.PlayOver();
        transform.parent.parent.GetComponent<Button>().onClick?.Invoke();
    }

    public abstract void DialogInteract(BaseMonster2D monster);
}
