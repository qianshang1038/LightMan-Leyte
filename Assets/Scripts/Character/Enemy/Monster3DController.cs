using System.Collections;
using System.Collections.Generic;
using UI.ExamplePanel;
using UnityEngine;

public class Monster3DController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && transform.gameObject.tag=="Monster")
        {
            GamePanel.Instance.gameObject.SetActive(false);
            FightScenePanel.Instance.ShowMe();   
        }
    }

    /// <summary>
    /// 怪物被击败时调用
    /// </summary>
    public void Defeat()
    {
        transform.gameObject.tag = "Monster_Defeat";
        Destroy(gameObject);
    }
}
