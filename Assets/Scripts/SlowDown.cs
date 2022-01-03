using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
    private GameObject character;
    private float runSpeedTMP ;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        character = GameObject.Find("Character");
        SpriteController sc = character.gameObject.GetComponent<SpriteController>();
        runSpeedTMP = sc.runSpeed;
        sc.runSpeed = sc.runSpeed / 2f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        character = GameObject.Find("Character");
        SpriteController sc = character.gameObject.GetComponent<SpriteController>();
        sc.runSpeed = runSpeedTMP;

    }
}
