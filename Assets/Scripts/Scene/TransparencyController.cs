using System.Collections;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player; 
    public LayerMask wallLayerMask;
    private RaycastHit hit;

    // 丝滑过渡持续时间
    public float fadeDuration = 0.2f;

    // 记录材质的原始状态声明
    private Material originalMaterial;
    private Color originalColor;
    
    private Renderer targetRenderer;

    // 正在转换状态?
    private bool isFading = false; 

    // 声明 当前透明度状态 目标透明度状态
    private float currentAlpha = 1.0f;
    private float targetAlpha = 1.0f; 


    [field: Range(0, 1)]
    public float finalAlpha = 0.55f;

    private void Awake()
    {
        wallLayerMask = LayerMask.GetMask("Wall");
    }

    void Update()
    {
        // 从摄像机到角色的射线
        Ray ray = new Ray(transform.position, player.position - transform.position);

        // 仅检测Wall层，记得把墙壁设置成Wall层
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, wallLayerMask))
        {
            // 检查是否是一个可以调整透明度的物体
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null && IsTransparent(renderer.material))
            {
                if (!isFading && currentAlpha != 0.3f)
                {
                    targetRenderer = renderer;
                    targetAlpha = finalAlpha;
                    StartCoroutine(FadeMaterial(renderer.material, currentAlpha, targetAlpha));
                }
            }
            else
            {
                // 如果没有遮挡物体，恢复透明度
                if (!isFading && targetRenderer != null && currentAlpha != 1.0f)
                {
                    targetAlpha = 1.0f;
                    StartCoroutine(FadeMaterial(targetRenderer.material, currentAlpha, targetAlpha));
                }
            }
        }
        else
        {
            // 如果没有遮挡物体，恢复透明度
            if (!isFading && targetRenderer != null && currentAlpha != 1.0f)
            {
                targetAlpha = 1.0f;
                StartCoroutine(FadeMaterial(targetRenderer.material, currentAlpha, targetAlpha));
            }
        }
    }

    // 判断材质的 RenderQueue 是否大于等于 3000, 如果是Opaque物体就不能修改透明度了
    private bool IsTransparent(Material material)
    {
        return material.renderQueue >= 3000;
    }

    // 开一个协程来切换颜色
    private IEnumerator FadeMaterial(Material mat, float startAlpha, float endAlpha)
    {
        // 每次开始渐变时，缓存材质的当前颜色
        if (originalMaterial == null || originalMaterial != mat)
        {
            originalMaterial = mat;
            originalColor = mat.color;
        }

        isFading = true;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            Color newColor = originalColor;
            newColor.a = alpha;
            mat.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最终应用调整后的材质
        Color finalColor = originalColor;
        finalColor.a = endAlpha;
        mat.color = finalColor;

        currentAlpha = endAlpha; // 更新当前透明度状态

        // 恢复透明时清空目标渲染器
        if (endAlpha == 1.0f)
        {
            targetRenderer = null;
            originalMaterial = null;
            originalColor = Color.white;
        }

        isFading = false;
    }

}
