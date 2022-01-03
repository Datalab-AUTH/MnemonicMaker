using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EffectManager : MonoBehaviour
{
    private SpriteController character;
    public GameObject night;
    public GameObject dungeon;
    public GameObject day;
    public GameObject fader;
    private BIP39Game game;
    public GameObject characterLight;
    public GameObject characterLightMoon;
    public GameObject clouds;
    private BitcoinController bController;
    private List<GameObject> lights;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("ScriptLoader").GetComponent<Globals>().spriteController;
        game = GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39;
        bController = GameObject.Find("ScriptLoader").GetComponent<Globals>().bitcoinController;
        lights = new List<GameObject>(GameObject.FindGameObjectsWithTag("Light"));
    }
    
    public void lightControl()
    {
        int nodeID = character.nodeID;
        //find all lights everytime a new chunk is loaded
        if (nodeID == 0 || nodeID == 1 || (nodeID >= 7 && nodeID <= 17) ||
            nodeID == 29 || nodeID == 30 || (nodeID >= 36 && nodeID <= 46))
        {
            Debug.Log("Controlling all lights in loaded chunks");
            lights = new List<GameObject>(GameObject.FindGameObjectsWithTag("Light"));
            if (game.day)
            {
                foreach (GameObject light in lights)
                {
                    light.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject light in lights)
                {
                    light.SetActive(true);
                }
            }
        }
    }

    public void changeEffects()
    {
        int nodeID = character.nodeID;
        /*
        if (game.day)
        {
            //characterLight.SetActive(false);
            bController.switchLights(!game.day);
        }
        else
        {
            //characterLight.SetActive(true);
            bController.switchLights(!game.day);
        }*/
        if (nodeID == 0 || nodeID == 29)
        {
            clouds.transform.Find("Cloud").gameObject.SetActive(true);
            if (game.day)
            {
                characterLight.SetActive(false);
                characterLightMoon.SetActive(false);
            }
            else
            {
                characterLight.SetActive(false);
                characterLightMoon.SetActive(true);
            }
        }
        else
        {
            clouds.transform.Find("Cloud").gameObject.SetActive(false);
            if (game.day)
            {
                characterLight.SetActive(false);
                characterLightMoon.SetActive(false);
            }
            else
            {
                characterLight.SetActive(true);
                characterLightMoon.SetActive(false);
            }
        }
        if (nodeID == 0 || nodeID == 1 || (nodeID>=7 && nodeID<=17) ||
            nodeID == 29 || nodeID == 30 || (nodeID >= 36 && nodeID <= 46))
        {
            dungeon.SetActive(false);
            if (game.day)
            {
                night.SetActive(false);
                day.SetActive(true);
            }
            else
            {
                night.SetActive(true);
                day.SetActive(false);
            }
            //bController.switchLights(!game.day);
            fader.SetActive(false);
        }
        else if(nodeID == 2 || nodeID == 3 || nodeID == 4 || nodeID == 5 || nodeID == 6 || nodeID == 18 || nodeID == 19 || nodeID == 20 ||
                nodeID == 31 || nodeID == 32 || nodeID == 33 || nodeID == 34 || nodeID == 35 || nodeID == 47 || nodeID == 48 || nodeID == 49)
        {
            night.SetActive(false);
            day.SetActive(false);
            dungeon.SetActive(true);
            fader.SetActive(true);
            characterLight.SetActive(true);
            //bController.switchLights(true);
        }
        else if((nodeID >= 21 && nodeID<=28 )||
                (nodeID >= 50 && nodeID <= 57))
        {
            night.SetActive(false);
            day.SetActive(false);
            dungeon.SetActive(true);
            fader.SetActive(false);
            characterLight.SetActive(true);
            //bController.switchLights(true);
        }
        else
        { //generic catch all
            if (game.day)
            {
                night.SetActive(false);
                day.SetActive(true);
                dungeon.SetActive(false);
            }
            else
            {
                night.SetActive(true);
                day.SetActive(false);
                dungeon.SetActive(false);
            }
            fader.SetActive(false);
        }
        
    }

}
