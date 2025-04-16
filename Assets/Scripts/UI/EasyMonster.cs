using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一个Monster示例 
public class EasyMonster : BaseMonster2D
{
    public override void MonsterSkill()
    {
        //里面写具体技能
        base.MonsterSkill();
    }

    public override string MonsterInfo()
    {
        //写描述
        return "easy";
    }
}
