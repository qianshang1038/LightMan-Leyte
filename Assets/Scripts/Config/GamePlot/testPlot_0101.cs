
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.GamePlot
{
public sealed partial class testPlot_0101 : Luban.BeanBase
{
    public testPlot_0101(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["Step"].IsNumber) { throw new SerializationException(); }  Step = _buf["Step"]; }
        { if(!_buf["SpeakType"].IsNumber) { throw new SerializationException(); }  SpeakType = _buf["SpeakType"]; }
        { if(!_buf["WaitTime"].IsNumber) { throw new SerializationException(); }  WaitTime = _buf["WaitTime"]; }
        { if(!_buf["Speaker"].IsString) { throw new SerializationException(); }  Speaker = _buf["Speaker"]; }
        { if(!_buf["Image"].IsString) { throw new SerializationException(); }  Image = _buf["Image"]; }
        { if(!_buf["Voice"].IsString) { throw new SerializationException(); }  Voice = _buf["Voice"]; }
        { if(!_buf["Lines"].IsString) { throw new SerializationException(); }  Lines = _buf["Lines"]; }
        { if(!_buf["Scripts"].IsString) { throw new SerializationException(); }  Scripts = _buf["Scripts"]; }
    }

    public static testPlot_0101 DeserializetestPlot_0101(JSONNode _buf)
    {
        return new GamePlot.testPlot_0101(_buf);
    }

    /// <summary>
    /// 对话ID
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 步骤
    /// </summary>
    public readonly int Step;
    /// <summary>
    /// 对话类型  0没有对话 1旁白 2人物对话
    /// </summary>
    public readonly int SpeakType;
    /// <summary>
    /// 间隔时长
    /// </summary>
    public readonly float WaitTime;
    /// <summary>
    /// 说话者
    /// </summary>
    public readonly string Speaker;
    /// <summary>
    /// 头像
    /// </summary>
    public readonly string Image;
    /// <summary>
    /// 播放语音
    /// </summary>
    public readonly string Voice;
    /// <summary>
    /// 台词
    /// </summary>
    public readonly string Lines;
    /// <summary>
    /// 调用代码 例如audio shake exit等等(需要后续实现)
    /// </summary>
    public readonly string Scripts;
   
    public const int __ID__ = -35388077;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "Step:" + Step + ","
        + "SpeakType:" + SpeakType + ","
        + "WaitTime:" + WaitTime + ","
        + "Speaker:" + Speaker + ","
        + "Image:" + Image + ","
        + "Voice:" + Voice + ","
        + "Lines:" + Lines + ","
        + "Scripts:" + Scripts + ","
        + "}";
    }
}

}

