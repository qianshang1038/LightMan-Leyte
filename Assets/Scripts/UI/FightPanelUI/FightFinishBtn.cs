using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightFinishBtn : Singleton<FightFinishBtn>
{
    /// <summary>
    /// 战斗胜利
    /// </summary>
    public void FightWin()
    {
        //查看是否成功

        GameInput.Instance.SetInput(true);
        FightBtn.Instance.FightEnd();
        //TODO 战斗胜利后

    }
    /// <summary>
    /// 战斗失败
    /// </summary>
    public void FightDefeat()
    {
        GameInput.Instance.SetInput(true);
        FightBtn.Instance.FightEnd();
        //TODO 战斗失败后回到篝火？
    }


}
