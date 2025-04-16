using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlider : MonoBehaviour
{
    [SerializeField] protected Slider slider;  
    protected float currentValue;              
    protected float maxValue;                 

    protected virtual void Start()
    {
        // ȷ�� Slider ����ѷ���
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    // �������ֵ
    public virtual void SetMaxValue(float maxValue)
    {
        this.maxValue = maxValue;
        slider.maxValue = maxValue;
        slider.value = maxValue; // Ĭ�ϳ�ʼ��ʱ��ֵ
        currentValue = maxValue;
    }

    // ���µ�ǰֵ
    public virtual void SetCurrentValue(float newValue)
    {
        currentValue = Mathf.Clamp(newValue, 0, maxValue);
        slider.value = currentValue;
    }

    // ��ȡ��ǰֵ
    public float GetCurrentValue()
    {
        return currentValue;
    }

    // ��ȡ���ֵ
    public float GetMaxValue()
    {
        return maxValue;
    }
    
}
