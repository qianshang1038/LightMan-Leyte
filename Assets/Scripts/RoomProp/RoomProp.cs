using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不给道具
/// </summary>

[RequireComponent(typeof(PlotLoader))]
public class RoomProp : MonoBehaviour,IInteract
{
    public GameObject InteractObject;
    public Transform InteractInfoPos;
    private GameObject _interactObject;
    private bool isGenerateEmote = false;

    private PlotLoader plotLoader;
    [SerializeField] private string plotJsonName = "";
    [SerializeField] private int plotNum = 0;


    private void Start()
    {
        plotLoader = gameObject.GetComponent<PlotLoader>();
    }

    public void Interact()
    {
        plotLoader.LoadPlot(plotJsonName);
        plotLoader.PlayPlotThroughNodeID(plotNum);
    }

    void Update()
    {
        if (!isGenerateEmote && _interactObject != null)
        {
            Destroy(_interactObject);
            _interactObject = null; // 确保 _interactObject 设置为 null，避免重复销毁
        }
    }

    void LateUpdate()
    {
        // 重置 isGenerateEmote 为 false，确保下一帧继续检测
        isGenerateEmote = false;
    }

    public void InteractInfo(bool b)
    {
        isGenerateEmote = true;
        // 只有在 _interactObject 不存在时才生成新对象
        if (_interactObject == null)
        {
            _interactObject = Instantiate(InteractObject, InteractInfoPos.position, InteractInfoPos.rotation);
            _interactObject.transform.parent = InteractInfoPos.transform;
        }
    }

}
