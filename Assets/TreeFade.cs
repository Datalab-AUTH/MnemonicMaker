using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFade : MonoBehaviour
{
    public GameObject tree;
    private GameObject character;
    private SpriteRenderer characterRend;
    private SpriteController characterSprCont;

    private void Start()
    {
        character = GameObject.Find("Character");

        characterRend = character.GetComponent<SpriteRenderer>();
        characterSprCont = character.GetComponent<SpriteController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Color tmp = tree.GetComponent<SpriteRenderer>().color;
        //tmp.a = 0.3f;
        //tree.GetComponent<SpriteRenderer>().color = tmp;
        //tree.GetComponent<SpriteRenderer>().sortingLayerName = "Over Character";

            if (characterSprCont.isBehindTree == 0)
            {
                characterSprCont.charLayerID = characterRend.sortingOrder;
                characterSprCont.charLayer = character.GetComponent<SpriteRenderer>().sortingLayerName;
            }
            //apply new
            if(characterRend.sortingOrder > tree.GetComponent<SpriteRenderer>().sortingOrder)
            {
            characterRend.sortingOrder = tree.GetComponent<SpriteRenderer>().sortingOrder;
                character.GetComponent<SpriteRenderer>().sortingLayerName = tree.GetComponent<SpriteRenderer>().sortingLayerName;
            }
            
            Debug.LogWarning(tree.gameObject.name + " enter");
            characterSprCont.isBehindTree++;
        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Color tmp = tree.GetComponent<SpriteRenderer>().color;
        //tmp.a = 1f;
        //tree.GetComponent<SpriteRenderer>().color = tmp;
        //tree.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            characterSprCont.isBehindTree--;
        if (characterSprCont.isBehindTree == 0)
        {
            characterRend.sortingOrder = characterSprCont.charLayerID;
            character.GetComponent<SpriteRenderer>().sortingLayerName = characterSprCont.charLayer;
            Debug.LogWarning(tree.gameObject.name + " exit");
        }
        
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (characterRend.sortingOrder > tree.GetComponent<SpriteRenderer>().sortingOrder)
        {
            characterRend.sortingOrder = tree.GetComponent<SpriteRenderer>().sortingOrder;
            character.GetComponent<SpriteRenderer>().sortingLayerName = tree.GetComponent<SpriteRenderer>().sortingLayerName;
        }
        
    }
}
