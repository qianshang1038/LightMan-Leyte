using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightFinishBtn : Singleton<FightFinishBtn>
{
    /// <summary>
    /// ս��ʤ��
    /// </summary>
    public void FightWin()
    {
        //�鿴�Ƿ�ɹ�

        GameInput.Instance.SetInput(true);
        FightBtn.Instance.FightEnd();
        //TODO ս��ʤ����

    }
    /// <summary>
    /// ս��ʧ��
    /// </summary>
    public void FightDefeat()
    {
        GameInput.Instance.SetInput(true);
        FightBtn.Instance.FightEnd();
        //TODO ս��ʧ�ܺ�ص�����
    }


}
