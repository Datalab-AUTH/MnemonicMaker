using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rabbitRigid;
    private BIP39Game script;

    public float moveSpeed;
    public bool isWalking;
    public float walkTime;
    public float waitTime;
    private float walkCounter;
    private float waitCounter;
    private int walkDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39;
        rabbitRigid = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;
        chooseDirection();
    }

    private void chooseDirection()
    {
        walkDirection = Random.Range(0, 2);
        isWalking = true;
        walkCounter = walkTime;
    }

    private void Update()
    {
      
        
        if (!script.day)
        {
            animator.SetBool("NightTime", true);
            rabbitRigid.velocity = Vector2.zero;
        }
        else
        {
            animator.SetBool("NightTime", false);

            if (isWalking)
            {

                walkCounter -= Time.deltaTime;

                if (walkDirection == 0)
                {
                    rabbitRigid.velocity = new Vector2(moveSpeed, 0);
                    animator.SetBool("TriggerJump", true);
                    animator.SetBool("TriggerJump2", false);
                }
                else
                {
                    rabbitRigid.velocity = new Vector2(-moveSpeed, 0);
                    animator.SetBool("TriggerJump", false);
                    animator.SetBool("TriggerJump2", true);
                }


                if (walkCounter < 0)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }
            }
            else
            {
                waitCounter -= Time.deltaTime;
                rabbitRigid.velocity = Vector2.zero;
                if(walkDirection == 0)
                {
                    animator.SetBool("idle", true);
                    animator.SetBool("idle2", false);
                    animator.SetBool("TriggerJump", false);
                    animator.SetBool("TriggerJump2", false);
                }
                else
                {

                    animator.SetBool("idle", false);
                    animator.SetBool("idle2", true);
                    animator.SetBool("TriggerJump", false);
                    animator.SetBool("TriggerJump2", false);
                }
                if (waitCounter < 0)
                {
                    chooseDirection();
                }
            }

            return;
        }
    }
}
