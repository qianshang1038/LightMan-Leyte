using System.Collections;
using UnityEngine;

public class PlotLoader : MonoBehaviour
{
    private Plot currentPlot;
    private bool isPlaying = false;

    // ��ָ��Json����Plot���Ҳ���
    public void LoadPlot(string plotJsonName)
    {
        currentPlot = new Plot(plotJsonName);
    }

    // ����ж�ȡ������Ϳ�ʼ����
    public void PlayCurrentPlot()
    {
        currentPlot?.PlayDialogue();
    }
    public void PlayCurrentPlot(int id)
    {
        currentPlot?.PlayDialogue(id);
    }

    //�Զ���
    public void PlayPlotThroughNodeID(int id)
    {
        if (isPlaying) return; // ��ֹ��ΰ��°�ť

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
