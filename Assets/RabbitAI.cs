using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour
{

    public GameObject rabbit;
    public GameObject entity;
    private GameObject character;
    private GameObject characterLight;
   // private Globals script;
    public Animator animator;
    private int counter = 0;
    private bool delayedStop;
    private bool triggered;
    private Vector3 resetPosition;
    private SpriteRenderer rabbitRenderer;


    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        //script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        character = GameObject.Find("Character");
        characterLight = character.gameObject.transform.Find("Night-Light").gameObject;
        delayedStop = false;
        triggered = false;
        resetPosition = rabbit.transform.position;
        rabbitRenderer = entity.gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
        /*
        Vector3 vecRabbitNew = new Vector3();
        if(character.transform.position.x < rabbit.transform.position.x)
        {
            vecRabbitNew.y = rabbit.transform.position.y;
            vecRabbitNew.z = rabbit.transform.position.z;
            vecRabbitNew.x = rabbit.transform.position.x + (float)0.1;
        }
        if (character.transform.position.x > rabbit.transform.position.x)
        {
            vecRabbitNew.y = rabbit.transform.position.y;
            vecRabbitNew.z = rabbit.transform.position.z;
            vecRabbitNew.x = rabbit.transform.position.x - (float)0.1;
        }
        if (character.transform.position.y < rabbit.transform.position.y)
        {
            vecRabbitNew.y = rabbit.transform.position.y + (float)0.1;
            vecRabbitNew.z = rabbit.transform.position.z;
            vecRabbitNew.x = rabbit.transform.position.x;
        }
        if (character.transform.position.y > rabbit.transform.position.y)
        {
            vecRabbitNew.y = rabbit.transform.position.y - (float)0.1;
            vecRabbitNew.z = rabbit.transform.position.z;
            vecRabbitNew.x = rabbit.transform.position.x;
        }
        rabbit.transform.position = vecRabbitNew;*/
        
        Debug.Log("enter");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        delayedStop = true;
        triggered = false;
        Debug.Log("exit");
        /*animator.SetBool("TriggerJump", false);
        Vector3 vecRabbitNew = new Vector3();
        vecRabbitNew.y = entity.transform.position.y;
        vecRabbitNew.z = entity.transform.position.z;
        vecRabbitNew.x = (float)0.425 + rabbit.transform.position.x;
        rabbit.transform.position = vecRabbitNew;*/
    }

    public void setParent()
    {
        //Debug.Log("Animation end");
        Vector3 vecRabbitNew = new Vector3();
        vecRabbitNew.y = entity.transform.position.y;
        vecRabbitNew.z = 0;
        vecRabbitNew.x = (float)0.425 + rabbit.transform.position.x;
        rabbit.transform.position = vecRabbitNew;
        if (delayedStop)
        {
            delayedStop = false;
            animator.SetBool("TriggerJump", false);
            animator.SetBool("TriggerJump2", false);
        }
    }

    public void setParent2()
    {
        //Debug.Log("Animation end");
        Vector3 vecRabbitNew = new Vector3();
        vecRabbitNew.y = entity.transform.position.y;
        vecRabbitNew.z = 0;
        vecRabbitNew.x = (float)-0.425 + rabbit.transform.position.x;
        rabbit.transform.position = vecRabbitNew;
        if (delayedStop)
        {
            delayedStop = false;
            animator.SetBool("TriggerJump", false);
            animator.SetBool("TriggerJump2", false);
        }
    }

    public void OnBecameInvisible()
    {
        rabbit.transform.position = resetPosition;
    }

    private void Update()
    {

        if (!rabbitRenderer.isVisible)
        {
            rabbit.transform.position = resetPosition;
        }

        if (characterLight.gameObject.activeSelf)
        {
            animator.SetBool("NightTime", true);
        }
        else
        {
            animator.SetBool("NightTime", false);
        }

        if (triggered)
        {
            if (character.transform.position.x < rabbit.transform.position.x)
            {
                animator.SetBool("TriggerJump", true);
                animator.SetBool("TriggerJump2", false);
            }
            if (character.transform.position.x >= rabbit.transform.position.x)
            {
                animator.SetBool("TriggerJump", false);
                animator.SetBool("TriggerJump2", true);
            }
        }
        
        /*
        Vector3 vecRabbitNew = new Vector3();
        if (counter < 5)
        {
            counter++;
            vecRabbitNew.y = rabbit.transform.position.y;
            vecRabbitNew.z = rabbit.transform.position.z;
            vecRabbitNew.x = rabbit.transform.position.x + (float)0.1;
        }
        if( counter == 5)
        {
            rabbit.GetComponent<SpriteRenderer>().flipX = true;
        }
        if(counter >= 5 && counter < 15)
        {
            counter++;
            vecRabbitNew.y = rabbit.transform.position.y;
            vecRabbitNew.z = rabbit.transform.position.z;
            vecRabbitNew.x = rabbit.transform.position.x - (float)0.1;
        }
        if(counter >= 15 && counter < 20)
        {
            counter++;
            vecRabbitNew.y = rabbit.transform.position.y;
            vecRabbitNew.z = rabbit.transform.position.z;
            vecRabbitNew.x = rabbit.transform.position.x + (float)0.1;
        }
        if(counter == 15)
        {
            rabbit.GetComponent<SpriteRenderer>().flipX = false;
        }
        if(counter == 20)
        {
            counter = 0;
        }
        rabbit.transform.position = vecRabbitNew;
        */
    }
}
