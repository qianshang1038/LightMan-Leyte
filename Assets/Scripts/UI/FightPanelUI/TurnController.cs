using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 调用 PlayerController 与 Monster2DController
/// </summary>
public class TurnController : Singleton<TurnController>
{
    /// <summary>
    /// 是否是玩家回合
    /// </summary>
    [HideInInspector] public int DiceNum=-1;
    [HideInInspector] public int playerDiceNum=-1;
    [HideInInspector] public int monsterDiceNum=-1;
    [HideInInspector] public bool isTurn =false;
    [HideInInspector] public int turnIndex = 0;
    private void FixedUpdate()
    {
        //后面可以加协程管理动画
        if (isTurn)
        {
            RideSideUIMgr.Instance.SetIsEnabled(true);
        }
        else
        {
            RideSideUIMgr.Instance.SetIsEnabled(false);
        }
           
    }
    private void Start()
    {
        ResetNum();

    }

    public void PlayerTurn()
    {
        isTurn = true;  
        Player2DController.Instance.Play();
    }

    public void EnemyTurn()
    {
        isTurn = false;
        CoroutineManager.Instance.AddTask(Monster2DController.Instance.Play());
    }

    /// <summary>
    ///回合结束时调用
    /// </summary>
    public void TurnEnd()
    {
        turnIndex++;

    }
    #region 骰子相关
    public bool isSameDice()
    {
        if(playerDiceNum!=-1&&monsterDiceNum!=-1)
        return playerDiceNum == monsterDiceNum;
        else return false;
    }
    public bool isPlayerTrun()
    {
        return playerDiceNum > monsterDiceNum;
    }
    public void ResetNum()
    {
        playerDiceNum = -1;
        monsterDiceNum = -1;
    }
    public bool isValidDice()
    {
        if (playerDiceNum != -1 && monsterDiceNum != -1)
            return playerDiceNum != monsterDiceNum;
        else return false;
    }
    #endregion
    /// <summary>
    ///每次回合结束检测战斗是否已经胜利或者失败
    /// </summary>
    public void DetectFightState()
    {
        //TurnIndex
        TurnController.Instance.TurnEnd();
        if (Player2D.Instance.hp <= 0)
        {
            Logger.Log("玩家失败");
            FightFinishBtn.Instance.FightDefeat();
        }
        else if (MonsterMgr.Instance.IsMonsterFail())
        {
            Logger.Log("玩家胜利");
            FightFinishBtn.Instance.FightWin();
        }
    }

}

