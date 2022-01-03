using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.LWRP;

public class SpriteController : MonoBehaviour
{
    public Rigidbody2D body;

    public float horizontal;
    public float vertical;
    float moveLimiter = 0.7f;

    public Vector2 lastClickedPos;
    public bool moving;

    bool movingHorizontal;
    bool movingVertical;
    public float xPos, yPos = 0;

    public float runSpeed;
    public float runSpeedBoost;
    public float runSpeedNormal;
    public bool boost;

    public Animator animator;
    public GameObject lightObject;
    private UnityEngine.Experimental.Rendering.Universal.Light2D light;

    public bool disableMoving;

    public bool isBelowBridge;
    public int isBehindTree;
    public bool isOnTopOfRock;
    public bool isOnSecondFloorRock;
    public bool isBehindRock;

    public int charLayerID;
    public int lastCharLayerID;
    public string lastCharLayer;
    private AudioLibrary audioController;

    public string World;
    public string Dungeon;
    public int Chunk;
    public int nodeID;

    public float deadzoneMovementSize = 0.1f;
    public bool inDeadzone = false;
    public bool deadzoneMove = false;

    public float last_xpos;
    public float last_ypos;

    public bool lastDirection;

    public bool disableAllMove;

    public List<int> layerIDqueue;
    public int defaultLayerID = 20;
    public string charLayer = "Overworld";
    public GameObject charOutline;
    public Animator charOutlineAnimator;


    void Start()
    {
        runSpeed = runSpeedNormal;
        boost = false;
        //body = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        light = lightObject.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        disableMoving = false;
        moving = false;
        isBelowBridge = false;
        isBehindTree = 0;
        isOnTopOfRock = false;
        audioController = GameObject.Find("Audio").GetComponent<AudioLibrary>();
        deadzoneMovementSize = 0.1f;
        inDeadzone = false;
        disableAllMove = false;
        layerIDqueue = new List<int>();
    }

    public void changeFloor(int floor)
    {
        if(floor == 0)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Overworld";
            charLayer = "Overworld";
            gameObject.layer = 13;
        }
        else if(floor == 1)
        {

            GetComponent<SpriteRenderer>().sortingLayerName = "OverworldFloor1";
            charLayer = "OverworldFloor1";
            gameObject.layer = 22;
        }
        else if(floor == 2)
        {

            GetComponent<SpriteRenderer>().sortingLayerName = "OverworldFloor2";
            charLayer = "OverworldFloor2";
            gameObject.layer = 23;
        }
    }

    public void signalLayer()
    {
        layerIDqueue.Sort();
    }

    void FixedUpdate()
    {
        if(isBehindTree == 0)
        {
            charOutline.SetActive(false);
            //GetComponent<SpriteRenderer>().sortingLayerName = charLayer;
            //GetComponent<SpriteRenderer>().sortingOrder = defaultLayerID;
        }
        else
        {
            charOutline.SetActive(true);
            //GetComponent<SpriteRenderer>().sortingOrder = layerIDqueue[0];
        }
        Vector2 targetPosition;
        xPos = 0;
        yPos = 0;
        if (disableAllMove) return;
        if (Input.GetMouseButton(0) && inDeadzone) return;
        //Function to move character on mouse click/hold.
        if (Input.GetMouseButton(0) && !disableMoving)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return; //prevent ui clicks



            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //y= a*x + b -> y = x
            //y = - x
            //normalize clicked pos according to transform pos. to 0,0 axis
            float norm_x = lastClickedPos.x - transform.position.x;
            float norm_y = lastClickedPos.y - transform.position.y;
            //determine quartile
            bool normal_line = true; //if clicked is on 1st or 3rd cartesian quartile

            if (lastClickedPos.x > transform.position.x) horizontal = 1;
            else horizontal = -1;
            if (lastClickedPos.y > transform.position.y) vertical = 1;
            else vertical = -1;

            if (horizontal != vertical) normal_line = false; //determine here

            quartiles(norm_x, norm_y, normal_line);


            
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            if(Mathf.Abs(norm_x)>=deadzoneMovementSize || Mathf.Abs(norm_y) >= deadzoneMovementSize)
            {
                targetPosition = Vector2.MoveTowards(transform.position, lastClickedPos, runSpeed * Time.deltaTime);
                body.MovePosition(targetPosition);
            }
            //animator.SetFloat("Horizontal", horizontal);
            updateAnimator("Horizontal", "Vertical", horizontal, vertical);
            //animator.SetFloat("Vertical", vertical);
            moving = true;
        }
        else //move with arrow keys no diagonal movement
        {
            moving = false; //with mouse
            // Gives a value between -1 and 1
            if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0)
            {
                //move only to the last key holded
                if (movingHorizontal)
                {
                    yPos = Input.GetAxisRaw("Vertical");
                    lastDirection = true; //where true = vertical
                }
                else if (movingVertical)
                {
                    xPos = Input.GetAxisRaw("Horizontal");
                    lastDirection = false; //where true = vertical
                }
            }
            else
            {
                movingHorizontal = Input.GetAxisRaw("Horizontal") != 0;
                xPos = Input.GetAxisRaw("Horizontal");
                movingVertical = Input.GetAxisRaw("Vertical") != 0;
                yPos = Input.GetAxisRaw("Vertical");
            }
            //for sprite flipping
            horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
            vertical = Input.GetAxisRaw("Vertical"); // -1 is down
            targetPosition.x = xPos;
            targetPosition.y = yPos;
            body.MovePosition(body.position + targetPosition * runSpeed * Time.deltaTime);

            updateAnimator("Horizontal", "Vertical", xPos, yPos);
            //animator.SetFloat("Horizontal", xPos);
            //animator.SetFloat("Vertical", yPos);
        }

        //not moving
        if(horizontal == 0 && vertical == 0)
        {
            if(last_xpos == 0 && last_ypos == 0)
            {
                updateAnimator("idle_Horizontal", "idle_Vertical", 0, -0.1f);
                //animator.SetFloat("idle_Horizontal", 0);
                //animator.SetFloat("idle_Vertical", -0.1f);
            }
            else
            {
                //if both keys released at the same time
                if(last_xpos != 0 && last_ypos != 0){
                    if (lastDirection)
                    {//last was vertical
                        last_xpos = 0;
                    }
                    else
                    {
                        last_ypos = 0;
                    }
                }
                updateAnimator("idle_Horizontal", "idle_Vertical", last_xpos, last_ypos);
                //animator.SetFloat("idle_Horizontal", last_xpos);
                //animator.SetFloat("idle_Vertical", last_ypos);
            }

        }
        else
        {
            last_xpos = horizontal;
            last_ypos = vertical;

            updateAnimator("idle_Horizontal", "idle_Vertical", 0, 0);
            //animator.SetFloat("idle_Horizontal", 0);
            //animator.SetFloat("idle_Vertical", 0);
        }
        

        //to prevent down and up animation from playing while
        //going diagoniaclly 
        //if (horizontal == 0) animator.SetBool("verticalOnly", true);
        //else animator.SetBool("verticalOnly", false);
        

      

        if (moving)
        {
            if (Time.timeSinceLevelLoad - _lastPlayedFootstepSoundTime > _timeBetweenFootsteps)
            {
                audioController.playFootstep();
                _lastPlayedFootstepSoundTime = Time.timeSinceLevelLoad;
            }
            
        }
    }

    const float _timeBetweenFootsteps = 0.5f;
    float _lastPlayedFootstepSoundTime = -_timeBetweenFootsteps;


    private void updateAnimator(string item1, string item2, float val1, float val2)
    {
        animator.SetFloat(item1, val1);
        animator.SetFloat(item2, val2);
        charOutlineAnimator.SetFloat(item1, val1);
        charOutlineAnimator.SetFloat(item2, val2);
    }

    private void quartiles(float norm_x, float norm_y, bool normal_line)
    {
        if (normal_line) // y = x line
        {
            if (norm_y > norm_x)
            { // then clicked above the line
                if (norm_x < 0)
                {//then its on 3rd quartile -> LEFT
                    horizontal = -1;
                    vertical = 0;
                }
                else
                {//its on 1st quartile -> UP
                    horizontal = 0;
                    vertical = 1;
                }

            }
            else
            {// clicked below the line
                if (norm_x < 0)
                {//then its on 3rd quartile -> DOWN 
                    horizontal = 0;
                    vertical = -1;
                }
                else
                {//its on 1st quartile -> RIGHT
                    horizontal = 1;
                    vertical = 0;
                }
            }
        }
        else
        {// y = -x line
            if (norm_y > (norm_x * -1) )
            { // then clicked above the line
                if (norm_x < 0)
                {//then its on 4th quartile -> UP
                    horizontal = 0;
                    vertical = 1;
                }
                else
                {//its on 2nd quartile -> RIGHT
                    horizontal = 1;
                    vertical = 0;
                }

            }
            else
            {// clicked below the line
                if (norm_x < 0)
                {//then its on 4th quartile -> LEFT 
                    horizontal = -1;
                    vertical = 0;
                }
                else
                {//its on 2nd quartile -> DOWN
                    horizontal = 0;
                    vertical = -1;
                }
            }
        }
    }
}
