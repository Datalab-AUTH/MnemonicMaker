using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private GameObject character;
    public bool below;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (below)
        {
            character.GetComponent<SpriteController>().isBelowBridge = true;
        }
        if (character.GetComponent<SpriteController>().isBehindTree == 0 &&
            !character.GetComponent<SpriteController>().isBehindRock)
        {
            if (below)
            {
                character.GetComponent<SpriteRenderer>().sortingOrder = 3;
                character.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            }
            else
            {
                character.gameObject.layer = 11;
            }
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (below)
        {
            character.GetComponent<SpriteRenderer>().sortingOrder = 20;
            character.GetComponent<SpriteRenderer>().sortingLayerName = "Overworld";
            character.GetComponent<SpriteController>().isBelowBridge = false;
        }
        else
        {
            character.gameObject.layer = 13;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (below)
        {
            character.GetComponent<SpriteController>().isBelowBridge = true;
        }
        if (character.GetComponent<SpriteController>().isBehindTree == 0 && 
            !character.GetComponent<SpriteController>().isBehindRock)
        {
            if (below)
            {
                character.GetComponent<SpriteRenderer>().sortingOrder = 3;
                character.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            }
            else
            {
                character.gameObject.layer = 11;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
