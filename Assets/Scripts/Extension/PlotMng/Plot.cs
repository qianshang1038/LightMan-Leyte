using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class Plot
{
    // 使用 Dictionary 存储对话节点，以 ID 作为键
    private Dictionary<int, DialogueNode> dialogueNodes;
    // 该情节对应的初始ID
    private int startId;

    #region 当前对话结点信息

    // 当前播放的Node索引（到了哪个Step，但是从0开始）
    private int currentNodeIndex;
    // 当前对话的对话类型 0无对话 1旁白 2人物对话
    private int speakType;
    // 当前说话的人名
    private string speakerName;
    // 当前对话的文本
    private string currentLine;

    // 当前对话所触发脚本事件
    private ScriptInfo scriptInfo;
    // 等待时间功能，纯手动可以不做
    private int waitTime;
    // 当前对话所用图片名称
    private string profilePhotoName;
    // 当前对话所用音频名称
    private string voiceName;

    #endregion

    public enum SpeakType
    {
        None,
        Narration,
        Dialogue
    }

    //加了T 表示是2d战斗场景下的
    public enum ScriptInfo
    {
        None,
        Shake,
        Exit,
        T_Right,
        T_Middle,
        T_Error,
    }

    public Plot(string plotJsonName)
    {
        LoadDialogueNodes(plotJsonName);
        currentNodeIndex = 0;
    }

    // 通过Json文件名从Resource文件夹加载Json文件并构造存入所有的DialogueNodes
    private void LoadDialogueNodes(string tableName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"PlotConfig/{tableName}");
        if (textAsset != null)
        {
            string json = textAsset.text;

            // 将Json文件解析为链表之后逐个加入字典 使用 ID 作为键存储节点
            List<DialogueNode> nodes = JsonConvert.DeserializeObject<List<DialogueNode>>(json);
            dialogueNodes = new Dictionary<int, DialogueNode>();
            foreach (DialogueNode node in nodes)
            {
                dialogueNodes[node.id] = node;
            }
            currentNodeIndex = nodes[0].id;
        }
        else
        {
            Debug.LogError($"Failed to load JSON file: PlotConfig/{tableName}");
        }
    }


    /// <summary>
    /// 通过 ID 获取当前节点并播放
    /// </summary>
    /// <param name="addIndex">从addIndex开始，默认为0</param>
    public void PlayDialogue(int addIndex=0)
    {
        if (dialogueNodes.TryGetValue(currentNodeIndex + addIndex, out DialogueNode node))
        {
            // 先输入当前对话结点的所有信息，用DialoguePanel播出来
            // 给当前对话结点赋对应值，之后执行相关功能
            profilePhotoName = node.Image;
            voiceName = node.Voice;
            speakType = GetSpeakType(node);
            scriptInfo = GetScriptInfo(node);
            speakerName = node.Speaker;
            currentLine = node.Lines;

            // 首先加载音频
            if (voiceName != null)
            {
                PlayPlotAudio(voiceName);
            }

            // 之后展示Dialogue在UI上
            DisplayDialogue(node);

            // 最后处理这个结点附带的脚本信息
            ProcessScriptInfo(scriptInfo,speakerName);
            currentNodeIndex = node.Step;
        }
    }

    // 跳转结点功能，还没做很好
    public void JumpToNode(int id)
    {
        if (dialogueNodes.ContainsKey(id))
        {
            currentNodeIndex = id - startId; // 更新为对应的索引
        }
    }

    // 展示对话信息
    private void DisplayDialogue(DialogueNode node)
    {
        //Logger.Log($"{node.Speaker}: {node.Lines}");

        // 在面板上展示
        // DialoguePanel.Instance.ShowDialogue(speakType, profilePhotoName, speakerName, currentLine);
        if (DialoguePanel.Instance == null)
        {
            Debug.LogError("DialoguePanel instance is null. Make sure it is initialized before calling DisplayDialogue.");
            return;
        }
        DialoguePanel.Instance.ShowDialogue(node.SpeakType, node.Image, node.Speaker, node.Lines);
    }

    private void PlayPlotAudio(string plotAudioName)
    {
        return;
    }

    /// <summary>
    /// 就算不对话 也得写个空对话 把触发方法 反应的人物/怪物名传进来  
    /// </summary>
    /// <param name="scriptInfo">触发方法</param>
    /// <param name="speakerName">人物/怪物名</param>
    private void ProcessScriptInfo(ScriptInfo scriptInfo,string speakerName)
    {
        switch (scriptInfo)
        {
            case ScriptInfo.Shake:
                // 处理摇动逻辑
                break;
            case ScriptInfo.Exit:
                ExitPlot();
                break;
            case ScriptInfo.None:
                break;
            case ScriptInfo.T_Right:
                Monster2DDialogCenter.Instance.OnReceive(scriptInfo, speakerName);
                break;
            case ScriptInfo.T_Middle:
                Monster2DDialogCenter.Instance.OnReceive(scriptInfo, speakerName);
                break;
            case ScriptInfo.T_Error:
                Monster2DDialogCenter.Instance.OnReceive(scriptInfo, speakerName);
                break;
            default:
                // 不做任何事情或记录警告
                break;
        }
    }

    // 退出情节播放的具体实现
    private void ExitPlot()
    {
        DialoguePanel.Instance.HideMe();
        return;
    }

    #region 当前结点信息获取方法,这个部分可以考虑放到Node里面？

    private int GetSpeakType(DialogueNode node)
    {
        int currentSpeakType = 0;
        switch (node.SpeakType)
        {
            case 0:
                currentSpeakType = (int)SpeakType.None;
                break;
            case 1:
                currentSpeakType = (int)SpeakType.Narration;
                break;
            case 2:
                currentSpeakType = (int)SpeakType.Dialogue;
                break;
            default:
                Logger.LogWarning("No implementation of this node's speak type!");
                return currentSpeakType;
        }
        return currentSpeakType;
    }

    // 处理调用脚本信息的逻辑
    private ScriptInfo GetScriptInfo(DialogueNode node)
    {
        if(node.Scripts == "") return ScriptInfo.None;    
        ScriptInfo scriptInfo = node.Scripts.ToLower() switch
        {
            "none" => ScriptInfo.None,
            "shake" => ScriptInfo.Shake,
            "exit" => ScriptInfo.Exit,
            "t_right"=>ScriptInfo.T_Right,
            "t_middle" => ScriptInfo.T_Middle,
            "t_error" => ScriptInfo.T_Error,
            _ => ScriptInfo.None // 先返回 None
        };

        if (scriptInfo == ScriptInfo.None && node.Scripts.ToLower() != "none")
        {
            Logger.LogWarning($"Unknown Script Info: {node.Scripts}. Defaulting to None.");
        }

        return scriptInfo;
    }

    #endregion

    public int GetCurrentNodeId()
    {
        return currentNodeIndex;
    }
}
