using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlotMgr : Singleton<TurnPlotMgr>
{
    PlotLoader plotLoader;
    private string plotJsonName = "TurnInformation";
    private void Start()
    {
        plotLoader = GetComponent<PlotLoader>();
        plotLoader.LoadPlot(plotJsonName);
    }
    //����ս��
    public void MeetEnemy()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(6);
    }
    #region �غ���Ϣ
    public void PlayerTurnStart()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(0);

    }

    public void MonsterTurnStart()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(1);

    }
    #endregion
    
    #region �������
    //�����Ӿ����Ⱥ���
    public void ThrowOffensiveDice()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(3);
    }
    //�����Ӿ����˺�
    public void ThrowDamageDice()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(2);
    }
    
    public void PlayerRoll()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(4);
    }
    public void MonsterRoll()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(5);
    }
    public void ReRoll()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(7);
    }
    #endregion
}
