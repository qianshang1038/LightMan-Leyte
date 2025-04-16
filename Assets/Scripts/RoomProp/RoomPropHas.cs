using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class RoomPropHas : MonoBehaviour,IInteract
{
    public Sprite prop;

    public GameObject InteractObject;
    public Transform InteractInfoPos;
    private GameObject _interactObject;
    private bool isGenerateEmote=false;

    public virtual void Interact()
    {
        UIBagMgr.Instance.SetAndGenerateSprite(prop);
        if(prop.name=="Map")
        {
            gameObject.tag = "InteractedObj";
        }
    }

    void Update()
    {
        // ֻ�е� isGenerateEmote Ϊ false �� _interactObject ����ʱ�������ٶ���
        if (!isGenerateEmote && _interactObject != null)
        {
            Destroy(_interactObject);
            _interactObject = null; // ȷ�� _interactObject ����Ϊ null�������ظ�����
        }
    }

    void LateUpdate()
    {
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
