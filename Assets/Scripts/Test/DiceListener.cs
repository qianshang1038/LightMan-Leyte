using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceListener : MonoBehaviour
{
    public DiceRoller roller;
    public SeqController seqController;
    [HideInInspector]public int times = 1;

    private void Awake()
    {
        times = 1;
    }
    private void Start()
    {
        if (roller != null)
        {
            roller.DiceFinishCalled += OnRollerCalled;
        }
    }
    private void OnRollerCalled()
    {

        if(times > 2)
        {
            //SetMonsterFinisheRoll();
            times = 1;
        }

        TransDiceNum();

        Logger.Log($"Player Dice number is {TurnController.Instance.playerDiceNum} and monste Dice number is {TurnController.Instance.monsterDiceNum}");
        if (TurnController.Instance.isValidDice())
        {
            if (seqController != null)
            {
                SetMonsterFinisheRoll();
            }
           
        }

        times++;

    }
    private void OnDestroy()
    {
        if (roller == null)
        {
            roller.DiceFinishCalled -= OnRollerCalled;
        }
    }
    //传递骰子参数到TurnController
    private void TransDiceNum()
    {
        if(times == 1)
        {
            TurnController.Instance.playerDiceNum = TurnController.Instance.DiceNum;
        }
        if (times == 2)
        {
            TurnController.Instance.monsterDiceNum = TurnController.Instance.DiceNum;
        }
    }
    private void SetMonsterFinisheRoll()
    {
        seqController.monsterFinsihed=true;
    }
    public void TimesReset()
    {
        times=1;
    }
}
