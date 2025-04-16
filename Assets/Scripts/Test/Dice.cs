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

    // ��õ�ǰ��������ռ�����ת�ķ���
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
        float maxDot = -1f; // ʹ�� -1 ��Ϊ��ʼ���ֵ����Ϊ�����Χ�� [-1, 1]
        DiceSide topFace = null;

        // ���������棬�ҵ��� transform.up ���ߵ��������
        foreach (var side in diceSides)
        {
            // ���������淨�����Ϸ���Y�ᣩ�ĵ��
            float dot = Vector3.Dot(transform.TransformDirection(side.normal), Vector3.up);

            // �ҵ���ӽ����ϣ������������
            if (dot > maxDot)
            {
                maxDot = dot;
                topFace = side;
            }
        }

        // ������ӽ����ϵ���ֵ��δ�ҵ��򷵻� -1
        return topFace != null ? topFace.value : -1;
    }
}
