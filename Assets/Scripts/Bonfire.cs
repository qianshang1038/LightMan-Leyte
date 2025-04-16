using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour,IInteract
{
    private HeadLight headLight;

    public GameObject InteractObject;
    public Transform InteractInfoPos;
    private GameObject _interactObject;

    private bool isGenerateEmote;

    void Start()
    {
        headLight=GameObject.FindGameObjectWithTag("HeadLight").GetComponent<HeadLight>();
    }

    void Update()
    {
        if(!isGenerateEmote)
        {
            Destroy(_interactObject);
        }
    }

    void LateUpdate()
    {
        isGenerateEmote = false;
    }

    //ºóÃæ¸Ä TODO
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("exitBornFire");
            headLight.SetHeadLightFilledSlowly(false);
        }

    }

    public void InteractInfo(bool b)
    {
        isGenerateEmote = true;
        _interactObject=Instantiate(InteractObject,InteractInfoPos);
        headLight.SetHeadLightFilledSlowly(true);
    }

    public void Interact()
    {
        Debug.Log("interact");
        headLight.SetHeadLightFilledQuickly();
        StartCoroutine(ShowBornfirePanel(2f));
        gameObject.tag ="InteractedObj";
    }

    private IEnumerator ShowBornfirePanel(float s)
    {
        BornFireInteractPanel.Instance.ShowMe();
        yield return new WaitForSeconds(s);
        BornFireInteractPanel.Instance.HideMe();
    }
}
