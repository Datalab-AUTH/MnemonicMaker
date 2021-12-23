using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCasterFix : MonoBehaviour
{
    public GameObject shadowCaster1;
    public GameObject shadowCaster2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait(1f));
        StartCoroutine(wait2(1.5f));
    }

    IEnumerator wait(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        shadowCaster1.SetActive(false);
        shadowCaster2.SetActive(false);
    }

    IEnumerator wait2(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        shadowCaster1.SetActive(true);
        shadowCaster2.SetActive(true);
    }

}
