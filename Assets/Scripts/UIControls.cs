using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    public GameObject inventory;
    public GameObject pathfindPanel;
    public GameObject gameMap;
    public MapPinsController gameMapPins;
    public GameObject helpPanel;
    private Animator animator;
    private Animator animatorPathfind;
    private Animator animatorHelpPanel;
    private Pathfinding pathfindScript;

    public Camera MinimapCamera;
    public Camera MapCamera;

    private bool visibleHelp;
    private bool settingsVisible = false;
    private Globals script;

    public bool lockControls;

    private SpriteController character;
    private Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        lockControls = true;
        //Debug.Log("Inventory run");
        animator = inventory.GetComponent<Animator>();
        animatorPathfind = pathfindPanel.GetComponent<Animator>();
        pathfindScript = pathfindPanel.GetComponent<Pathfinding>();
        animatorHelpPanel = helpPanel.GetComponent<Animator>();
        //inventory.SetActive(false);
        visibleHelp = false;
        settingsVisible = false;
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        MapCamera.transform.gameObject.SetActive(false);
        character = GameObject.Find("Character").GetComponent<SpriteController>();
        inv = inventory.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (script.mainMenuScript.creditsPlaying)
        {
            if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            {
                script.mainMenuScript.stopTheCredits();
            }
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.O)) && !settingsVisible)
            {
                settingsVisible = true;
                script.mainMenuScript.openSettingsWindow();
            }
            else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.O)) && settingsVisible)
            {
                settingsVisible = false;
                script.mainMenuScript.returnSettingsButton();
            }
            if (!lockControls)
            {
                if (Input.GetKeyDown(KeyCode.I) && inventory.activeSelf)
                {
                    //UI Destory manages 
                    animator.Play("InventoryHide");
                }
                else if (Input.GetKeyDown(KeyCode.I) && !inventory.activeSelf)
                {
                    //Debug.Log("Inventory show?");
                    inventory.SetActive(true);
                    animator.Play("InventoryShow");
                }
                if (Input.GetKeyDown(KeyCode.P) && pathfindPanel.activeSelf && pathfindScript.pathfindPanelActive)
                {
                    animatorPathfind.Play("PathfindHide");
                    pathfindScript.pathfindPanelActive = false;
                }
                else if (Input.GetKeyDown(KeyCode.P) && pathfindPanel.activeSelf && !pathfindScript.pathfindPanelActive)
                {
                    animatorPathfind.Play("PathfindShow");
                    pathfindScript.pathfindPanelActive = true;
                }
                if (Input.GetKeyDown(KeyCode.M) && gameMap.activeSelf)
                {
                    if (character.Dungeon == "")
                    {
                        gameMapPins.closeMap();
                        MapCamera.transform.gameObject.SetActive(false);
                        gameMap.SetActive(false);
                        //UI Destory manages 
                        //animatorMinimap.Play("MinimapHide");
                    }
                }
                else if (Input.GetKeyDown(KeyCode.M) && !gameMap.activeSelf && inv.draggedID == -1)
                {
                    if (character.Dungeon == "")
                    {
                        gameMapPins.updateMap();
                        MapCamera.transform.gameObject.SetActive(true);
                        gameMap.SetActive(true);
                    }
                }
                if (Input.GetKeyDown(KeyCode.H) && !visibleHelp)
                {
                    animatorHelpPanel.Play("HelpPanelShow");
                    visibleHelp = true;
                }
                else if (Input.GetKeyDown(KeyCode.H) && visibleHelp)
                {
                    animatorHelpPanel.Play("HelpPanelHide");
                    visibleHelp = false;
                }

            }
        }
        
        

    }
    
}
