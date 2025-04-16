using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterClick : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        print("click "+ this.gameObject.name);
        MonsterMgr.Instance.MoveToChooseMonster(this.GetComponent<BaseMonster2D>().index);
    }
}
