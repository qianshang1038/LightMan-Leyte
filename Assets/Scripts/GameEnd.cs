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
            //TODO 然后将画面改成游戏胜利 
        }
    }
}
