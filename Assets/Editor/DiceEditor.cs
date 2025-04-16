using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Dice))]
public class DiceEditor : Editor
{
    // 一个数组存放所有Dice的面
    SerializedProperty diceSides;

    // 当遇到Dice类的时候将所有Dice面找到并准备计算
    void OnEnable()
    {
        // 从Dice的序列化字段中寻找diceSides（是个数组）
        diceSides = serializedObject.FindProperty("diceSides");
    }

    #region Inspector展示骰子信息

    // 将Dice类的每个面信息展示出来，同时提供计算方法
    public override void OnInspectorGUI()
    {
        // 更新一下序列化字段
        serializedObject.Update();

        ShowDiceSideInspectorGUI();

        // 放一个计算骰子的Button
        serializedObject.ApplyModifiedProperties();
        if(GUILayout.Button("Calculate Sides Info"))
        {
            CalculateSides();
        }
    }

    // 在GUI上展示骰子信息
    private void ShowDiceSideInspectorGUI()
    {
        EditorGUILayout.LabelField("Dice Editor", EditorStyles.boldLabel);
        // GUI面板增加一个缩进存放DiceSides的信息
        EditorGUI.indentLevel++;
        for (int i = 0; i < diceSides.arraySize; i++)
        {
            ShowDiceSideUI(i);
        }
        EditorGUI.indentLevel--;
    }

    // 展示骰子每一面的信息
    private void ShowDiceSideUI(int index)
    {
        // 获取每一个面的信息并且赋值
        SerializedProperty currentSide = diceSides.GetArrayElementAtIndex(index);
        SerializedProperty value = currentSide.FindPropertyRelative("value");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(value, new GUIContent("Side " + (index + 1)));

        // 提供一个展示当前面的按钮
        if (GUILayout.Button("Show", GUILayout.Width(70)))
        {
            RotateDiceToSide(index);
        }

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region 辅助计算骰子信息的方法

    private void CalculateSides()
    {
        // 标记并获取当前编辑对象这个骰子，得到其Mesh信息
        Dice thisDice = target as Dice;
        Mesh mesh = GetMesh(thisDice);

        // 传入Mesh自动寻找每个面的信息，将找到的Side信息存储起来
        List<DiceSide> foundSides = FindDiceSides(mesh);
        thisDice.diceSides = new DiceSide[foundSides.Count];
        serializedObject.Update();

        // 遍历每一个Side
        for (int i = 0; i < foundSides.Count; i++)
        {
            DiceSide side = foundSides[i];
            SerializedProperty sideProperty = diceSides.GetArrayElementAtIndex(i);
            sideProperty.FindPropertyRelative("center").vector3Value = side.center;
            sideProperty.FindPropertyRelative("normal").vector3Value = side.normal;
        }

        serializedObject.ApplyModifiedProperties();
        thisDice.transform.rotation = thisDice.GetWorldRotationFor(0);
    }

    // 把骰子的最顶面转过来然后刷新视图
    private void RotateDiceToSide(int index)
    {
        Dice thisDice = target as Dice;
        thisDice.transform.rotation = thisDice.GetWorldRotationFor(index);
        SceneView.RepaintAll();
    }

    // 找到骰子每个面（类似渲染器读mesh）然后分配法线坐标等
    private List<DiceSide> FindDiceSides(Mesh mesh)
    {
        List<DiceSide> result = new();

        int[] triangles = mesh.GetTriangles(0);
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 a = vertices[triangles[i]];
            Vector3 b = vertices[triangles[i + 1]];
            Vector3 c = vertices[triangles[i + 2]];

            // 计算每个面的中心和法线
            result.Add(new DiceSide
            {
                center = (a + b + c) / 3f,
                normal = Vector3.Cross(b - a, c - a).normalized
            });
        }

        return result;
    }

    // 用MeshCollider或者MeshFilter获得这个骰子的mesh
    private Mesh GetMesh(Dice thisDice)
    {
        MeshCollider meshCollider = thisDice.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            return meshCollider.sharedMesh;
        }
        else
        {
            MeshFilter meshFilter = thisDice.GetComponent<MeshFilter>();
            return meshFilter.sharedMesh;
        }
    }

    #endregion
}
