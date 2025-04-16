using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ͨ����
public class AttackBtn : MonoBehaviour
{
    
    private BaseMonster2D monster;
    private int attackValue;
    public virtual void BtnInteract()
    {
        //����Ч��
        Debug.Log("attack" + MonsterMgr.Instance.GetCurrentMonsterObject().name);
        TurnPlotMgr.Instance.ThrowDamageDice();
        AttackHandle();
        Player2DController.Instance.PlayOver();
    }
    private void AttackHandle()
    {
        
        //���д���
        bool isHit = AttackManager.Instance.HitDecided(Player2D.Instance.hitRate);
        if (isHit)
        {
            //Ͷ����
            CoroutineManager.Instance.AddTask( AttackManager.Instance.RollAttackValue());
            
            CoroutineManager.Instance.AddTask(HandleDamage());

        }
        else
        {
           //δ����
        }
    }
    //�����˺�
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
