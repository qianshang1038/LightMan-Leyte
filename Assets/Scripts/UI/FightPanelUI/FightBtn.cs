using System.Collections;
using System.Collections.Generic;
using UI.ExamplePanel;
using UnityEngine;
/// <summary>
/// ս����������ӿ�
/// </summary>
public class FightBtn : Singleton<FightBtn>
{
   public void FightStart(string monsterName,int num)
    {
        FightScenePanel.Instance.InitializeMonsters(monsterName,num);
        FightScenePanel.Instance.ShowMe();
        //�����ж�ֹͣ
        GameInput.Instance.SetInput(false);
        //�����ж�ֹͣ
        Monster3DMgr.Instance.SetEnemyMoveable(false);
    }
    //�������Ϣδ����
    public void FightStart()
    {
        FightScenePanel.Instance.ShowMe();
    }

    /// <summary>
    /// ս�������ǵõ�
    /// </summary>
    public void FightEnd()
    {
        GamePanel.Instance.ShowMe();
        FightScenePanel.Instance.HideMe();
        GameInput.Instance.SetInput(true);
        Monster3DMgr.Instance.SetEnemyMoveable(true);
    }
}
