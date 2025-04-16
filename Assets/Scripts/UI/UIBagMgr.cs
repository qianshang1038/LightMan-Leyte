using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIBagMgr : Singleton<UIBagMgr>
{
    public List<Button> buttons; // �洢���а�ť

    void Start()
    {
        // ȷ�����а�ť��ʼ״̬��û��ͼ��
        foreach (var button in buttons)
        {
            ClearButtonIcon(button);
        }
    }

    #region �ڲ���Ҫ����

    // ���ô˷���������ͼ�꣬�ҵ���һ����ͼ������
    private void GenerateIcon(Sprite se)
    {
        foreach (var button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();

            // ����Ƿ�������ͬ��ͼ��
            if (buttonImage.sprite != null && buttonImage.sprite.name == se.name)
            {
                // ��������
                IncreaseIconCount(button);
                return;
            }
            // ���ͼ��Ϊ�գ�������ͼ��
        if (buttonImage.sprite == null)
        {
            SetButtonIcon(button, se);
            SetIconCount(button, 1);  // ��ʼ������Ϊ1
            break;
        }
        }
        
    }

    // ���ð�ťͼ��
    private void SetButtonIcon(Button button, Sprite icon)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = icon;
        }
    }

    // �����ťͼ��
    private void ClearButtonIcon(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = null;
        }

        SetIconCount(button, 0); // �������
    }

    // ����ͼ������
    private void SetIconCount(Button button, int count)
    {
        TextMeshProUGUI countText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (countText != null)
        {
            countText.text = count > 1 ? count.ToString() : ""; // ����������1ʱ����ʾ
        }
    }

    // ����ͼ������
    private void IncreaseIconCount(Button button)
    {
        TextMeshProUGUI countText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (countText != null)
        {
            int currentCount = int.Parse(countText.text == "" ? "1" : countText.text);
            countText.text = (currentCount + 1).ToString();
        }
    }

    // ���º���İ�ťͼ��
    private void UpdateButtonIcons()
    {
        for (int i = 0; i < buttons.Count - 1; i++)
        {
            Image currentImage = buttons[i].GetComponent<Image>();
            Image nextImage = buttons[i + 1].GetComponent<Image>();
            if (currentImage != null && currentImage.sprite == null && nextImage != null && nextImage.sprite != null)
            {
                currentImage.sprite = nextImage.sprite;
                // ͬʱ��������
                SetIconCount(buttons[i], GetIconCount(buttons[i + 1]));
                // �����һ����ť��ͼ��
                ClearButtonIcon(buttons[i + 1]);
            }
        }
    }

    // ��ȡͼ������
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

    #region �ⲿ�ɵ��÷���

    // �����Ƿ���ָ�����Ƶ�ͼ��
    public bool UseItemByName(string itemName)
    {
        foreach (var button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null && buttonImage.sprite != null && buttonImage.sprite.name == itemName)
            {
                // �����������1�����������
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

    // �ⲿ����UI��ʾ�Ľӿ�
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
