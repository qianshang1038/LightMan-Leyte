using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightDialogBtn : BaseDialogBtn
{
    public override void DialogInteract(BaseMonster2D monster)
    {
        //�����Ի����¼�
        monster.BeginDialog(BaseMonster2D.DialogType.right);
    }
}
