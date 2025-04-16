using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DController : Singleton<Player2DController>
{
    public void Play()
    {
        //后面加动画  TODO
        
        if (Player2D.Instance.IsAlive()&&!MonsterMgr.Instance.IsMonsterFail())
        {
            Debug.Log("玩家行动");
            TurnPlotMgr.Instance.PlayerTurnStart();
        }
        

    }
    
    /// <summary>
    /// 玩家回合结束调用
    /// </summary>
    public void PlayOver()
    {
        TurnController.Instance.DetectFightState();
        
        if(!MonsterMgr.Instance.IsMonsterFail()&&Player2D.Instance.IsAlive())
        {
            TurnController.Instance.EnemyTurn();

        }
        

        //敌人回合
    }

}
