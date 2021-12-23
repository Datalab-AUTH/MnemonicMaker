using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinsController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject character;
    private BIP39Game bipRef;

    public GameObject targetPin;
    public GameObject playerPin;

    public bool mapOpen;

    private void Start()
    {
        bipRef = GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39;
        //ResetCamera();
        mapOpen = false;
    }

    public void updateMap()
    {
        mapOpen = true;
        int x_offset = 0;
        //int y_offset = 0;
        if (!bipRef.day)
        {
            x_offset = 588;
        }
        float char_x = character.transform.position.x;
        float char_y = character.transform.position.y;

        float pin_x = (((char_x-x_offset) * 2483) / 196) - 2483/2;
        float pin_y = ((char_y * 1030.542f) / 81f) - 1030.542f/2;

        playerPin.transform.localPosition = new Vector3(pin_x, pin_y, playerPin.transform.localPosition.z);
    }

    public void closeMap()
    {

        mapOpen = false;
    }


    public Camera camera;
    public BoxCollider2D cameraBounds;
    public float panSpeed = 2;
    private Vector3 dragOrigin;

    public float zoomMin = 3f;
    public float zoomMax = 20f;
    public float zoomAmount = 1f;
    public float moveSmooth = 0.1f;


    private float min_x = -38;
    private float max_x = -1.8f;
    private float min_y = -7.5f;
    private float max_y = 7.5f;

    public float scroll;        //Input axis of your scroll wheel.
    public float addedSpeed = 0.4f;    //Input axis doesnt have enough power to make camera zoom properly so we add it
    public float cameraSpeed = 1f;   //Lerp speed (PLAY AROUND WITH IT!)

    public RectTransform bounds;

    ////////
    // Awake
    void Awake()
    {
        // DontDestroyOnLoad( this ); if( FindObjectsOfType( GetType() ).Length > 1 ) Destroy( gameObject );
        ResetCamera();
    }

    ////////////////
    // RESET: Camera
    public void ResetCamera()
    {
        //// Camera Origin
        camera.orthographicSize = 7f;
        //height = 2f * camera.orthographicSize;
        //width = height * camera.aspect;
        camera.transform.position = new Vector3(-21f, 0, 0);
        cameraBounds.size = new Vector2(camera.aspect * 2f * camera.orthographicSize, 2f * camera.orthographicSize);
    }

    /////////
    // Update
    void Update()
    {
        if (mapOpen)
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (camera.orthographicSize > zoomMin)
                {
                    /*
                    Vector2 mouse = camera.ScreenToWorldPoint(Input.mousePosition);
                    Debug.Log("Target mouse: x: " + camera.ScreenToWorldPoint(Input.mousePosition).x + " y: " + camera.ScreenToWorldPoint(Input.mousePosition).y);
                    Debug.Log("Camera center: x: " + camera.transform.position.x + " y: " + camera.transform.position.y);

                    camera.transform.position = new Vector3(mouse.x, mouse.y, 0);*/

                    camera.orthographicSize = camera.orthographicSize - zoomAmount;
                    cameraBounds.size = new Vector2(camera.aspect * 2f * camera.orthographicSize, 2f * camera.orthographicSize);
                }
                else
                {
                    camera.orthographicSize = zoomMin;
                }
            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (camera.orthographicSize < zoomMax)
                {
                    camera.orthographicSize = camera.orthographicSize + zoomAmount;
                    cameraBounds.size = new Vector2(camera.aspect * 2f * camera.orthographicSize, 2f * camera.orthographicSize);

                }
                else
                {
                    camera.orthographicSize = zoomMax;
                }
            }
            
            //// Moving of camera via click / drag
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                dragOrigin = camera.ScreenToWorldPoint(dragOrigin);
            }

            if (Input.GetMouseButton(0))
            {
                //Vector3 currentPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                //currentPos = camera.ScreenToWorldPoint(currentPos);
                //Vector3 movePos = dragOrigin - currentPos;
                //transform.position += (movePos * moveSmooth);

                Vector3 currentPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                currentPos = camera.ScreenToWorldPoint(currentPos);
                Vector3 movePos = dragOrigin - currentPos;
                Vector3 newPos = new Vector3(camera.transform.position.x + movePos.x, camera.transform.position.y + movePos.y, camera.transform.position.z);
                if (newPos.x >= min_x && newPos.x <= max_x &&
                    newPos.y >= min_y && newPos.y <= max_y)
                {
                    
                    camera.transform.position += (movePos * moveSmooth);
                }
                
            }
        }
        
    }

    public void cameraLook()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        Physics.Raycast(ray, out point, 25);
        Vector3 Scrolldirection = ray.GetPoint(5);

        ///float step = zoomAmount * Time.deltaTime;
        Debug.Log("Target mouse: x: " + Scrolldirection.x + " y: " + Scrolldirection.y);
        Debug.Log("Camera center: x: " + camera.transform.position.x + " y: " + camera.transform.position.y);
        Scrolldirection.z = camera.transform.position.z;
        //camera.transform.position = Scrolldirection;
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, Scrolldirection, Time.deltaTime);
    }

}

