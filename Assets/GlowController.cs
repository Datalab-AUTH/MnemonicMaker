using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowController : MonoBehaviour
{
    public Material defaultNonGlow;
    public Material glow;
    
    public void setDefaultMaterial()
    {
        transform.GetComponent<SpriteRenderer>().material = defaultNonGlow;
        //should only look for childs of the parent GO.
        transform.Find("MinimapTarget").GetComponent<SpriteRenderer>().material = defaultNonGlow;
    }

    public void setGlowMaterial()
    {
        transform.GetComponent<SpriteRenderer>().material = glow;
        //should only look for childs of the parent GO.
        transform.Find("MinimapTarget").GetComponent<SpriteRenderer>().material = glow;
    }
}
