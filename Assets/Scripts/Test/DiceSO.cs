/*using UnityEngine;

[CreateAssetMenu(fileName = "NewDice", menuName = "Dice/New Dice")]
public class DiceSO : ScriptableObject
{
    [Tooltip("骰子面数，注意和DiceSide对应")]
    public int sidesCount; 
    // 引用每一面的DiceSide数据
    public DiceSide[] diceSides;

    public GameObject diceModel; // 骰子的3D模型

    public int GetMatch()
    {
        // 这里可以实现随机返回一个骰子面的值
        return diceSides[Random.Range(0, sidesCount)].value;
    }
}
*/