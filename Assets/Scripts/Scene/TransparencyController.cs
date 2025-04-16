using System.Collections;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player; 
    public LayerMask wallLayerMask;
    private RaycastHit hit;

    // ˿�����ɳ���ʱ��
    public float fadeDuration = 0.2f;

    // ��¼���ʵ�ԭʼ״̬����
    private Material originalMaterial;
    private Color originalColor;
    
    private Renderer targetRenderer;

    // ����ת��״̬?
    private bool isFading = false; 

    // ���� ��ǰ͸����״̬ Ŀ��͸����״̬
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
        // �����������ɫ������
        Ray ray = new Ray(transform.position, player.position - transform.position);

        // �����Wall�㣬�ǵð�ǽ�����ó�Wall��
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, wallLayerMask))
        {
            // ����Ƿ���һ�����Ե���͸���ȵ�����
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
                // ���û���ڵ����壬�ָ�͸����
                if (!isFading && targetRenderer != null && currentAlpha != 1.0f)
                {
                    targetAlpha = 1.0f;
                    StartCoroutine(FadeMaterial(targetRenderer.material, currentAlpha, targetAlpha));
                }
            }
        }
        else
        {
            // ���û���ڵ����壬�ָ�͸����
            if (!isFading && targetRenderer != null && currentAlpha != 1.0f)
            {
                targetAlpha = 1.0f;
                StartCoroutine(FadeMaterial(targetRenderer.material, currentAlpha, targetAlpha));
            }
        }
    }

    // �жϲ��ʵ� RenderQueue �Ƿ���ڵ��� 3000, �����Opaque����Ͳ����޸�͸������
    private bool IsTransparent(Material material)
    {
        return material.renderQueue >= 3000;
    }

    // ��һ��Э�����л���ɫ
    private IEnumerator FadeMaterial(Material mat, float startAlpha, float endAlpha)
    {
        // ÿ�ο�ʼ����ʱ��������ʵĵ�ǰ��ɫ
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

        // ����Ӧ�õ�����Ĳ���
        Color finalColor = originalColor;
        finalColor.a = endAlpha;
        mat.color = finalColor;

        currentAlpha = endAlpha; // ���µ�ǰ͸����״̬

        // �ָ�͸��ʱ���Ŀ����Ⱦ��
        if (endAlpha == 1.0f)
        {
            targetRenderer = null;
            originalMaterial = null;
            originalColor = Color.white;
        }

        isFading = false;
    }

}
