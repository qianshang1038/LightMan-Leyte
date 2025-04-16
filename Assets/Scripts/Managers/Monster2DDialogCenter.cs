using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem.Switch;
using static Plot;

/// <summary>
/// �ű�����Monster2DMgr��
/// </summary>
public class Monster2DDialogCenter : Singleton<Monster2DDialogCenter>
{
    private BaseMonster2D monster = null;
    public void OnReceive(ScriptInfo scriptInfo,string monsterName)
    {
        switch (scriptInfo)
        {
            case ScriptInfo.T_Right:
                TRight(monsterName);
                break;
            case ScriptInfo.T_Middle:
                TMiddle(monsterName);
                break;
            case ScriptInfo.T_Error:
                TError(monsterName);
                break;
        }
    }

    private void TRight(string monsterName)
    {
        Transform child = transform.Find(monsterName);
        if (child != null)
        {
            monster=child.GetComponent<BaseMonster2D>();
        }

        if (monster != null)
        {
            //������Ӧ�Ի� TODO
            Debug.Log("��Ӧ�Ի�");
            //sanֵ����50%
            monster.LoseSanityPercent(0.5f);
        }
        else
        {
            Logger.LogError("û�жԻ�����");
        }
    }

    private void TMiddle(string monsterName)
    {
        Transform child = transform.Find(monsterName);
        if (child != null)
        {
            monster = child.GetComponent<BaseMonster2D>();
        }

        if (monster != null)
        {
            //������Ӧ�Ի� TODO

        }
        else
        {
            Logger.LogError("û�жԻ�����");
        }
    }

    private void TError(string monsterName)
    {
        Transform child = transform.Find(monsterName);
        if (child != null)
        {
            monster = child.GetComponent<BaseMonster2D>();
        }

        if (monster != null)
        {
            //ֱ�ӹ��� TODO

        }
        else
        {
            Logger.LogError("û�жԻ�����");
        }
    }
}
