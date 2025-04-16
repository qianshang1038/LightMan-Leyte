using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//普通攻击
public class AttackBtn : MonoBehaviour
{
    
    private BaseMonster2D monster;
    private int attackValue;
    public virtual void BtnInteract()
    {
        //触发效果
        Debug.Log("attack" + MonsterMgr.Instance.GetCurrentMonsterObject().name);
        TurnPlotMgr.Instance.ThrowDamageDice();
        AttackHandle();
        Player2DController.Instance.PlayOver();
    }
    private void AttackHandle()
    {
        
        //命中处理
        bool isHit = AttackManager.Instance.HitDecided(Player2D.Instance.hitRate);
        if (isHit)
        {
            //投骰子
            CoroutineManager.Instance.AddTask( AttackManager.Instance.RollAttackValue());
            
            CoroutineManager.Instance.AddTask(HandleDamage());

        }
        else
        {
           //未命中
        }
    }
    //处理伤害
    public IEnumerator HandleDamage()
    {
        attackValue = AttackManager.Instance.GetAttackValue();
        Logger.Log($"Monster should get damage {attackValue}");
        yield return new WaitForSeconds(1f);
        MonsterMgr.Instance.GetCurrentMonster().TakeDamage(attackValue);
        MonsterMgr.Instance.GetCurrentMonster().gameObject.GetComponentInChildren<MonsterHpSlider>().ReduceHealth(attackValue);
        
        yield return new WaitForSeconds(1f);
        CoroutineManager.Instance.Finish();
    }
}
