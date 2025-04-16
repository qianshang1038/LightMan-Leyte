using UnityEngine;

[System.Serializable]
public class DiceSide
{
    public Vector3 center;
    public Vector3 normal;
    public int value;
}

public class Dice : MonoBehaviour
{
    [SerializeField] public DiceSide[] diceSides;

    // 获得当前骰子世界空间下旋转的方法
    public Quaternion GetWorldRotationFor(int index)
    {
        Vector3 worldNormalToMatch = transform.TransformDirection(GetDiceSide(index).normal);
        return Quaternion.FromToRotation(worldNormalToMatch, Vector3.up) * transform.rotation;
    }

    public DiceSide GetDiceSide(int index)
    {
        return diceSides[index];
    }

    public int GetTopFaceValue()
    {
        float maxDot = -1f; // 使用 -1 作为初始最大值，因为点积范围是 [-1, 1]
        DiceSide topFace = null;

        // 遍历所有面，找到与 transform.up 法线点积最大的面
        foreach (var side in diceSides)
        {
            // 计算骰子面法线与上方向（Y轴）的点积
            float dot = Vector3.Dot(transform.TransformDirection(side.normal), Vector3.up);

            // 找到最接近向上（最大点积）的面
            if (dot > maxDot)
            {
                maxDot = dot;
                topFace = side;
            }
        }

        // 返回最接近向上的面值，未找到则返回 -1
        return topFace != null ? topFace.value : -1;
    }
}
