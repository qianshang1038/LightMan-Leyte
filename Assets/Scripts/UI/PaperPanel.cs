using System.Collections.Generic;
using UI;
using UI.ExamplePanel;
using UnityEngine;
using UnityEngine.UI;

public class PaperPanel : BasePanel<PaperPanel>
{
    
    public Image paperImage;
    public List<Sprite> DoctorPapers;
    public List<Sprite> Map;

    private List<Sprite> currentPapers; 
    private int currentIndex = 0;

    public override void Init()
    {
        base.Init();
    }

    void Start()
    {
        // 初始化使用 DoctorPapers
        //SetPaperList(DoctorPapers);
    }

    public void ShowNextPaper()
    {
        if (currentPapers == null || currentPapers.Count == 0) return;
        currentIndex = (currentIndex + 1) % currentPapers.Count;
        UpdatePaperImage();
    }

    public void ShowPreviousPaper()
    {
        if (currentPapers == null || currentPapers.Count == 0) return;

        currentIndex = (currentIndex - 1 + currentPapers.Count) % currentPapers.Count; 
        UpdatePaperImage();
    }

    /// <summary>
    /// 要显示文本就调用这个方法
    /// </summary>
    /// <param name="newPaperList"></param>
    public void SetPaperList(List<Sprite> newPaperList)
    {
        ShowMe();
        currentPapers = newPaperList;
        currentIndex = 0; // 重置为第一个 Sprite
        UpdatePaperImage();
    }

    private void UpdatePaperImage()
    {
        paperImage.sprite = currentPapers[currentIndex];
    }
}
