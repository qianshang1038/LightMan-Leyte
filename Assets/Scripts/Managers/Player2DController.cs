using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DController : Singleton<Player2DController>
{
    public void Play()
    {
        //����Ӷ���  TODO
        
        if (Player2D.Instance.IsAlive()&&!MonsterMgr.Instance.IsMonsterFail())
        {
            Debug.Log("����ж�");
            TurnPlotMgr.Instance.PlayerTurnStart();
        }
        

    }
    
    /// <summary>
    /// ��һغϽ�������
    /// </summary>
    public void PlayOver()
    {
        TurnController.Instance.DetectFightState();
        
        if(!MonsterMgr.Instance.IsMonsterFail()&&Player2D.Instance.IsAlive())
        {
            TurnController.Instance.EnemyTurn();

        }
        

        //���˻غ�
    }

}
