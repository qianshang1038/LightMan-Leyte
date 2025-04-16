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
        // ��ʼ��ʹ�� DoctorPapers
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
    /// Ҫ��ʾ�ı��͵����������
    /// </summary>
    /// <param name="newPaperList"></param>
    public void SetPaperList(List<Sprite> newPaperList)
    {
        ShowMe();
        currentPapers = newPaperList;
        currentIndex = 0; // ����Ϊ��һ�� Sprite
        UpdatePaperImage();
    }

    private void UpdatePaperImage()
    {
        paperImage.sprite = currentPapers[currentIndex];
    }
}
