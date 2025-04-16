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
        // 确保 Slider 组件已分配
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    // 设置最大值
    public virtual void SetMaxValue(float maxValue)
    {
        this.maxValue = maxValue;
        slider.maxValue = maxValue;
        slider.value = maxValue; // 默认初始化时满值
        currentValue = maxValue;
    }

    // 更新当前值
    public virtual void SetCurrentValue(float newValue)
    {
        currentValue = Mathf.Clamp(newValue, 0, maxValue);
        slider.value = currentValue;
    }

    // 获取当前值
    public float GetCurrentValue()
    {
        return currentValue;
    }

    // 获取最大值
    public float GetMaxValue()
    {
        return maxValue;
    }
    
}
