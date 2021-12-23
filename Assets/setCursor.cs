using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCursor : MonoBehaviour
{
    public Texture2D crosshair;
    [SerializeField] private Texture2D[] cursorAnimation;
    public int frameCount;
    public float framerate;

    private int currentFrame;
    private float frameTimer;

    // Start is called before the first frame update
    void Start()
    {
        //set the cursor origin to its centre. (default is upper left corner)
        //Vector2 cursorOffset = new Vector2(crosshair.width / 2, crosshair.height / 2);

        //Sets the cursor to the Crosshair sprite with given offset 
        //and automatic switching to hardware default if necessary
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);
        //Cursor.SetCursor(cursorAnimation[0], new Vector2(0,0), CursorMode.Auto);
    }

    private void Update()
    {
        /*
        frameTimer -= Time.deltaTime;
        if(frameTimer <= 0f)
        {
            frameTimer += framerate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorAnimation[currentFrame], new Vector2(0, 0), CursorMode.Auto);
        }
        */
    }

}
