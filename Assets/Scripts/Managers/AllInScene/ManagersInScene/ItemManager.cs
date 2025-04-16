using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteract
{
    public enum ItemType
    {
        None,
        Prop,        // ��������
        PropHas      // ������
    }

    public ItemType itemType = ItemType.None;

    // �������õ�����Ϣ
    [SerializeField] private string plotJsonName = "";
    [SerializeField] private int plotNum = 0;
    public Sprite prop;  // ���ߵ�ͼ��
    public GameObject InteractObject; // ��������
    public Transform InteractInfoPos; // ������Ϣ��ʾλ��
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
                Debug.Log("�����Ի�");
                plotLoader.LoadPlot(plotJsonName);
                plotLoader.PlayPlotThroughNodeID(plotNum);
                break;

            case ItemType.PropHas:
                UIBagMgr.Instance.SetAndGenerateSprite(prop);
                break;

            default:
                Debug.LogWarning("û�ж���ĵ������͡�");
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
