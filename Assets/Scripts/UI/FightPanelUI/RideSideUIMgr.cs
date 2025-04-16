using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

using UnityEngine.UI;

public class RideSideUIMgr : Singleton<RideSideUIMgr>
{
    private List<Button> buttons=new List<Button>();
    private void Start()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            Button _t=transform.GetChild(i).GetComponent<Button>();
            buttons.Add(_t);
        }
    }
    public void SetIsEnabled(bool b)
    {
        for(int i=0;i<buttons.Count;i++)
        {
            buttons[i].enabled = b;
        }
    }

    public void MakeOtherButtonsNotEnabled(Button CurrentButton)
    {
        //AttackBtn ²»ÓÃ
        for (int i = 0; i < buttons.Count - 1; i++)
        {
            if (buttons[i] != CurrentButton)
            {
                if (buttons[i].transform.GetChild(1).gameObject != null)
                {
                    buttons[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}
