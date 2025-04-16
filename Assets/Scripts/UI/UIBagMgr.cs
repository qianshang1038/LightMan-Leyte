using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIBagMgr : Singleton<UIBagMgr>
{
    public List<Button> buttons; // 存储所有按钮

    void Start()
    {
        // 确保所有按钮初始状态都没有图标
        foreach (var button in buttons)
        {
            ClearButtonIcon(button);
        }
    }

    #region 内部主要方法

    // 调用此方法来生成图标，找到下一个空图标生成
    private void GenerateIcon(Sprite se)
    {
        foreach (var button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();

            // 检查是否已有相同的图标
            if (buttonImage.sprite != null && buttonImage.sprite.name == se.name)
            {
                // 增加数量
                IncreaseIconCount(button);
                return;
            }
            // 如果图标为空，设置新图标
        if (buttonImage.sprite == null)
        {
            SetButtonIcon(button, se);
            SetIconCount(button, 1);  // 初始化数量为1
            break;
        }
        }
        
    }

    // 设置按钮图标
    private void SetButtonIcon(Button button, Sprite icon)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = icon;
        }
    }

    // 清除按钮图标
    private void ClearButtonIcon(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = null;
        }

        SetIconCount(button, 0); // 清除数量
    }

    // 设置图标数量
    private void SetIconCount(Button button, int count)
    {
        TextMeshProUGUI countText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (countText != null)
        {
            countText.text = count > 1 ? count.ToString() : ""; // 当数量大于1时才显示
        }
    }

    // 增加图标数量
    private void IncreaseIconCount(Button button)
    {
        TextMeshProUGUI countText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (countText != null)
        {
            int currentCount = int.Parse(countText.text == "" ? "1" : countText.text);
            countText.text = (currentCount + 1).ToString();
        }
    }

    // 更新后面的按钮图标
    private void UpdateButtonIcons()
    {
        for (int i = 0; i < buttons.Count - 1; i++)
        {
            Image currentImage = buttons[i].GetComponent<Image>();
            Image nextImage = buttons[i + 1].GetComponent<Image>();
            if (currentImage != null && currentImage.sprite == null && nextImage != null && nextImage.sprite != null)
            {
                currentImage.sprite = nextImage.sprite;
                // 同时复制数量
                SetIconCount(buttons[i], GetIconCount(buttons[i + 1]));
                // 清除下一个按钮的图标
                ClearButtonIcon(buttons[i + 1]);
            }
        }
    }

    // 获取图标数量
    private int GetIconCount(Button button)
    {
        Text countText = button.GetComponentInChildren<Text>();
        if (countText != null && countText.text != "")
        {
            return int.Parse(countText.text);
        }
        return 1;
    }

    #endregion

    #region 外部可调用方法

    // 搜索是否有指定名称的图标
    public bool UseItemByName(string itemName)
    {
        foreach (var button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null && buttonImage.sprite != null && buttonImage.sprite.name == itemName)
            {
                // 如果数量大于1，则减少数量
                int count = GetIconCount(button);
                if (count > 1)
                {
                    SetIconCount(button, count - 1);
                }
                else
                {
                    ClearButtonIcon(button);
                    UpdateButtonIcons();
                }
                return true;
            }
        }
        return false;
    }

    // 外部放入UI显示的接口
    public void SetAndGenerateSprite(Sprite sprite)
    {
        GenerateIcon(sprite);
    }

    #endregion

    public void OnButtonClick(Button button)
    {
        string icon = button.GetComponent<Image>().sprite.name;
        switch (icon)
        {
            case "battery":
                /*if(isCloseToDoctorDoor())
                {
                    ClearButtonIcon(button);
                    UpdateButtonIcons();
                }*/
                break;
            case "Map":
                PaperPanel.Instance.SetPaperList(PaperPanel.Instance.Map);
                break;
        }
    }
}
