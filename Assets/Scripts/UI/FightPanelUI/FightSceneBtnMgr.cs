using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightSceneBtnMgr : MonoBehaviour
{
    public Button[] buttons;
    public Color highlightColor = Color.yellow; 
    public Color normalColor = Color.white;     
    private int currentIndex = 0;

    private bool activeOrNot=false;

    void Start()
    {
        // 初始化按钮颜色，默认第一个高亮
        HighlightButton(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveToNextButton();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveToLastButton();
        }

        if(Input.GetKeyDown (KeyCode.E))
        {
            buttons[currentIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    public void BtnInteract()
    {
        activeOrNot = !activeOrNot;
        gameObject.SetActive(activeOrNot);
        RideSideUIMgr.Instance.MakeOtherButtonsNotEnabled(transform.parent.GetComponent<Button>());
    }

    private void MoveToNextButton()
    {
        UnhighlightButton(currentIndex);
        currentIndex = (currentIndex + 1) % buttons.Length;
        HighlightButton(currentIndex);
    }

    private void MoveToLastButton()
    {
        UnhighlightButton(currentIndex);
        currentIndex = (currentIndex - 1+buttons.Length) % buttons.Length;
        HighlightButton(currentIndex);
    }

    private void HighlightButton(int index)
    {
        var buttonImage = buttons[index].GetComponent<Image>();
        buttonImage.color = highlightColor;
        switch(index)
        {
            case 0:
                SkillPlotMgr.Instance.ExposurePlot();
                break;
            case 1:
                SkillPlotMgr.Instance.RefractionPlot();
                break;
            case 2:SkillPlotMgr.Instance.StroboscopicPlot();
                break;


        }
    }

    private void UnhighlightButton(int index)
    {
        var buttonImage = buttons[index].GetComponent<Image>();
        buttonImage.color = normalColor;
    }
}
