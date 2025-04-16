using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʾ�ı�
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
    /// �ض���
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
