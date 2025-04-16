using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_HighExposure : BaseSkillBtn
{
    //������-ѣ�ε���һ�غ�
    public override void BtnInteract()
    {
        if (Player2D.Instance.eletric - 30 < 0)
        {
            Logger.Log("No eletric");
        }
        else
        {
            Player2D.Instance.LoseEletricQuantity(20);
            Player2D.Instance.gameObject.GetComponentInChildren<PlayerElectricSlider>().ReduceEletric(20);
            Logger.Log($"������!-{MonsterMgr.Instance.GetCurrentMonster().name}ѣ��");
            //�л�ָ��monster״̬Ϊѣ��
            MonsterMgr.Instance.GetCurrentMonster().state = 1;
            Player2DController.Instance.PlayOver();
            transform.parent.parent.GetComponent<Button>().onClick?.Invoke();
        }
            
        

    }
}
