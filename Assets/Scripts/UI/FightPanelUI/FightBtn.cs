using System.Collections;
using System.Collections.Generic;
using UI.ExamplePanel;
using UnityEngine;
/// <summary>
/// 战斗场景进入接口
/// </summary>
public class FightBtn : Singleton<FightBtn>
{
   public void FightStart(string monsterName,int num)
    {
        FightScenePanel.Instance.InitializeMonsters(monsterName,num);
        FightScenePanel.Instance.ShowMe();
        //人物行动停止
        GameInput.Instance.SetInput(false);
        //怪物行动停止
        Monster3DMgr.Instance.SetEnemyMoveable(false);
    }
    //怪物的信息未传入
    public void FightStart()
    {
        FightScenePanel.Instance.ShowMe();
    }

    /// <summary>
    /// 战斗结束记得调
    /// </summary>
    public void FightEnd()
    {
        GamePanel.Instance.ShowMe();
        FightScenePanel.Instance.HideMe();
        GameInput.Instance.SetInput(true);
        Monster3DMgr.Instance.SetEnemyMoveable(true);
    }
}
