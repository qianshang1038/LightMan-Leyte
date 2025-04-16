using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    void Start()
    {
        // 在开始时让所有带有 SpriteRenderer 的子物体面向相机
        AllFaceCamera(transform);
    }

    // 递归检查所有子物体及其子物体，找到带有 SpriteRenderer 组件的物体进行旋转
    public void AllFaceCamera(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                child.rotation = Camera.main.transform.rotation;
            }

            // 递归调用，继续检查子物体的子物体
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
