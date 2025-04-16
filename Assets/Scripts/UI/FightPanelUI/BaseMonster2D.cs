using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Plot;

[RequireComponent(typeof(PlotLoader))]
public class BaseMonster2D : MonoBehaviour
{
    PlotLoader plotLoader;
  
    public enum DialogType
    {
        right,
        middle,
        error
    }
    

    public int hp;
    public float san;
    public int maxHp;
    public GameObject pointer;
    [HideInInspector]public int index;
    public int state;//0正常,1眩晕
    public float hitRate;//命中率
    
       
    
    public void Start()
    {
        plotLoader = gameObject.GetComponent<PlotLoader>();
        hitRate = san * 0.01f;
    }
    #region 暂定一项 后面怪物继承需要重写
    public virtual void MonsterSkill()
    {
        //Damege
        Debug.Log(name + "使用技能");

    }

    public virtual string MonsterInfo() 
    {
        //test
        return "hard";
    }

    public virtual string SetPlotJsonName()
    {
        return "Monster2DDialogTest";
    }

    #endregion

  

    public void BeginDialog(DialogType dt)
    {
        string plotJsonName = SetPlotJsonName();
        plotLoader.LoadPlot(plotJsonName);
        switch (dt)
        {
            case DialogType.right:
                plotLoader.PlayPlotThroughNodeID(0);
                break;
            case DialogType.middle:
                plotLoader.PlayPlotThroughNodeID(3);
                break;
            case DialogType.error:
                plotLoader.PlayPlotThroughNodeID(4);
                break;
        }
        
    }
    //指针
    public void SetSpriteActiveOrNot(bool b)
    {
        pointer.SetActive(b);
    }

    public void Initialize(int initialHP, int initialSan)
    {
        hp = initialHP;
        san = initialSan;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp < 0)  hp = 0;
        DetectDied();
    }
    public void DetectDied()
    {
        if(!IsAlive())
        {
            Destroy(gameObject);
           MonsterMgr.Instance.UpdateMonsterCount();
        }
        
    }
    public void LoseSanityPercent(float sanityLoss)
    {
        san -= san*sanityLoss;
        if (san < 0) san = 0;
    }
    public void LoseHitRate(float hitLoss)
    {
        hitRate -= hitRate *hitLoss;
        if(hitRate <0) hitRate = 0; 
    }

    public bool IsAlive()
    {
        return hp > 0;
    }

    public bool IsInsane()
    {
        return san == 0;
    }

/*    private void Start()
    {
        plotLoader = gameObject.GetComponent<PlotLoader>();

        string plotJsonName = SetPlotJsonName();
        plotLoader.LoadPlot(plotJsonName);
        BeginDialog(DialogType.right);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            plotLoader.PlayCurrentPlot();
        }
    }*/
}
