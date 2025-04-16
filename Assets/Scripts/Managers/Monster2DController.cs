using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2DController : Singleton<Monster2DController>
{
    public IEnumerator Play()
    {
        if (MonsterMgr.Instance.IsMonsterFail())
        {
            CoroutineManager.Instance.Finish();//����ֱ�ӽ�����
        }
        TurnPlotMgr.Instance.MonsterTurnStart();
        Debug.Log("�����ж�");
        yield return new WaitForSeconds(1f);

        yield return AITurn();

        yield return PlayOver();
        CoroutineManager.Instance.Finish();
    }

    private IEnumerator PlayOver()
    {
        
        TurnController.Instance.DetectFightState();
        if(Player2D.Instance.IsAlive())
        {
            TurnController.Instance.PlayerTurn();
        }
        
        yield return null;
    }

    public IEnumerator AITurn()
    {
        //MonsterMgr.Instance.MakeMonsterDoSkill();

        yield return MonsterMgr.Instance.MakeMonsterDoSkill();

    }
}
