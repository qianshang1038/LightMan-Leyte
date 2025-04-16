using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPlotMgr : Singleton<SkillPlotMgr>
{
    PlotLoader plotLoader;
    private string plotJsonName = "SkillPlotInformation";
    private void Start()
    {
        plotLoader = GetComponent<PlotLoader>();
        plotLoader.LoadPlot(plotJsonName);
    }
    /// <summary>
    /// ��������Ϣ
    /// </summary>
    public void ExposurePlot()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(0);
    }
    /// <summary>
    /// �ƹ�������Ϣ
    /// </summary>
    public void RefractionPlot()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(1);
    }
    /// <summary>
    /// �ƹ�Ƶ����Ϣ
    /// </summary>
    public void StroboscopicPlot()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(2);
    }
}
