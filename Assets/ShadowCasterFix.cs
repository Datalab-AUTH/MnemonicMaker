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
        if (!GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39.day)
        {
            StartCoroutine(wait(0.2f));
            StartCoroutine(wait2(0.3f));
        }
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
