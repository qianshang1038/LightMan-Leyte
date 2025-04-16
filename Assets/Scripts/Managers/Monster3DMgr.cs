using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster3DMgr : Singleton<Monster3DMgr>
{
    public void SetEnemyMoveable(bool b)
    {
        for(int i=0;i<transform.childCount;i++)
        {
            var _t=transform.GetChild(i).GetChild(0).GetComponent<MonsterMovement>();
            _t.SetMoveable(b);
        }
    }
}
