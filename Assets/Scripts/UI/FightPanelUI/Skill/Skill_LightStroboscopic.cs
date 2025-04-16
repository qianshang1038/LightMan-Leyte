using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_LightStroboscopic : BaseSkillBtn
{
    public override void BtnInteract()
    {
        base.BtnInteract();
        Player2D.Instance.LoseEletricQuantity(10);
        Player2D.Instance.gameObject.GetComponentInChildren<PlayerElectricSlider>().ReduceEletric(20);
        Logger.Log($"�ƹ�Ƶ����{MonsterMgr.Instance.GetCurrentMonster().name}�������½���50%");
        MonsterMgr.Instance.GetCurrentMonster().LoseHitRate(0.5f);
    }
}
