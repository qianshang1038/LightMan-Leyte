using System.Collections;
using System.Collections.Generic;
using UI.ExamplePanel;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            GameOverPanel.Instance.ShowMe();
            //TODO Ȼ�󽫻���ĳ���Ϸʤ�� 
        }
    }
}
