using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite[] slotImages;
    private ChestSlot[] slots;

    public GameObject container24;
    public GameObject container;
    private Globals script;
    public Text inventoryText;
    int layout = 12;
    public Sprite sprite1;
    public Sprite sprite2;
    // Start is called before the first frame update

    public SpriteController spriteController;
    public bool isOverInventory;

    public class ChestSlot
    {
        public Bitcoin item;
        public Sprite slotImage;
    }

    void Start()
    {
        slots = new ChestSlot[12]; //arbitrary
        int counter = 0;
        foreach(Sprite s in slotImages)
        {
            slots[counter] = new ChestSlot();
            slots[counter].slotImage = s;
            slots[counter].item = null;
            counter++;
        }
        GameObject parent = gameObject;
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        //container = parent.transform.Find("24Word").gameObject;
        //container = this.gameObject.transform.Find(layout+"Word").gameObject;
        inventoryText = container24.transform.Find("ChestItemDisplay").transform.Find("Text").GetComponent<Text>();
        container = container24;
        isOverInventory = false;
        //destroyed from UIDestroy - Inventory GO. - nope
        this.GetComponent<RectTransform>().gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void FakeUpdate()
    {

    }

    public void setInventoryToChestItems(List<Bitcoin> content)
    {
        //spriteController.disableAllMove = true;
        int counter = 0;
        for(int i=0; i<content.Count; i++)
        {
            slots[i].item = content[i];
            counter = i;
            setItemInSlot(i, content[i]);
            if(counter == 11)
            {
                Debug.Log("sdsad");
            }
        }
        for(int j = counter + 1; j<slots.Length; j++)
        {
            //clear remaining slots if have to.
            slots[j].item = null;
            setItemInSlot(j,null);
        }
    }

    public void clearInventory()
    {
        //spriteController.disableAllMove = false;
        for (int j = 0; j < slots.Length; j++)
        {
            //clear remaining slots if have to.
            slots[j].item = null;
            setItemInSlot(j, null);
        }
        script.bitcoinController.setActiveBitcoin(-1);
        script.mainMenuScript.returnButton();
        Debug.Log("Chest contents cleared");
    }

    private void setItemInSlot(int slotID, Bitcoin item)
    {
        Transform tr = container.transform.Find("LeftSlots").transform.Find("Button (" + slotID + ")");
        GameObject buttonSlot = null;
        if (tr == null)
        {
            tr = container.transform.Find("RightSlots").transform.Find("Button (" + slotID + ")");
        }
        if (tr != null)
        {
            buttonSlot = tr.gameObject;
        }
        else
        {
            Debug.Log("Slot id " + slotID + " in chest not found ");
        }
        if (buttonSlot != null)
        {
            if (item != null)
            {
                buttonSlot.GetComponent<ChestHandler>().setImage(slots[slotID].slotImage);
            }
            else
            {
                buttonSlot.GetComponent<ChestHandler>().setImage(script.transparent);
            }
        }
    }

    public void selectChestItem(int slotID)
    {
        if (slots[slotID].item != null)
        {
            script.bitcoinController.setActiveBitcoin(slots[slotID].item.BitcoinID);
            script.mainMenuScript.openSelectionWindow();
            //and open inventory for consistency
            if (!script.inventory.gameObject.activeSelf)
            {
                script.inventory.GetComponent<RectTransform>().gameObject.SetActive(true);
                script.inventory.GetComponent<Animator>().Play("InventoryShow");
            }
            openItemPanel(slots[slotID].item.ItemWord);
        }
    }

    private void openItemPanel(string item)
    {
        script.itemPanel.SetActive(true);
        script.itemPanel.GetComponent<Animator>().Play("ItemPanelMove");
        script.itemDisplay.text = item;
    }

    public void onHover(int slotID)
    {
        if (slots[slotID].item != null)
        {
            inventoryText.text = slots[slotID].item.ItemWord;
        }
        else
        {
            inventoryText.text = "Empty";
        }
    }

    public void onHoverExit(int slotID)
    {
        inventoryText.text = "Empty";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if the player is holding the mouse button to move
        if (!spriteController.moving)
            spriteController.disableMoving = false;
        isOverInventory = false;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!spriteController.moving)
            spriteController.disableMoving = true;
        isOverInventory = true;
    }
}
