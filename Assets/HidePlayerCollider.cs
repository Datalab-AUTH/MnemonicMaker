using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlayerCollider : MonoBehaviour
{
    private GameObject character;
    private SpriteController characterSprCont;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
        characterSprCont = character.GetComponent<SpriteController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        character.GetComponent<SpriteRenderer>().sortingOrder = 4;
        character.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (characterSprCont.isBehindTree == 0)
        {
            character.GetComponent<SpriteRenderer>().sortingOrder = 20;
            character.GetComponent<SpriteRenderer>().sortingLayerName = "Overworld";
        }
    }
}
