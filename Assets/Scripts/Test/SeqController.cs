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
    private bool isStartRolled=true;//启动过了没有
    public event Action ShouldStartFight;
    void Start()
    {
        //Logger.Log("Start rolling to decide offensive");
        monsterFinsihed = false;
       
    }

    void Update()
    {
        //当seqController想要触发骰子先后手功能,需要调用SetStartRolled()
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
            //重置骰子投掷次数计数器
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
    //这个是先后手控制的开关想开启就使用它
    public void SetStartRolled(bool _isStartRolled)
    {
        isStartRolled = _isStartRolled;
    }
}
