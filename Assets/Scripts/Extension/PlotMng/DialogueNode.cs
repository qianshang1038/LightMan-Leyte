using System;

[Serializable]
public class DialogueNode
{
    public int id;
    public int Step;
    public int SpeakType;
    public float WaitTime;
    public string Speaker;
    public string Image;
    public string Voice;
    public string Lines;
    public string Scripts;

    // 从Json读取进行构造的构造函数
    public DialogueNode(int id, int step, int speakType, float waitTime, string speaker, string image, string voice, string lines, string scripts)
    {
        this.id = id;
        this.Step = step;
        this.SpeakType = speakType;
        this.WaitTime = waitTime;
        this.Speaker = speaker;
        this.Image = image;
        this.Voice = voice;
        this.Lines = lines;
        this.Scripts = scripts;
    }
}
