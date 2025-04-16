using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeqController :MonoBehaviour 
{
    public GameObject dice;
    public DiceListener diceListener;  
    public GameObject FightCanvas;
    [HideInInspector] public bool monsterFinsihed;
    [HideInInspector] public bool isSeqFinished=true;
    private bool isStartRolled=true;//��������û��
    public event Action ShouldStartFight;
    void Start()
    {
        //Logger.Log("Start rolling to decide offensive");
        monsterFinsihed = false;
       
    }

    void Update()
    {
        //��seqController��Ҫ���������Ⱥ��ֹ���,��Ҫ����SetStartRolled()
        if (!isStartRolled)
        {
            StarRoll();
            isSeqFinished =false;
            isStartRolled = true;
        }

        if (diceListener.times == 2&& !isSeqFinished)
        {
            //Logger.Log("monster's turn to roll");
            TurnPlotMgr.Instance.MonsterRoll();
            diceListener.roller.AutoRoll();
            
        }

        if (TurnController.Instance.isSameDice() && monsterFinsihed&&!isSeqFinished)
        {
            TurnPlotMgr.Instance.ReRoll();
            //��������Ͷ������������
            diceListener.TimesReset();
            StarRoll();
        }
        if (IsRollFinished()&& !isSeqFinished)
        {
             EndRoll();
            
        }
        
    }
    private void StarRoll()
    {
        TurnController.Instance.ResetNum();
        //Logger.Log("Player's turn to roll");
        TurnPlotMgr.Instance.PlayerRoll();
        dice.SetActive(true);

    }
  
    private void EndRoll()
    {
        Logger.Log("End Roll");
        dice.SetActive(false);
        ShouldStartFight?.Invoke();
        //FightCanvas.SetActive(true);
        isSeqFinished =true;
    }
    private bool IsRollFinished()
    {
        return TurnController.Instance.isValidDice() && monsterFinsihed;

    }
    //������Ⱥ��ֿ��ƵĿ����뿪����ʹ����
    public void SetStartRolled(bool _isStartRolled)
    {
        isStartRolled = _isStartRolled;
    }
}
