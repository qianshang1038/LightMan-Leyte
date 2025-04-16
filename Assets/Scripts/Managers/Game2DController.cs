using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2DController : Singleton<Game2DController>
{
    private bool offensive = true;
    /// <summary>
    /// ս����ʼ�Ľӿ� �ȵ���SetOffensive�����ĸ������ٵ��ô˷���
    /// ����Ĭ���������
    /// </summary>
    public void StartFight()
    {
       /* //Ͷ���Ӿ����Ⱥ���
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
    /// �غϽ�������
    /// </summary>
    /// <returns></returns>
    public IEnumerator Turn()
    {
        //TODO
        yield return null;
    }

    /// <summary>
    /// ����˭������ 
    /// </summary>
    /// <param name="b">true ����� false ������</param>
    public void Setoffensive(bool b)
    {
        offensive = b;
    }
}
