using System.Collections;
using UnityEngine;
using scene;

public class KeyNeedingDoor : Door
{
    [SerializeField]
    public string needingKeyName = "";

    private Material doorMaterial; 
    
    // dissolveValue = _Dissolve in Shader
    private float dissolveValue = 0f;
    private float dissolveSpeed = 0.2f;   
    private bool isDissolving = false;

    protected override bool TryUnlock()
    {
        Logger.Log("Try Unlock");
        if (!UIBagMgr.Instance.UseItemByName(needingKeyName))
        {
            Logger.Log("No related named key was found!");
            return false;
        }

        DissolveUnlock();
        return base.TryUnlock();
    }

    #region Unlock Behaviour

    private void DissolveUnlock()
    {
        if (!isDissolving)
        {
            isDissolving = true;

            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                doorMaterial = renderer.material;
                StartCoroutine(PerformDissolve());
            }
            else
            {
                Logger.LogWarning("No Renderer component found on the door!");
            }
        }
    }

    // 逐步（逐帧增加）调整_Dissolve值并控制SetActive
    private IEnumerator PerformDissolve()
    {
        while (dissolveValue < 1f)
        {
            dissolveValue += Time.deltaTime * dissolveSpeed;
            doorMaterial.SetFloat("_Dissolve", dissolveValue);

            if (dissolveValue > 0.98f)
            {
                gameObject.SetActive(false);
            }

            yield return null;
        }
    }

    #endregion
}
