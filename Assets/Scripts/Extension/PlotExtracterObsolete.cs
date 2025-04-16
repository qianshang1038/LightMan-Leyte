using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using cfg;
using cfg.GamePlot;
using UnityEngine.UI;
using System;

public class PlotExtracter : MonoBehaviour
{
    // ��ʹ�õľ����
    private Tables tables;
    // ��ǰʹ�õľ��������
    private string currentTableName;
    // ������̨��
    private Text currentLine;
    private Text speakerName;
    // ʹ�õ�ͷ��
    private Image profilePhoto;
    // ��ǰ��Ӧ����Ƶ
    private AudioSource audioSource;
    //private bool isWaiting;
    // ��ǰ���е��Ի��ĵڼ���
    private int currentStep;

    private Dictionary<string, object> plotTableDict;
    private Dictionary<string, GetPlotDelegate> getPlotMethods;

    public enum SpeakType
    {
        None, Narration, Dialogue
    }

    public enum DialogueEffects
    {
        Audio, Shake, FadeOut
    }

    private delegate testPlot_0101 GetPlotDelegate(int id);

    private void Start()
    {
        tables = new Tables(LoadTable);
        currentStep = 0; // ��ʼ����ǰ����

        // ��ʼ���ֵ�ӳ��
        InitializePlotTableDict();

        // �����Ļ�ȡ����
        CacheGetMethods();

        UpdateDialogue(); // ���¶Ի�
    }

    // ��������ץȡ���ض�����Ƭ��
    private JSONNode LoadTable(string tableName)
    {
        currentTableName = tableName;
        TextAsset textAsset = Resources.Load<TextAsset>($"PlotConfig/{tableName}");
        return JSON.Parse(textAsset.text);
    }

    // ��ʼ�������ֶα��ڻ�ȡ
    private void InitializePlotTableDict()
    {
        plotTableDict = new Dictionary<string, object>
        {
            { "TbtestPlot0101", tables.TbtestPlot0101 },
            { "TbtestPlot0201", tables.TbtestPlot0201 }
        };
    }

    // ����ÿ����� Get ����
    private void CacheGetMethods()
    {
        getPlotMethods = new Dictionary<string, GetPlotDelegate>();
        foreach (var table in plotTableDict)
        {
            var tableName = table.Key;
            var tableInstance = table.Value;
            var methodInfo = tableInstance.GetType().GetMethod("Get", new System.Type[] { typeof(int) });
            if (methodInfo != null)
            {
                var getMethod = (GetPlotDelegate)Delegate.CreateDelegate(typeof(GetPlotDelegate), tableInstance, methodInfo);
                getPlotMethods[tableName] = getMethod;
            }
        }
    }

    private void OnPlayerClick(int id, string tableName)
    {
        if (getPlotMethods.TryGetValue(tableName, out var getMethod))
        {
            var thisLine = getMethod(id);
            if (thisLine != null)
            {
                speakerName.text = thisLine.Speaker; // ����˵��������
                currentLine.text = thisLine.Lines; // ���µ�ǰ�Ի�����

                // ����ͷ��
                if (!string.IsNullOrEmpty(thisLine.Image))
                {
                    profilePhoto.sprite = Resources.Load<Sprite>($"ProfilePhotos/{thisLine.Image}");
                    if (profilePhoto.sprite == null)
                    {
                        Debug.LogWarning("Profile Photo Missing!");
                    }
                }

                // ������Ƶ
                if (!string.IsNullOrEmpty(thisLine.Voice))
                {
                    audioSource.clip = Resources.Load<AudioClip>($"Audio/PlotVoice/{thisLine.Voice}");
                    if (audioSource != null)
                    {
                        audioSource.Play();
                    }
                    else
                    {
                        Debug.LogWarning("Audio Missing!");
                    }
                }

                // ����ͬ�ĶԻ�����
                switch (thisLine.SpeakType)
                {
                    case (int)SpeakType.Narration:
                        // �԰��߼�
                        break;
                    case (int)SpeakType.Dialogue:
                        // ����Ի�
                        break;
                }

                // �ȴ�ʱ���߼�
                if (thisLine.WaitTime > 0)
                {
                    //StartCoroutine(WaitAndProceed(thisLine.WaitTime));
                }
            }
            else
            {
                Debug.LogError($"Line with ID {id} not found in table {tableName}");
            }
        }
        else
        {
            Debug.LogError($"Get method not found for table {tableName}");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ��������
        {
            OnPlayerClick(currentStep, currentTableName); // ������Ը�����Ҫ��������� ID
            currentStep++; // ���Ӳ���
            UpdateDialogue(); // ���¶Ի�
        }
    }

    private void UpdateDialogue()
    {
        // ������жԻ��͸�����һ��
        if (currentStep < tables.TbtestPlot0201.DataList.Count)
        {
            OnPlayerClick(currentStep, currentTableName); // ���ݵ�ǰ������¶Ի�
        }
        else
        {
            // �Ի���������
            Debug.Log("End of Dialogue");
            // ���磺��ʾ��������򷵻����˵�
        }
    }

/*    private IEnumerator WaitAndProceed(float waitTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }*/
}
