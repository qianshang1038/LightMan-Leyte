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
    /// 大照射信息
    /// </summary>
    public void ExposurePlot()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(0);
    }
    /// <summary>
    /// 灯光折射信息
    /// </summary>
    public void RefractionPlot()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(1);
    }
    /// <summary>
    /// 灯光频闪信息
    /// </summary>
    public void StroboscopicPlot()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayCurrentPlot(2);
    }
}
