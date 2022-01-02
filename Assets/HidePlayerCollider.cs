using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlayerCollider : MonoBehaviour
{
    private GameObject character;
    private SpriteController characterSprCont;

    //public int applyLayerID = 0;
    //public string applyLayerName = "Overworld";

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
        characterSprCont = character.GetComponent<SpriteController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Trim()== "Character")
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
