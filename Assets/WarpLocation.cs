using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpLocation : MonoBehaviour
{
    private GameObject character;
    private GameObject lights;
    private GameObject night;
    private GameObject day;
    private GameObject characterLight;
    public Transform doormat;
    private Portal Portal;
    private int GateID;
    public int layerOveride;

    public int gateID { get => GateID; set => GateID = value; }
    public Portal portal { get => Portal; set => Portal = value; }

    private void Start()
    {
        GateID = -1;
        character = GameObject.Find("Character");
        lights = GameObject.Find("Lights-Time");
        night = lights.gameObject.transform.Find("Night").gameObject;
        day = lights.gameObject.transform.Find("Day").gameObject;
        characterLight = character.gameObject.transform.Find("Night-Light").gameObject;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        GameObject.Find("Character").GetComponent<SpriteController>().changeFloor(layerOveride);
        Portal.warp(GateID);
    }

}
