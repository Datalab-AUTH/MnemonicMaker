using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepController : MonoBehaviour
{
    public GameObject UI;
    private bool isInRange;
    private Animator animator;
    private Globals script;

    // Start is called before the first frame update
    void Start()
    {
        isInRange = false;
        animator = UI.GetComponent<Animator>();
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.Z))
        {

            //Animation with a fade in circle

            //Animation with ZZZZZzzzZZZ

            //Custom warp to other main house.
            Portal portal = GameObject.Find("DayNightPortal").GetComponent<Portal>();
            //set bip39game game.day false
            if (script.bip39.day)
            {
                //Control the global lights
                script.bip39.day = false;
                portal.warp(portal.entryNode.gateID);
            }
            else
            {
                //Control the global lights
                script.bip39.day = true;
                portal.warp(portal.exitNode.gateID);

            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        animator.SetBool("IsNear", true);
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("IsNear", false);
        isInRange = false;
    }
}
