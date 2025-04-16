using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.ExamplePanel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 在下方使用对话系统进行对话时候所用的面板
/// </summary>
public class DialoguePanel : BasePanel<DialoguePanel>
{
    // 声明说话者，说话者头像，对话文本，
    private TextMeshProUGUI speakerName;
    private TextMeshProUGUI plotText;
    private Image profilePhoto;

    // 声明接收到的参数

    // 对话类型 0无对话 1旁白 2人物对话
    private string speakType;

    public override void Init()
    {
        base.Init();
        // 初始化文本组件
        profilePhoto = GetControl<Image>("ProfilePhoto");
        speakerName = GetControl<TextMeshProUGUI>("SpeakerName");
        plotText = GetControl<TextMeshProUGUI>("PlotText");

        // 检查组件是否成功初始化
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
    /// 传入对话类型，头像名，说话者名称，所说的文本。如果对话类型不是人物对话就会忽略头像
    /// </summary>
    /// <param name="speakType"></param>
    /// <param name="profilePhotoName"></param>
    /// <param name="speakerName"></param>
    /// <param name="plotText"></param>
    public void ShowDialogue(int speakType, string profilePhotoName, string speakerName, string plotText)
    {
        // 允许空文本传入,自动匹配对应的头像，但是要求头像符合命名规则，也支持使用特定头像
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
        currentColor.a = 1; // 设置为完全不透明
        profilePhoto.color = currentColor;
    }

    private void HideProfilePhoto()
    {
        // 获取当前颜色
        Color currentColor = profilePhoto.color;
        // 创建一个新的颜色，保持其他属性不变，只修改 alpha
        currentColor.a = 0;
        // 将新的颜色赋值回去
        profilePhoto.color = currentColor;
    }

}
