using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDestroy : MonoBehaviour
{
    public GameObject inventory;
    public GameObject chestInventory;
    public GameObject itemPanel;

    public void disableUI()
    {
        inventory.SetActive(false);
        chestInventory.SetActive(false);
    }

    public void disableItemPanel()
    {
        itemPanel.transform.Find("ItemDisplay").GetComponent<Text>().text = "";
        itemPanel.SetActive(false);
    }


}
