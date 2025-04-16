using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
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
            _interactObject = null; // ȷ�� _interactObject ����Ϊ null�������ظ�����
        }
    }

    void LateUpdate()
    {
        // ���� isGenerateEmote Ϊ false��ȷ����һ֡�������
        isGenerateEmote = false;
    }

    public void InteractInfo(bool b)
    {
        isGenerateEmote = true;
        // ֻ���� _interactObject ������ʱ�������¶���
        if (_interactObject == null)
        {
            _interactObject = Instantiate(InteractObject, InteractInfoPos.position, InteractInfoPos.rotation);
            _interactObject.transform.parent = InteractInfoPos.transform;
        }
    }

}
