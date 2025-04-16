using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightMan : BaseMonster2D
{
    private int attackValue = 10;
    public override string MonsterInfo()
    {
        return base.MonsterInfo();
    }

    public override void MonsterSkill()
    {
        base.MonsterSkill();
        if (AttackManager.Instance.HitDecided(hitRate))
        {
            //TODO 怪物的特定技能
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
