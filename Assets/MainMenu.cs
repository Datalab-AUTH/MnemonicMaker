using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public RectTransform introScreen;
    public RectTransform wordLengthScreen;
    public RectTransform listScreen;
    public RectTransform UIPanel;
    public RectTransform itemSelection;
    public RectTransform settingsPanel;
    private BIP39Game bipReference;


    public Text resolutionText;
    public GameObject fullscreenBoolean;
    public GameObject windowedBoolean;

    private List<(int, int)> resolutions;
    private int currentResolutionID;
    private FullScreenMode currentScreenMode;
    private AudioLibrary audioController;

    public GameObject endPrompt;
    private bool endPromptVisible;
    public Button extractButton;
    private BoatIntro biRef;
    // Start is called before the first frame update
    void Start()
    {
        endPromptVisible = false;
        bipReference = GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39;
        resolutions = new List<(int, int)>();
        //resolutions.Add((320, 180));
        resolutions.Add((640, 360));
        resolutions.Add((960, 540));
        Resolution[] res = Screen.resolutions;
        foreach(Resolution reso in res)
        {
            float w = reso.width / 16f;
            float h = reso.height / 9f;
            if(h == w)
            {
                //Debug.Log(" [16:9 SUPPORTED] W: " + reso.width + " H: " + reso.height);
                if (!resolutions.Contains((reso.width,reso.height)))
                {
                    resolutions.Add((reso.width, reso.height));
                }
            }
            else
            {
                //Debug.Log(" [INCOMPATIBLE ASPECT RATIO] W: " + reso.width + " H: " + reso.height);
            }
        }

        int tempRes = 0;
        int tempCounter = 0;
        bool defaultResolutionFound = false;
        foreach((int,int) resolution in resolutions)
        {
            Debug.Log(" [ADDED RESOLUTION] W: " + resolution.Item1 + " H: " + resolution.Item2);
            if(resolution.Item1 == 1280 && resolution.Item2 == 720)
            {
                tempRes = tempCounter;
                defaultResolutionFound = true;
                Debug.Log(" [DEFAULT RESOLUTION FOUND] W: 1280 H: 720");
            }
            tempCounter++;
        }
        Debug.Log(" [DETECTED RESOLUTION] W: " + Screen.width + " H: " + Screen.height);
        if(Screen.width == 1280 && Screen.height == 720)
        {
            currentResolutionID = tempRes;
            resolutionText.text = resolutions[currentResolutionID].Item1 + "x" + resolutions[currentResolutionID].Item2;
        }
        else if(defaultResolutionFound)
        {//and for some reason screen res is not the default
            currentResolutionID = tempRes;
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            resolutionText.text = resolutions[currentResolutionID].Item1 + "x" + resolutions[currentResolutionID].Item2;
            Debug.Log(" [SET TO DEFAULT] W: 1280 H: 720");
        }
        else
        {//for some reason 720p not supported
            if(Screen.currentResolution.width >= 640 && Screen.currentResolution.height >= 360)
            {//fallback to some smaller resolution
                currentResolutionID = 0;
                Screen.SetResolution(640, 360, FullScreenMode.Windowed);
                resolutionText.text = resolutions[currentResolutionID].Item1 + "x" + resolutions[currentResolutionID].Item2;
                Debug.Log(" [SET TO MINIMUM] W: " + Screen.width + " H: " + Screen.height);
            }
            else
            {
                Debug.Log("Monitor not found (joke)");
            }
        }
        currentScreenMode = FullScreenMode.Windowed;


        audioController = GameObject.Find("Audio").GetComponent<AudioLibrary>();
        //setMusicVolume(0.5f);
        //setAmbientVolume(0.666f);
        //setSFXVolume(0.666f);
    }


    public void showHideEndPrompt()
    {
        if (!endPromptVisible)
        {
            endPromptVisible = true;
            endPrompt.GetComponent<Animator>().Play("EndPromptShow");
        }
        else
        {

            endPromptVisible = false;
            endPrompt.GetComponent<Animator>().Play("EndPromptHide");
        }
    }

    public void extract()
    {
        if (bipReference.normal)
        {
            GUIUtility.systemCopyBuffer = bipReference.getMnemonic();
        }
        else
        { //same..
            GUIUtility.systemCopyBuffer = bipReference.getMnemonic();
        }
        //copy to clipboard
        extractButton.transform.Find("ClipboardText").GetComponent<Text>().text = "Copied to Clipboard";
    }

    public void inventoryFilled(Inventory.InventoryItem[] slots)
    {
        if (bipReference.normal)
        {
            string mnemonic = "";
            BitcoinController bCont = GameObject.Find("ScriptLoader").GetComponent<Globals>().bitcoinController;
            int counter = 0;
            foreach (Inventory.InventoryItem slot in slots)
            {
                string wordReturned = bCont.findWordByBitcoin(slot.parentID, slot.star);
                if (wordReturned != null)
                {
                    mnemonic += " " + wordReturned;
                }
                else
                {
                    mnemonic += " " + slot.itemName;
                    Debug.Log(slot.itemName + " was null when checking for end game");
                }
                counter++;
            }
            bipReference.setMnemonic(mnemonic);
            //open prompt to end game
            endPromptVisible = false;
            endPrompt.SetActive(true);
        }
        else if(bipReference.demo || bipReference.practice)
        {
            string[] mnemonic = new string[slots.Length];
            BitcoinController bCont = GameObject.Find("ScriptLoader").GetComponent<Globals>().bitcoinController;
            int counter = 0;
            foreach (Inventory.InventoryItem slot in slots)
            {
                string wordReturned = bCont.findWordByBitcoin(slot.parentID, slot.star);
                if (wordReturned != null)
                {
                    mnemonic[counter] = wordReturned;
                }
                else
                {
                    mnemonic[counter] = slot.itemName;
                    Debug.Log(slot.itemName + " was null when checking for end game");
                }
                counter++;
            }
            if (bipReference.checkMnemonic(mnemonic))
            {
                //win
                endPromptVisible = false;
                endPrompt.SetActive(true);
            }
            else
            {
                extractButton.transform.Find("ClipboardText").GetComponent<Text>().text = "Extract";
                endPromptVisible = false;
                endPrompt.SetActive(false);
            }
        }
    }

    public void inventoryNotFilled()
    {
        StartCoroutine(closePrompt(1f));
    }

    IEnumerator closePrompt(float delayTime)
    {
        extractButton.transform.Find("ClipboardText").GetComponent<Text>().text = "Extract";
        if(endPromptVisible)
            endPrompt.GetComponent<Animator>().Play("EndPromptHide");
        endPromptVisible = false;
        yield return new WaitForSeconds(delayTime);
        endPrompt.SetActive(false);
    }


    public void startPractice()
    {
        introScreen.gameObject.SetActive(false);
        //wordLengthScreen.gameObject.SetActive(true);
        listScreen.gameObject.SetActive(true);
    }

    public void startPractice(BoatIntro biRef)
    {
        this.biRef = biRef;
        introScreen.gameObject.SetActive(false);
        //wordLengthScreen.gameObject.SetActive(true);
        listScreen.gameObject.SetActive(true);
    }

    public void start12Word()
    {
        //pass 12 word argument to mnemonic
        wordLengthScreen.gameObject.SetActive(false);
        //UIPanel.gameObject.SetActive(false);
        listScreen.gameObject.SetActive(true);
    }

    public void startNormal()
    {
        //CinemachineFader cvFader = GameObject.Find("ScriptLoader").GetComponent<CinemachineFader>();
        //start fade animation
        bipReference.normalGameMode();
        //cvFader.fadeCamera();
        //StartCoroutine(closeAll(1.2f));

    }

    public void startGamePracticeMode(string[] mnemonic)
    {
        //Do stuff with mnemonic.
        //CinemachineFader cvFader = GameObject.Find("ScriptLoader").GetComponent<CinemachineFader>();
        //start fade animation
        bipReference.practiceGameMode(mnemonic);
        listScreen.gameObject.SetActive(false);
        biRef.continueCutsceneFromPractice();
        //cvFader.fadeCamera();
        //StartCoroutine(closeAll2(1.2f));

    }

    public void startDemo()
    {
        //currentgamemode load 11 out of 12 words.
        //do something more here
        //CinemachineFader cvFader = GameObject.Find("ScriptLoader").GetComponent<CinemachineFader>();
        //start fade animation
        bipReference.demoGameMode();
        //cvFader.fadeCamera();
        //StartCoroutine(closeAll(1.2f));

    }

    public void pickLight()
    {
        GameObject.Find("ScriptLoader").GetComponent<Globals>().bitcoinController.pickBitcoin(true);
        returnButton();
    }

    public void pickDark()
    {
        GameObject.Find("ScriptLoader").GetComponent<Globals>().bitcoinController.pickBitcoin(false);
        returnButton();
    }

    public void returnButton()
    {
        itemSelection.gameObject.SetActive(false);
        //UIPanel.gameObject.SetActive(false);
    }

    public void returnSettingsButton()
    {
        settingsPanel.gameObject.SetActive(false);
        //UIPanel.gameObject.SetActive(false);
    }

    public void changeResLeft()
    {
        if (currentResolutionID == 0)
        {
            currentResolutionID = resolutions.Count - 1;
        }
        else
        {
            currentResolutionID--;
        }
        Screen.SetResolution(resolutions[currentResolutionID].Item1, resolutions[currentResolutionID].Item2, currentScreenMode);
        resolutionText.text = resolutions[currentResolutionID].Item1 + "x" + resolutions[currentResolutionID].Item2;

    }

    public void changeResRight()
    {
        if (currentResolutionID == resolutions.Count - 1)
        {
            currentResolutionID = 0;
        }
        else
        {
            currentResolutionID++;
        }
        Screen.SetResolution(resolutions[currentResolutionID].Item1, resolutions[currentResolutionID].Item2, currentScreenMode);
        resolutionText.text = resolutions[currentResolutionID].Item1 + "x" + resolutions[currentResolutionID].Item2;
    }

    public void changeScreenMode(int id)
    {
        if(id == 0)
        {
            //fullscreen
            if (currentScreenMode != FullScreenMode.FullScreenWindow)
            {
                windowedBoolean.SetActive(false);
                fullscreenBoolean.SetActive(true);
                currentScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(resolutions[currentResolutionID].Item1, resolutions[currentResolutionID].Item2, currentScreenMode);
            }
        }
        else
        {
            //windowed
            if (currentScreenMode != FullScreenMode.Windowed)
            {

                windowedBoolean.SetActive(true);
                fullscreenBoolean.SetActive(false);
                currentScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(resolutions[currentResolutionID].Item1, resolutions[currentResolutionID].Item2, currentScreenMode);
            }
        }
    }

    public void setMusicVolume(float value)
    {
        //normalize slider to (maximum current volume)
        float max = 0.3f;
        float min = 0f;
        float newVolume = value * max;
        audioController.changeVolume(0, newVolume);
    }

    public void setAmbientVolume(float value)
    {
        //normalize slider to (maximum current volume)
        float max = 0.15f;
        float min = 0f;
        float newVolume = value * max;
        audioController.changeVolume(1, newVolume);
    }

    public void setSFXVolume(float value)
    {
        //normalize slider to (maximum current volume)
        float max = 0.6f;
        float min = 0f;
        float newVolume = value * max;
        audioController.changeVolume(2, newVolume);
    }

    public void exitGame()
    {
        //exit game
        Application.Quit();
    }

    public void openSelectionWindow()
    {
        //UIPanel.gameObject.SetActive(true);
        itemSelection.gameObject.SetActive(true);
    }

    public void openSettingsWindow()
    {
        //UIPanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(true);
    }

}
