using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_LightRefraction : BaseSkillBtn
{
    int attackValue=10;
    //灯光折射-全体攻击
    public override void BtnInteract()
    {
        if (Player2D.Instance.eletric - 30 < 0)
        {
            Logger.Log("No eletric");
        }
        else
        {
            Player2D.Instance.LoseEletricQuantity(30);
            Player2D.Instance.gameObject.GetComponentInChildren<PlayerElectricSlider>().ReduceEletric(30);
            CoroutineManager.Instance.AddTask(DamegeAll());
            Player2DController.Instance.PlayOver();
            transform.parent.parent.GetComponent<Button>().onClick?.Invoke();
        }
        
    }
    public IEnumerator DamegeAll()
    {
        Logger.Log("灯光折射-全体攻击");
        yield return new WaitForSeconds(1f);
        for (int i=0; i< MonsterMgr.Instance.GetMonsters().Count; i++)
        {
            
            MonsterMgr.Instance.GetMonsters()[i].TakeDamage(attackValue);
            MonsterMgr.Instance.UpdateMonsterCount();
            MonsterMgr.Instance.GetMonsters()[i].gameObject.GetComponentInChildren<MonsterHpSlider>().ReduceHealth(attackValue);
            Logger.Log($"{MonsterMgr.Instance.GetMonsters()[i].name}受到伤害");
            
        }
        
        CoroutineManager.Instance.Finish();
    }
}
