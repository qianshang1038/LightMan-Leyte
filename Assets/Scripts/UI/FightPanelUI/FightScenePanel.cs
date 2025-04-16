using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightScenePanel : BasePanel<FightScenePanel>
{
    private SeqController seqController;
    private bool shouldStart = false;
    private string monster;
    private int monsterNum;
    void Start()
    {
        LoadMonsters();
        
        TurnPlotMgr.Instance.MeetEnemy();

        //只要这个场景SetActive就进入战斗
        //直接调试战斗直接把这一段注释,SetStartRolled设置为true,然后shouldStart设置为ture
        seqController = GameObject.Find("DiceManager").GetComponent<SeqController>();
        if (seqController == null)
        {
            Logger.LogError("seqController is null");
        }

        TurnPlotMgr.Instance.ThrowOffensiveDice();
        seqController.ShouldStartFight += SetStart;

        seqController.SetStartRolled(false);

    }
    private void Update()
    {
        if (shouldStart)
        {
            //判断谁先后手
            DecideOffensive();
            TurnController.Instance.ResetNum();
            Game2DController.Instance.StartFight();
            shouldStart = false;
        }
    }
    public void DecideOffensive()
    {
        Logger.Log($"Player is {TurnController.Instance.playerDiceNum}, and monster is {TurnController.Instance.monsterDiceNum}");
        if (TurnController.Instance.playerDiceNum > TurnController.Instance.monsterDiceNum)
        {
            Game2DController.Instance.Setoffensive(true);
        }
        else
        {
            Game2DController.Instance.Setoffensive(false);
        }
    }
    public void SetStart()
    {
        shouldStart = true;
    }
    public void LoadMonsters()
    {
        GameObject ms = Resources.Load<GameObject>($"MonsterPrefabs/{monster}");
        for (int i = 0; i < monsterNum; i++)
        {
            GameObject pref = GameObject.Instantiate(ms);
            pref.transform.parent = transform.Find("MonsterMgr");
        }

    }
    public void InitializeMonsters(string _monsterName, int _monsterNum)
    {
        monster = _monsterName;
        monsterNum = _monsterNum;
    }


}
