using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//һ��Monsterʾ�� 
public class EasyMonster : BaseMonster2D
{
    public override void MonsterSkill()
    {
        //����д���弼��
        base.MonsterSkill();
    }

    public override string MonsterInfo()
    {
        //д����
        return "easy";
    }
}
