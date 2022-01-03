using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Globals : MonoBehaviour
{
    // GameObject.Find("ScriptLoader").GetComponent<CinemachineFader>();
    //script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
    public GameObject itemPanel;
    public Text itemDisplay;
    public GameObject inventory;
    public ChestInventory chestInventory;
    public Inventory inventoryScript;
    public ChunkController chunkController;
    public BIP39Game bip39;
    public SpriteController spriteController;
    public Sprite transparent;
    public GameObject dummyDrag;
    public Camera UICamera;
    public CinemachineVirtualCamera activeCV; //cinemachine keyword
    public MainMenu mainMenuScript;
    public BitcoinController bitcoinController;
    public GameObject pathfinder;

    void Start()
    {
        itemPanel.SetActive(false);
        inventory.SetActive(false);
        pathfinder.SetActive(false);
#if UNITY_EDITOR
        Debug.Log("Unity Editor detected");
#endif

#if UNITY_WEBGL
        Debug.Log("WEBGL detected");
        Debug.Log(" [START WEBGL] W: " + Screen.width + " H: " + Screen.height + " RR: " + Screen.currentResolution.refreshRate);
        //Screen.SetResolution(1280, 720, FullScreenMode.ExclusiveFullScreen);
        StartCoroutine(DelayAction(2f));
#endif

#if UNITY_STANDALONE_WIN
        Debug.Log("WINDOWS detected");
        Debug.Log(" [START] W: " + Screen.width + " H: " + Screen.height + " RR: "+ Screen.currentResolution.refreshRate);
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed, Screen.currentResolution.refreshRate);
        Debug.Log(" [CHANGE] W: 1280 H: 720 RR: " + Screen.currentResolution.refreshRate);
#endif
    }

    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Debug.Log(" [CHANGE WEBGL] W: " + Screen.width + " H: " + Screen.height + " RR: " + Screen.currentResolution.refreshRate);
    }
}
