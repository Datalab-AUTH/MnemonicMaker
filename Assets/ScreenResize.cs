using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScreenResize : MonoBehaviour
{
    private bool triggeredResize;
    private int oldH;
    private int oldW;
    private int newH;
    private int newW;


    int frameCounter;

    private void Start()
    {
        frameCounter = 0;
        triggeredResize = false;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (!triggeredResize)
        {
            triggeredResize = true;
            Resolution resolution = Screen.currentResolution;
            oldH = Screen.height;
            oldW = Screen.width;
            newH = oldH;
            newW = oldW;
        }
        
    }

    private void changeResolution()
    {
        newH = Screen.height;
        newW = Screen.width;
        Debug.Log("Resize - Start W: " + oldW + " H: " + oldH);
        Debug.Log("Resize - End W: " + newW + " H: " + newH);
        int height=0;
        int width=0;
        bool resize = false;
        if (newH % 2 != 0)
        {
            height = newH - 1;
            resize = true;
        }
        else
        {
            height = newH;
        }
        if (newW % 2 != 0)
        {
            width = newW - 1;
            resize = true;
        }
        else
        {
            width = newW;
        }
        if (resize)
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
            Debug.Log("Fix - W: " + width + " H: " + height);
        }
        else
        {
            Debug.Log("Window Resolution is correct.");
        }
    }

    private void Update()
    {

        if (triggeredResize)
        {
            int currentH = Screen.height;
            int currentW = Screen.width;
            if(newH == currentH && newW == currentW)
            {
                frameCounter++;
            }
            else
            {
                frameCounter = 0;
            }
            newH = currentH;
            newW = currentW;
            if (frameCounter == 60)
            {
                triggeredResize = false;
                changeResolution();
                frameCounter = 0;
            }
        }
    }

    // Adjust via the Inspector
    public int maxLines = 8;
    private Queue<string> queue = new Queue<string>();
    private string currentText = "";

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Delete oldest message
        if (queue.Count >= maxLines) queue.Dequeue();

        queue.Enqueue(logString);

        var builder = new StringBuilder();
        foreach (string st in queue)
        {
            builder.Append(st).Append("\n");
        }

        currentText = builder.ToString();
    }

    void OnGUI()
    {
        GUI.Label(
           new Rect(
               Screen.width- 350,                   // x, left offset
               Screen.height/2 - 75, // y, bottom offset
               300f,                // width
               150f                 // height
           ),
           currentText,             // the display text
           GUI.skin.textArea        // use a multi-line text area
        );
    }
}
