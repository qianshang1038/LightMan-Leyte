using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����è
public class MutantCat : BaseMonster2D
{
    public int attackValue=10;
    public override string MonsterInfo()
    {
        return base.MonsterInfo();
    }

    public override void MonsterSkill()
    {
        base.MonsterSkill();
        //����sanֵ�ж�
        if(AttackManager.Instance.HitDecided(hitRate)){
            //TODO ������ض�����
            Player2D.Instance.TakeDamage(attackValue);
            Player2D.Instance.GetComponentInChildren<PlayerHpSlider>().ReduceHealth(attackValue);
            
        }
        else
        {
            Logger.Log(name + " attack falied");
        }
        
    }

    public override string SetPlotJsonName()
    {
        return base.SetPlotJsonName();
    }
}
