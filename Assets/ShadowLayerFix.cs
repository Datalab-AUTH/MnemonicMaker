using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowLayerFix : MonoBehaviour
{
    public GameObject character;

    // Update is called once per frame
    void Update()
    {
        gameObject.layer = character.layer;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = character.GetComponent<SpriteRenderer>().sortingOrder - 1;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = character.GetComponent<SpriteRenderer>().sortingLayerName;
    }
}
