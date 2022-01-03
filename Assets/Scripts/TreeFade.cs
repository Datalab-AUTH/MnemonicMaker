using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFade : MonoBehaviour
{
    //public GameObject tree;
    private GameObject character;
    private SpriteRenderer characterRend;
    private SpriteController characterSprCont;

    private void Start()
    {

        //characterRend = character.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Trim() == "Character")
        {
            character = GameObject.Find("Character");
            characterSprCont = character.GetComponent<SpriteController>();
            characterSprCont.isBehindTree++;
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Trim() == "Character")
        {
            character = GameObject.Find("Character");
            characterSprCont = character.GetComponent<SpriteController>();
            characterSprCont.isBehindTree--;
        }
    }
}
