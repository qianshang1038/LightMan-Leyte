using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 显示文本
/// </summary>
public class ShowPaper : RoomPropHas
{
    private enum PaperType
    {
        DoctorPaper_up,
    }

    [SerializeField]
    private PaperType type;

    /// <summary>
    /// 重定义
    /// </summary>
    public override void Interact()
    {
        switch (type)
        {
            case PaperType.DoctorPaper_up:
                PaperPanel.Instance.SetPaperList(PaperPanel.Instance.DoctorPapers);
                break;
        }
    }
}
