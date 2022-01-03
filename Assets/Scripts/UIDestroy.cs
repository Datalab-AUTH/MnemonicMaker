using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDestroy : MonoBehaviour
{
    public GameObject inventory;
    public GameObject chestInventory;
    public GameObject minimap;
    public Camera MinimapCamera;

    public void disableUI()
    {
        inventory.SetActive(false);
        chestInventory.SetActive(false);
    }

    public void disableMinimapUI()
    {
        minimap.SetActive(false);
        MinimapCamera.transform.gameObject.SetActive(false);
    }

}
