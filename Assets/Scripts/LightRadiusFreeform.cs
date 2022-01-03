using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightRadiusFreeform : MonoBehaviour
{
    private CustomFixedUpdate FU_instance;
    public Light2D light;
    public float minimum_Falloff;
    public float maximum_Falloff;
    private float[] arrFalloff;
    private int counter;

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(0.1f, OnFixedUpdate);
    }

    void Start()
    {
        int max = 10;
        arrFalloff = new float[max];
        for (int i = 0; i < max / 2; i++)
        {
            arrFalloff[i] = minimum_Falloff + (i) * (maximum_Falloff - minimum_Falloff) / ((max) / 2f);
        }
        for (int i = 0; i < max / 2; i++)
        {
            arrFalloff[max - (i + 1)] = minimum_Falloff + (i) * (maximum_Falloff - minimum_Falloff) / ((max) / 2f);
        }
        
        counter = 0;
    }
    // Update is called once per frame
    void Update()
    {
        FU_instance.Update();
    }

    // this method will be called 10 times per second
    void OnFixedUpdate(float dt)
    {
        if (counter == 9)
        {
            counter = 0;
        }
        counter++;
        //unity hasnt implemented
        //light.falloffIntensity = arrFalloff[counter];

    }
}
