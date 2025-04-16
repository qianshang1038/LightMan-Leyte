using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseSkillBtn : MonoBehaviour
{ 
   

    public virtual void BtnInteract()
    {
        //´¥·¢Ð§¹û

        Debug.Log("skill");

        Player2DController.Instance.PlayOver();
        transform.parent.parent.GetComponent<Button>().onClick?.Invoke();
    }
}
