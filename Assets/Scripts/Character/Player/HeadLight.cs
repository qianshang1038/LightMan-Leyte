using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLight : MonoBehaviour
{
    [SerializeField]
    private float originValue = 10f;
    private float intensityValue;
    [SerializeField]
    private float changeSpeed = 0.2f;

    public Light light;

    private bool isCloseToBornfire=false;
    private bool isQuickFillLight=false;

    void Start()
    {
        light=GetComponent<Light>();
        initLight();    
    }

    void Update()
    {
        //Debug.Log(isCloseToBornfire);
        if(isCloseToBornfire)
        {
            if(isQuickFillLight)
            {
                initLight();
                isQuickFillLight = false;
            }
            MakeLightHigher();
        }
        else
        {
            MakeLightLower();
        }

    }

    public void MakeLightHigher()
    {
        if(intensityValue>=originValue)
        {
            intensityValue=originValue;
            return;
        }
        intensityValue += changeSpeed;
        light.intensity = intensityValue;
    }

    public void MakeLightLower()
    {
        if(intensityValue <= 0)
        {
            intensityValue = 0;
            return;
        }
        intensityValue -= changeSpeed;
        light.intensity = intensityValue;
    }

    private void initLight()
    {
        light.intensity = originValue;
        intensityValue=originValue;
    }

    public void SetHeadLightFilledSlowly(bool b)
    {
        isCloseToBornfire = b;
    }

    public void SetHeadLightFilledQuickly()
    {
        isQuickFillLight = true;
    }
}
