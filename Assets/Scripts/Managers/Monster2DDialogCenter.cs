using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem.Switch;
using static Plot;

/// <summary>
/// 脚本挂在Monster2DMgr下
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
            //触发回应对话 TODO
            Debug.Log("回应对话");
            //san值掉落50%
            monster.LoseSanityPercent(0.5f);
        }
        else
        {
            Logger.LogError("没有对话怪物");
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
            //触发回应对话 TODO

        }
        else
        {
            Logger.LogError("没有对话怪物");
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
            //直接攻击 TODO

        }
        else
        {
            Logger.LogError("没有对话怪物");
        }
    }
}
