using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 给道具
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
        // 只有当 isGenerateEmote 为 false 且 _interactObject 存在时，才销毁对象
        if (!isGenerateEmote && _interactObject != null)
        {
            Destroy(_interactObject);
            _interactObject = null; // 确保 _interactObject 设置为 null，避免重复销毁
        }
    }

    void LateUpdate()
    {
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
