using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.ExamplePanel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���·�ʹ�öԻ�ϵͳ���жԻ�ʱ�����õ����
/// </summary>
public class DialoguePanel : BasePanel<DialoguePanel>
{
    // ����˵���ߣ�˵����ͷ�񣬶Ի��ı���
    private TextMeshProUGUI speakerName;
    private TextMeshProUGUI plotText;
    private Image profilePhoto;

    // �������յ��Ĳ���

    // �Ի����� 0�޶Ի� 1�԰� 2����Ի�
    private string speakType;

    public override void Init()
    {
        base.Init();
        // ��ʼ���ı����
        profilePhoto = GetControl<Image>("ProfilePhoto");
        speakerName = GetControl<TextMeshProUGUI>("SpeakerName");
        plotText = GetControl<TextMeshProUGUI>("PlotText");

        // �������Ƿ�ɹ���ʼ��
        if (profilePhoto == null)
        {
            Debug.LogError("ProfilePhoto component is not assigned or not found.");
        }
        if (speakerName == null)
        {
            Debug.LogError("SpeakerName component is not assigned or not found.");
        }
        if (plotText == null)
        {
            Debug.LogError("PlotText component is not assigned or not found.");
        }
    }


    /// <summary>
    /// ����Ի����ͣ�ͷ������˵�������ƣ���˵���ı�������Ի����Ͳ�������Ի��ͻ����ͷ��
    /// </summary>
    /// <param name="speakType"></param>
    /// <param name="profilePhotoName"></param>
    /// <param name="speakerName"></param>
    /// <param name="plotText"></param>
    public void ShowDialogue(int speakType, string profilePhotoName, string speakerName, string plotText)
    {
        // ������ı�����,�Զ�ƥ���Ӧ��ͷ�񣬵���Ҫ��ͷ�������������Ҳ֧��ʹ���ض�ͷ��
        if(speakType == (int)Plot.SpeakType.Dialogue)
        {
            if(profilePhotoName == "" || profilePhotoName == null)
            {
                string tempProfilePhotoNameAutoMatch = speakerName + "_Normal";
                //Debug.Log(tempProfilePhotoNameAutoMatch);
                profilePhoto.sprite = Resources.Load<Sprite>($"ProfilePhotos/{tempProfilePhotoNameAutoMatch}");
            }
            else
            {
                profilePhoto.sprite = Resources.Load<Sprite>(profilePhotoName);
            }
            if(profilePhoto != null)
            {
                ShowProfilePhoto();
            }
            else
            {
                HideProfilePhoto(); 
            }
        }
        else
        {
            HideProfilePhoto();
        }
        this.speakerName.text = speakerName;
        this.plotText.text = plotText;
        ShowMe();
    }

    private void ShowProfilePhoto()
    {
        Color currentColor = profilePhoto.color;
        currentColor.a = 1; // ����Ϊ��ȫ��͸��
        profilePhoto.color = currentColor;
    }

    private void HideProfilePhoto()
    {
        // ��ȡ��ǰ��ɫ
        Color currentColor = profilePhoto.color;
        // ����һ���µ���ɫ�������������Բ��䣬ֻ�޸� alpha
        currentColor.a = 0;
        // ���µ���ɫ��ֵ��ȥ
        profilePhoto.color = currentColor;
    }

}
