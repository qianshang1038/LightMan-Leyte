using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� PlayerController �� Monster2DController
/// </summary>
public class TurnController : Singleton<TurnController>
{
    /// <summary>
    /// �Ƿ�����һغ�
    /// </summary>
    [HideInInspector] public int DiceNum=-1;
    [HideInInspector] public int playerDiceNum=-1;
    [HideInInspector] public int monsterDiceNum=-1;
    [HideInInspector] public bool isTurn =false;
    [HideInInspector] public int turnIndex = 0;
    private void FixedUpdate()
    {
        //������Լ�Э�̹�����
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
    ///�غϽ���ʱ����
    /// </summary>
    public void TurnEnd()
    {
        turnIndex++;

    }
    #region �������
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
    ///ÿ�λغϽ������ս���Ƿ��Ѿ�ʤ������ʧ��
    /// </summary>
    public void DetectFightState()
    {
        //TurnIndex
        TurnController.Instance.TurnEnd();
        if (Player2D.Instance.hp <= 0)
        {
            Logger.Log("���ʧ��");
            FightFinishBtn.Instance.FightDefeat();
        }
        else if (MonsterMgr.Instance.IsMonsterFail())
        {
            Logger.Log("���ʤ��");
            FightFinishBtn.Instance.FightWin();
        }
    }

}

