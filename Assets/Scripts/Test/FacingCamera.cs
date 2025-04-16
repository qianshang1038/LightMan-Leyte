using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    void Start()
    {
        // �ڿ�ʼʱ�����д��� SpriteRenderer ���������������
        AllFaceCamera(transform);
    }

    // �ݹ������������弰�������壬�ҵ����� SpriteRenderer ��������������ת
    public void AllFaceCamera(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                child.rotation = Camera.main.transform.rotation;
            }

            // �ݹ���ã���������������������
            if (child.childCount > 0)
            {
                AllFaceCamera(child);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AllFaceCamera(transform);
        }
    }
}
