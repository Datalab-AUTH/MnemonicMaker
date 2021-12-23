using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightRadius : MonoBehaviour
{
    private CustomFixedUpdate FU_instance;
    public Light2D light;
    public float minimum_Outer;
    public float maximum_Outer;
    public float minimum_Inner;
    public float maximum_Inner;
    private float[] arrOuter;
    private float[] arrInner;
    private int counter;

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(0.1f, OnFixedUpdate);
    }

    void Start()
    {
        int max = 10;
        arrOuter = new float[max];
        for(int i=0; i<max/2; i++)
        {
            arrOuter[i] = minimum_Outer + (i) * (maximum_Outer - minimum_Outer) /((max)/2f) ;
        }
        for (int i = 0; i < max / 2; i++)
        {
            arrOuter[max-(i+1)] = minimum_Outer + (i) * (maximum_Outer - minimum_Outer) / ((max) / 2f);
        }
        
        arrInner = new float[max];
        for (int i = 0; i < 5; i++)
        {
            arrInner[i] = minimum_Inner + (i) * (maximum_Inner - minimum_Inner) / ((max) / 2f);
        }
        for (int i = 0; i < 5; i++)
        {
            arrInner[max - (i + 1)] = minimum_Inner + (i) * (maximum_Inner - minimum_Inner) / ((max) / 2f);
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
        if(counter == 9)
        {
            counter = 0;
        }
        counter++;
        light.pointLightInnerRadius = arrInner[counter];
        light.pointLightOuterRadius = arrOuter[counter];
        
    }
}
