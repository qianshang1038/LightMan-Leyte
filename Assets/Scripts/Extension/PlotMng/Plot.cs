using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class Plot
{
    // ʹ�� Dictionary �洢�Ի��ڵ㣬�� ID ��Ϊ��
    private Dictionary<int, DialogueNode> dialogueNodes;
    // ����ڶ�Ӧ�ĳ�ʼID
    private int startId;

    #region ��ǰ�Ի������Ϣ

    // ��ǰ���ŵ�Node�����������ĸ�Step�����Ǵ�0��ʼ��
    private int currentNodeIndex;
    // ��ǰ�Ի��ĶԻ����� 0�޶Ի� 1�԰� 2����Ի�
    private int speakType;
    // ��ǰ˵��������
    private string speakerName;
    // ��ǰ�Ի����ı�
    private string currentLine;

    // ��ǰ�Ի��������ű��¼�
    private ScriptInfo scriptInfo;
    // �ȴ�ʱ�书�ܣ����ֶ����Բ���
    private int waitTime;
    // ��ǰ�Ի�����ͼƬ����
    private string profilePhotoName;
    // ��ǰ�Ի�������Ƶ����
    private string voiceName;

    #endregion

    public enum SpeakType
    {
        None,
        Narration,
        Dialogue
    }

    //����T ��ʾ��2dս�������µ�
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

    // ͨ��Json�ļ�����Resource�ļ��м���Json�ļ�������������е�DialogueNodes
    private void LoadDialogueNodes(string tableName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"PlotConfig/{tableName}");
        if (textAsset != null)
        {
            string json = textAsset.text;

            // ��Json�ļ�����Ϊ����֮����������ֵ� ʹ�� ID ��Ϊ���洢�ڵ�
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
    /// ͨ�� ID ��ȡ��ǰ�ڵ㲢����
    /// </summary>
    /// <param name="addIndex">��addIndex��ʼ��Ĭ��Ϊ0</param>
    public void PlayDialogue(int addIndex=0)
    {
        if (dialogueNodes.TryGetValue(currentNodeIndex + addIndex, out DialogueNode node))
        {
            // �����뵱ǰ�Ի�����������Ϣ����DialoguePanel������
            // ����ǰ�Ի���㸳��Ӧֵ��֮��ִ����ع���
            profilePhotoName = node.Image;
            voiceName = node.Voice;
            speakType = GetSpeakType(node);
            scriptInfo = GetScriptInfo(node);
            speakerName = node.Speaker;
            currentLine = node.Lines;

            // ���ȼ�����Ƶ
            if (voiceName != null)
            {
                PlayPlotAudio(voiceName);
            }

            // ֮��չʾDialogue��UI��
            DisplayDialogue(node);

            // ����������㸽���Ľű���Ϣ
            ProcessScriptInfo(scriptInfo,speakerName);
            currentNodeIndex = node.Step;
        }
    }

    // ��ת��㹦�ܣ���û���ܺ�
    public void JumpToNode(int id)
    {
        if (dialogueNodes.ContainsKey(id))
        {
            currentNodeIndex = id - startId; // ����Ϊ��Ӧ������
        }
    }

    // չʾ�Ի���Ϣ
    private void DisplayDialogue(DialogueNode node)
    {
        //Logger.Log($"{node.Speaker}: {node.Lines}");

        // �������չʾ
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
    /// ���㲻�Ի� Ҳ��д���նԻ� �Ѵ������� ��Ӧ������/������������  
    /// </summary>
    /// <param name="scriptInfo">��������</param>
    /// <param name="speakerName">����/������</param>
    private void ProcessScriptInfo(ScriptInfo scriptInfo,string speakerName)
    {
        switch (scriptInfo)
        {
            case ScriptInfo.Shake:
                // ����ҡ���߼�
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
                // �����κ�������¼����
                break;
        }
    }

    // �˳���ڲ��ŵľ���ʵ��
    private void ExitPlot()
    {
        DialoguePanel.Instance.HideMe();
        return;
    }

    #region ��ǰ�����Ϣ��ȡ����,������ֿ��Կ��Ƿŵ�Node���棿

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

    // ������ýű���Ϣ���߼�
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
            _ => ScriptInfo.None // �ȷ��� None
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
