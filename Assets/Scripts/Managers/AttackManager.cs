using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    public GameObject attackDice;
    public DiceListener diceListener;
    private int attackValue;
    //命中判定
    public bool HitDecided(float hitRate)
    {
        float result= RangeHit();
        float hitChance = hitRate;
        return result <= hitChance;
    }
    public float RangeHit()
    {
        float result = Random.Range(0f, 1f);
        return result;
    }
   
    //摇骰子决定AttackValue
    public IEnumerator RollAttackValue()
    {
        bool isRunning = true;
        attackDice.SetActive(true);
        Logger.Log("Roll for your AttackValue");
        while (isRunning)
        {
            if (TurnController.Instance.playerDiceNum != -1)
            {
                Logger.Log($"The dice num is {TurnController.Instance.playerDiceNum} ");
                yield return new WaitForSeconds(1f);
                SetAttackValue();
                attackDice.SetActive(false);
                isRunning = false;
                TurnController.Instance.ResetNum();
                diceListener.TimesReset();
            }
            yield return null;
        }

        CoroutineManager.Instance.Finish();
    }

    public int GetAttackValue()
    {
        return attackValue;
    }
    public void SetAttackValue()
    {
        attackValue = TurnController.Instance.playerDiceNum;
    }
}
