using System.Collections;
using UnityEngine;

public class PlotLoader : MonoBehaviour
{
    private Plot currentPlot;
    private bool isPlaying = false;

    // 从指定Json加载Plot并且播放
    public void LoadPlot(string plotJsonName)
    {
        currentPlot = new Plot(plotJsonName);
    }

    // 如果有读取到剧情就开始播放
    public void PlayCurrentPlot()
    {
        currentPlot?.PlayDialogue();
    }
    public void PlayCurrentPlot(int id)
    {
        currentPlot?.PlayDialogue(id);
    }

    //自动跳
    public void PlayPlotThroughNodeID(int id)
    {
        if (isPlaying) return; // 防止多次按下按钮

        isPlaying = true;
        currentPlot?.PlayDialogue(id);
        CoroutineManager.Instance.AddTask(wait());
    }

    private IEnumerator wait()
    {
        while (currentPlot.GetCurrentNodeId() != -1)
        {
            yield return new WaitForSeconds(1f);
            currentPlot?.PlayDialogue();
        }
        isPlaying = false;
        CoroutineManager.Instance.Finish();
    }

    public void JumpToNode(int id)
    {
        currentPlot?.JumpToNode(id);
    }
}
