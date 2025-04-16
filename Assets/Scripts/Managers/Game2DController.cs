using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2DController : Singleton<Game2DController>
{
    private bool offensive = true;
    /// <summary>
    /// 战斗开始的接口 先调用SetOffensive决定哪个先手再调用此方法
    /// 否则默认玩家先手
    /// </summary>
    public void StartFight()
    {
       /* //投骰子决定先后手
        GameObject seq = GameObject.Find("DiceManager");
        if (seq != null)
        {
            seqController = seq.GetComponent<SeqController>();
        }
        else
        {
            Logger.LogError("seq is null");
        }
        seqController.SetStartRolled();
*/
        if (offensive)
        {
            TurnController.Instance.PlayerTurn();
        }
        else
        {
            //CoroutineManager.Instance.AddTask(Monster2DController.Instance.Play());
            TurnController.Instance.EnemyTurn();
        }
    }

    /// <summary>
    /// 回合结束调用
    /// </summary>
    /// <returns></returns>
    public IEnumerator Turn()
    {
        //TODO
        yield return null;
    }

    /// <summary>
    /// 决定谁是先手 
    /// </summary>
    /// <param name="b">true 玩家先 false 怪物先</param>
    public void Setoffensive(bool b)
    {
        offensive = b;
    }
}
