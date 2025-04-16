using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteract
{
    public enum ItemType
    {
        None,
        Prop,        // 不给道具
        PropHas      // 给道具
    }

    public ItemType itemType = ItemType.None;

    // 用于设置道具信息
    [SerializeField] private string plotJsonName = "";
    [SerializeField] private int plotNum = 0;
    public Sprite prop;  // 道具的图标
    public GameObject InteractObject; // 交互对象
    public Transform InteractInfoPos; // 交互信息显示位置
    private GameObject _interactObject;
    private bool isGenerateEmote = false;

    private PlotLoader plotLoader;

    private void Start()
    {
        plotLoader = gameObject.GetComponent<PlotLoader>();
        if (plotLoader == null)
        {
            Debug.LogError("PlotLoader component is missing.");
        }
    }

    public void Interact()
    {
        switch (itemType)
        {
            case ItemType.Prop:
                Debug.Log("触发对话");
                plotLoader.LoadPlot(plotJsonName);
                plotLoader.PlayPlotThroughNodeID(plotNum);
                break;

            case ItemType.PropHas:
                UIBagMgr.Instance.SetAndGenerateSprite(prop);
                break;

            default:
                Debug.LogWarning("没有定义的道具类型。");
                break;
        }
    }

    void Update()
    {
        if (!isGenerateEmote && _interactObject != null)
        {
            Destroy(_interactObject);
            _interactObject = null;
        }
    }

    void LateUpdate()
    {
        isGenerateEmote = false;
    }

    public void InteractInfo(bool b)
    {
        isGenerateEmote = true;
        if (_interactObject == null)
        {
            _interactObject = Instantiate(InteractObject, InteractInfoPos.position, InteractInfoPos.rotation);
            _interactObject.transform.parent = InteractInfoPos.transform;
        }
    }
}
