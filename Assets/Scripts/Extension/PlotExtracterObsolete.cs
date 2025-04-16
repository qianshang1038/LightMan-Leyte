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
    // 可使用的剧情表
    private Tables tables;
    // 当前使用的剧情表名称
    private string currentTableName;
    // 剧情表的台本
    private Text currentLine;
    private Text speakerName;
    // 使用的头像
    private Image profilePhoto;
    // 当前对应的音频
    private AudioSource audioSource;
    //private bool isWaiting;
    // 当前进行到对话的第几步
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
        currentStep = 0; // 初始化当前步骤

        // 初始化字典映射
        InitializePlotTableDict();

        // 缓存表的获取方法
        CacheGetMethods();

        UpdateDialogue(); // 更新对话
    }

    // 读表函数，抓取出特定剧情片段
    private JSONNode LoadTable(string tableName)
    {
        currentTableName = tableName;
        TextAsset textAsset = Resources.Load<TextAsset>($"PlotConfig/{tableName}");
        return JSON.Parse(textAsset.text);
    }

    // 初始化所有字段便于获取
    private void InitializePlotTableDict()
    {
        plotTableDict = new Dictionary<string, object>
        {
            { "TbtestPlot0101", tables.TbtestPlot0101 },
            { "TbtestPlot0201", tables.TbtestPlot0201 }
        };
    }

    // 缓存每个表的 Get 方法
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
                speakerName.text = thisLine.Speaker; // 更新说话者名字
                currentLine.text = thisLine.Lines; // 更新当前对话内容

                // 更新头像
                if (!string.IsNullOrEmpty(thisLine.Image))
                {
                    profilePhoto.sprite = Resources.Load<Sprite>($"ProfilePhotos/{thisLine.Image}");
                    if (profilePhoto.sprite == null)
                    {
                        Debug.LogWarning("Profile Photo Missing!");
                    }
                }

                // 播放音频
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

                // 处理不同的对话类型
                switch (thisLine.SpeakType)
                {
                    case (int)SpeakType.Narration:
                        // 旁白逻辑
                        break;
                    case (int)SpeakType.Dialogue:
                        // 人物对话
                        break;
                }

                // 等待时间逻辑
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
        if (Input.GetMouseButtonDown(0)) // 检测鼠标点击
        {
            OnPlayerClick(currentStep, currentTableName); // 这里可以根据需要传入表名和 ID
            currentStep++; // 增加步骤
            UpdateDialogue(); // 更新对话
        }
    }

    private void UpdateDialogue()
    {
        // 如果还有对话就更新下一步
        if (currentStep < tables.TbtestPlot0201.DataList.Count)
        {
            OnPlayerClick(currentStep, currentTableName); // 根据当前步骤更新对话
        }
        else
        {
            // 对话结束处理
            Debug.Log("End of Dialogue");
            // 例如：显示结束画面或返回主菜单
        }
    }

/*    private IEnumerator WaitAndProceed(float waitTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }*/
}
