using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_HighExposure : BaseSkillBtn
{
    //大照射-眩晕敌人一回合
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
            Logger.Log($"大照射!-{MonsterMgr.Instance.GetCurrentMonster().name}眩晕");
            //切换指定monster状态为眩晕
            MonsterMgr.Instance.GetCurrentMonster().state = 1;
            Player2DController.Instance.PlayOver();
            transform.parent.parent.GetComponent<Button>().onClick?.Invoke();
        }
            
        

    }
}
