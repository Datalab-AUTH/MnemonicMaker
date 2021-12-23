using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class BoatIntro : MonoBehaviour
{

    public Animator animator;
    public GameObject blessingUI;
    public GameObject blessingUIskip;
    private bool cutscenePlaying;
    private bool blockBackwards;
    private bool blessingTriggered;
    private int counter;
    public GameObject[] indicators;
    public GameObject[] indicatorsSKIP;
    private bool m_isAxisInUse;
    // Update is called once per frame

    public GameObject[] dialogueNormal;
    public GameObject[] dialoguePractice;
    public GameObject[] dialogueDemo;
    public GameObject infoPanel;
    public bool forwardBlock;
    public GameObject flyingCat;

    private bool skipEverything;
    private bool customCallback;
    public GameObject UIPanel;
    public GameObject blackbar_down;
    public GameObject blackbar_up;
    public bool blessingBeforeSkip;

    public GameObject[] toEnableAfterIntro;

    private void Awake()
    {
        blessingBeforeSkip = false;
        blessingUIskip.SetActive(false);
        cutscenePlaying = false;
        blockBackwards = false;
        blessingTriggered = false;
        m_isAxisInUse = false;
        forwardBlock = false;
        skipEverything = false;
        customCallback = false;
        counter = 0;
        indicators[0].SetActive(true);
        indicators[1].SetActive(false);
        indicators[2].SetActive(false);
        infoPanel.SetActive(false);
    }

    public void skipIntro()
    {
        flyingCat.gameObject.SetActive(false);
        skipEverything = true;
        UIPanel.SetActive(false);
        animator.SetFloat("AnimationSpeed", 7);
    }

    public void enableInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void disableInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    void Update()
    {
        if (skipEverything) return;
        if (!cutscenePlaying)
        {
            animator.SetFloat("AnimationSpeed", 0);
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                //move backwards
                if(!blockBackwards)
                    animator.SetFloat("AnimationSpeed", -0.5f);
            }
            else if ((Input.GetAxisRaw("Horizontal") == -1) && !forwardBlock)
            {
                animator.SetFloat("AnimationSpeed", 1);
            }
        }
        else
        {
            if (blessingTriggered)
            {
                if (Input.GetAxisRaw("Horizontal") == 1)
                {
                    //to the right
                    if (counter < 2 && m_isAxisInUse == false)
                    {
                        m_isAxisInUse = true;
                        counter++;
                        selectingBlessing(counter-1);
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") == -1)
                {
                    if (counter > 0 && m_isAxisInUse == false)
                    {
                        m_isAxisInUse = true;
                        counter--;
                        selectingBlessing(counter+1);
                    }
                }
                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    m_isAxisInUse = false;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    blessing(counter);
                }
            }
        }
        
    }

    public void cutscene()
    {
        if (skipEverything) return;
        cutscenePlaying = true;
        animator.SetFloat("AnimationSpeed", 1);
    }

    public void chooseBlessing()
    {
        if (skipEverything) return;
        animator.SetFloat("AnimationSpeed", 0);
        //ui for blessing pop up
        blessingTriggered = true;
    }

    private void selectingBlessing(int oldBlessing)
    {
        if (skipEverything) return;
        indicators[oldBlessing].SetActive(false);
        indicators[counter].SetActive(true);
    }

    private int blessingSelected;

    public void blessing(int id)
    {
        if (skipEverything) return;
        blessingBeforeSkip = true;
        blessingSelected = id;
        //id = 0 -> normal mode practice demo .
        MainMenu script = GameObject.Find("ScriptLoader").GetComponent<Globals>().mainMenuScript;
        if (id == 0)
        {
            script.startNormal();
            foreach(GameObject go in dialoguePractice)
            {
                foreach(Transform child in go.transform)
                {
                    child.GetComponent<Text>().enabled = false;
                }
            }
            foreach (GameObject go in dialogueDemo)
            {
                foreach (Transform child in go.transform)
                {
                    child.GetComponent<Text>().enabled = false;
                }
            }
            if (customCallback)
            {
                afterSkipBlessing();
            }
        }
        else if(id == 1)
        {
            foreach (GameObject go in dialogueDemo)
            {
                foreach (Transform child in go.transform)
                {
                    child.GetComponent<Text>().enabled = false;
                }
            }
            foreach (GameObject go in dialogueNormal)
            {
                foreach (Transform child in go.transform)
                {
                    child.GetComponent<Text>().enabled = false;
                }
            }
        }
        else if(id == 2)
        {
            
            script.startDemo();
            foreach (GameObject go in dialoguePractice)
            {
                foreach (Transform child in go.transform)
                {
                    child.GetComponent<Text>().enabled = false;
                }
            }
            foreach (GameObject go in dialogueNormal)
            {
                foreach (Transform child in go.transform)
                {
                    child.GetComponent<Text>().enabled = false;
                }
            }
            if (customCallback)
            {
                afterSkipBlessing();
            }
        }
        //cutscenePlaying = false; //this will enable the update again
        if(!customCallback)
            blessingTriggered = false;
        continueCutscene();
    }

    public void openWordlistWindow()
    {
        if (skipEverything) return;
        if (blessingSelected == 1 && blessingTriggered)
        {
            animator.SetFloat("AnimationSpeed", 0);
            blessingTriggered = false;
            if (!customCallback)
            {

                blessingUI.SetActive(false);
            }
            else
            {

                blessingUIskip.SetActive(false);
            }

            MainMenu script = GameObject.Find("ScriptLoader").GetComponent<Globals>().mainMenuScript;
            script.startPractice(transform.GetComponent<BoatIntro>());
        }
    }

    public void continueCutsceneFromPractice()
    {
        if (skipEverything) return;
        if (!customCallback)
        {
            blessingUI.SetActive(true);
        }
        else
        {
            afterSkipBlessing();
        }
        animator.SetFloat("AnimationSpeed", 1);
    }

    private void continueCutscene()
    {
        if (skipEverything) return;
        animator.SetFloat("AnimationSpeed", 1);
    }

    public void stopCutscene()
    {
        if (skipEverything) return;
        cutscenePlaying = false; //this will enable the update again
        blockBackwards = true;
    }

    public void enableBackwards()
    {
        if (skipEverything) return;
        blockBackwards = false;
    }

    public void normalModeSkip()
    {
        if (skipEverything) return;
        if (blessingSelected == 0)
        {
            Debug.Log("normal skip");
            infoPanel.SetActive(false);
        }
    }

    public void undoNormalModeSkip()
    {
        if (skipEverything) return;
        if (blessingSelected == 0)
        {
            Debug.Log("normal skip undo");
            infoPanel.SetActive(true);
        }
    }

    public void blockForward()
    {
        UIPanel.SetActive(true);
        forwardBlock = true;
        animator.SetFloat("AnimationSpeed", 0);
    }

    public void enableForward()
    {
        forwardBlock = false;
    }

    public Animator offsetCameraFix;
    public void disembark()
    {
        cutscenePlaying = true;
        //do some camera and other stuff
        animator.SetFloat("AnimationSpeed", 1);
        offsetCameraFix.Play("CameraOffsetFix");
        
    }

    public CinemachineVirtualCamera vcam;
    public CinemachineVirtualCamera boatCam;

    public void swapCharacters()
    {
        if (skipEverything && !blessingBeforeSkip)
        {
            //GameObject.Find("Character").GetComponent<SpriteController>().disableAllMove = true;
            skipEverything = false;

            chooseBlessing();

            blessingUIskip.SetActive(true);

            counter = 0;
            indicators = indicatorsSKIP;
            indicators[0].SetActive(true);
            indicators[1].SetActive(false);
            indicators[2].SetActive(false);
            customCallback = true;
        }
        else
        {
            GameObject charOnBoat = GameObject.Find("CharacterOnBoat");
            Vector3 position = charOnBoat.transform.position;
            GameObject character = GameObject.Find("Character");
            character.transform.SetParent(null);
            character.transform.position = position;
            character.transform.Find("Night-Light").gameObject.SetActive(true);
            charOnBoat.SetActive(false);
            character.GetComponent<SpriteRenderer>().enabled = true;
            character.GetComponent<SpriteController>().enabled = true;
            character.transform.Find("character shadow").gameObject.SetActive(true);
            boatCam.Priority = 1;
            vcam.Priority = 99;
            
            foreach(GameObject go in toEnableAfterIntro)
            {
                go.SetActive(true);
            }
            GameObject.Find("GlobalEventHandling").GetComponent<UIControls>().lockControls = false;
            
        }
        
    }

    public void afterSkipBlessing()
    {
        blessingUIskip.SetActive(false);

        GameObject charOnBoat = GameObject.Find("CharacterOnBoat");
        Vector3 position = charOnBoat.transform.position;
        GameObject character = GameObject.Find("Character");
        character.transform.SetParent(null);
        character.transform.position = position;
        character.transform.Find("Night-Light").gameObject.SetActive(true);
        charOnBoat.SetActive(false);
        character.GetComponent<SpriteRenderer>().enabled = true;
        character.GetComponent<SpriteController>().enabled = true;
        character.transform.Find("character shadow").gameObject.SetActive(true);
        boatCam.Priority = 1;
        vcam.Priority = 99;
        
        foreach (GameObject go in toEnableAfterIntro)
        {
            go.SetActive(true);
        }
        GameObject.Find("GlobalEventHandling").GetComponent<UIControls>().lockControls = false;
        
    }

}
