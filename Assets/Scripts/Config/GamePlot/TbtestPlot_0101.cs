
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
public partial class TbtestPlot_0101
{
    private readonly System.Collections.Generic.Dictionary<int, GamePlot.testPlot_0101> _dataMap;
    private readonly System.Collections.Generic.List<GamePlot.testPlot_0101> _dataList;
    
    public TbtestPlot_0101(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, GamePlot.testPlot_0101>();
        _dataList = new System.Collections.Generic.List<GamePlot.testPlot_0101>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            GamePlot.testPlot_0101 _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = GamePlot.testPlot_0101.DeserializetestPlot_0101(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, GamePlot.testPlot_0101> DataMap => _dataMap;
    public System.Collections.Generic.List<GamePlot.testPlot_0101> DataList => _dataList;

    public GamePlot.testPlot_0101 GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public GamePlot.testPlot_0101 Get(int key) => _dataMap[key];
    public GamePlot.testPlot_0101 this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

