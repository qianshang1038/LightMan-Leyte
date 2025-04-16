using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Dice))]
public class DiceEditor : Editor
{
    // һ������������Dice����
    SerializedProperty diceSides;

    // ������Dice���ʱ������Dice���ҵ���׼������
    void OnEnable()
    {
        // ��Dice�����л��ֶ���Ѱ��diceSides���Ǹ����飩
        diceSides = serializedObject.FindProperty("diceSides");
    }

    #region Inspectorչʾ������Ϣ

    // ��Dice���ÿ������Ϣչʾ������ͬʱ�ṩ���㷽��
    public override void OnInspectorGUI()
    {
        // ����һ�����л��ֶ�
        serializedObject.Update();

        ShowDiceSideInspectorGUI();

        // ��һ���������ӵ�Button
        serializedObject.ApplyModifiedProperties();
        if(GUILayout.Button("Calculate Sides Info"))
        {
            CalculateSides();
        }
    }

    // ��GUI��չʾ������Ϣ
    private void ShowDiceSideInspectorGUI()
    {
        EditorGUILayout.LabelField("Dice Editor", EditorStyles.boldLabel);
        // GUI�������һ���������DiceSides����Ϣ
        EditorGUI.indentLevel++;
        for (int i = 0; i < diceSides.arraySize; i++)
        {
            ShowDiceSideUI(i);
        }
        EditorGUI.indentLevel--;
    }

    // չʾ����ÿһ�����Ϣ
    private void ShowDiceSideUI(int index)
    {
        // ��ȡÿһ�������Ϣ���Ҹ�ֵ
        SerializedProperty currentSide = diceSides.GetArrayElementAtIndex(index);
        SerializedProperty value = currentSide.FindPropertyRelative("value");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(value, new GUIContent("Side " + (index + 1)));

        // �ṩһ��չʾ��ǰ��İ�ť
        if (GUILayout.Button("Show", GUILayout.Width(70)))
        {
            RotateDiceToSide(index);
        }

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region ��������������Ϣ�ķ���

    private void CalculateSides()
    {
        // ��ǲ���ȡ��ǰ�༭����������ӣ��õ���Mesh��Ϣ
        Dice thisDice = target as Dice;
        Mesh mesh = GetMesh(thisDice);

        // ����Mesh�Զ�Ѱ��ÿ�������Ϣ�����ҵ���Side��Ϣ�洢����
        List<DiceSide> foundSides = FindDiceSides(mesh);
        thisDice.diceSides = new DiceSide[foundSides.Count];
        serializedObject.Update();

        // ����ÿһ��Side
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

    // �����ӵ����ת����Ȼ��ˢ����ͼ
    private void RotateDiceToSide(int index)
    {
        Dice thisDice = target as Dice;
        thisDice.transform.rotation = thisDice.GetWorldRotationFor(index);
        SceneView.RepaintAll();
    }

    // �ҵ�����ÿ���棨������Ⱦ����mesh��Ȼ����䷨�������
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

            // ����ÿ��������ĺͷ���
            result.Add(new DiceSide
            {
                center = (a + b + c) / 3f,
                normal = Vector3.Cross(b - a, c - a).normalized
            });
        }

        return result;
    }

    // ��MeshCollider����MeshFilter���������ӵ�mesh
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
