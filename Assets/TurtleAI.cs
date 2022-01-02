using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAI : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D turtleRigid;
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
        turtleRigid = GetComponent<Rigidbody2D>();

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

        /*
        if (!script.day)
        {
            animator.SetBool("NightTime", true);
            rabbitRigid.velocity = Vector2.zero;
        }*/
        if (true)
        {
            animator.SetBool("NightTime", false);

            if (isWalking)
            {

                walkCounter -= Time.deltaTime;

                if (walkDirection == 0)
                {
                    turtleRigid.velocity = new Vector2(moveSpeed, 0);
                    animator.SetBool("walk", true);
                    animator.SetBool("walkLeft", false);
                }
                else
                {
                    turtleRigid.velocity = new Vector2(-moveSpeed, 0);
                    animator.SetBool("walk", false);
                    animator.SetBool("walkLeft", true);
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
                turtleRigid.velocity = Vector2.zero;
                if (walkDirection == 0)
                {
                    animator.SetBool("idle", true);
                    animator.SetBool("idleleft", false);
                    animator.SetBool("walk", false);
                    animator.SetBool("walkLeft", false);
                }
                else
                {

                    animator.SetBool("idle", false);
                    animator.SetBool("idleleft", true);
                    animator.SetBool("walk", false);
                    animator.SetBool("walkLeft", false);
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
