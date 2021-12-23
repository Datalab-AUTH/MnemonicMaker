using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject container24;
    public GameObject container;
    public Text inventoryText;
    int layout = 12;
    public InventoryItem[] slots;
    public Sprite sprite1;//variant of true
    public Sprite sprite2;//variant of false
    public Sprite defaultSprite;
    public GameObject dragDummy;
    // Start is called before the first frame update


    public SpriteController spriteController;
    public bool isOverInventory;
    void Start()
    {
        GameObject parent = gameObject;
        //container = parent.transform.Find("24Word").gameObject;
        //container = this.gameObject.transform.Find(layout+"Word").gameObject;
        //inventoryText = container24.transform.Find("InventoryItemDisplay").transform.Find("Text").GetComponent<Text>();
        container = container24;
        slots = new InventoryItem[layout];
        isOverInventory = false;
    }

    private void checkEndCondition()
    {
        //not sure on if heavy
        foreach(var item in slots)
        {
            if (item == null)
            {
                GameObject.Find("ScriptLoader").GetComponent<Globals>().mainMenuScript.inventoryNotFilled();
                return;
            }
        }
        GameObject.Find("ScriptLoader").GetComponent<Globals>().mainMenuScript.inventoryFilled(slots);
    }

    public class InventoryItem
    {
        public int parentID;
        public bool star; //variant(light-dark coin)
        public string itemName;//for .. display purposes
    }

    private void setItemInSlot(int slotID, InventoryItem item)
    {
        Transform tr = container.transform.Find("LeftSlots").transform.Find("Button (" + slotID + ")");
        GameObject buttonSlot = null;
        if (tr == null)
        {
            tr = container.transform.Find("RightSlots").transform.Find("Button (" + slotID + ")");
        }
        if( tr != null)
        {
            buttonSlot = tr.gameObject;
        }
        if(buttonSlot != null && item!=null)
        {
            //set the image placeholder
            if (item.star)
            {
                buttonSlot.GetComponent<InventoryHandler>().setImage(sprite1);
            }
            else
            {
                buttonSlot.GetComponent<InventoryHandler>().setImage(sprite2);
            }
            //slots[slotID] = item;
        }
        else if(item==null)
        {
            buttonSlot.GetComponent<InventoryHandler>().setImage(defaultSprite);
        }

        checkEndCondition();
    }

    public bool itemCollected(InventoryItem item)
    {
        if (item != null)
        {
            for(int i=0; i<layout; i++)
            {
                if(slots[i] == null)
                {
                    slots[i] = item; //data
                    //FakeUpdate(); // need better implementation
                    setItemInSlot(i, item); //visual
                    if(item.star)
                        Debug.Log("Item picked up : Light " + item.itemName);
                    else
                        Debug.Log("Item picked up : Dark " + item.itemName);
                    return true;
                }
            }
            return false;
        }
        else
        {
            Debug.Log("You hit some air");
            return false;
        }
    }

    public void hover(string data)
    {
        string slotID = "";
        if(data.Trim() == "Empty")
        {
            inventoryText.text = "Empty";
            //Debug.LogWarning("hover : " + data);
        }
        else
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (System.Char.IsNumber(data[i]))
                {
                    slotID += data[i];
                }
            }
            //Debug.LogWarning("hover else : " + data);
            int slotIDhovered = int.Parse(slotID);
            if (slots[slotIDhovered] != null)
            {
                inventoryText.text = slots[slotIDhovered].itemName;
            }
            else
            {
                inventoryText.text = "Empty";
            }
        }
        
    }

    public int draggedID = -1;
    public int droppedID = -1;

    public void dragged(int buttonID)
    {
        if (slots[buttonID] != null)
        {
            draggedID = buttonID;
        }
        else
        {
            draggedID = -1;
        }
    }
    
    public void dropped(int buttonID)
    {
        Debug.LogWarning("hover dropped : " + buttonID);
        string slotID = "";
        if(draggedID != -1) // fix for dragging empty slots.
        {
            int slotIDhovered = buttonID;
            droppedID = buttonID;
            //dropped on the same slot 
            if (draggedID == slotIDhovered)
            {
                //draggedID = -99;
                InventoryItem itemThatWasDragged = slots[draggedID];

                //re-set image
                setItemInSlot(draggedID, itemThatWasDragged);

                //data was not hurt.

                Debug.Log("dropped in same slot");
            }
            else
            {
                //dropped in a place that is occupied
                if (slots[slotIDhovered] != null)
                {
                    //dropped in an empty slot - swap
                    InventoryItem itemInSlotThatDropped = slots[slotIDhovered];
                    InventoryItem itemThatWasDragged = slots[draggedID];

                    //FakeUpdate(); // need better implementation

                    //swap data
                    slots[slotIDhovered] = itemThatWasDragged;
                    slots[draggedID] = itemInSlotThatDropped;

                    //swap images
                    setItemInSlot(slotIDhovered, itemThatWasDragged); //dropped here 
                    setItemInSlot(draggedID, itemInSlotThatDropped);


                    Debug.Log("dropped in occupied slot");
                }
                else
                {
                    InventoryItem itemThatWasDragged = slots[draggedID];

                    //set data
                    slots[draggedID] = null;
                    slots[slotIDhovered] = itemThatWasDragged;

                    //set image
                    setItemInSlot(slotIDhovered, itemThatWasDragged);
                    setItemInSlot(draggedID, null); //set transparent image

                    Debug.Log("dropped in empty slot");
                }
            }
            draggedID = -1;
        }
    }

    public void removed()
    {
        if (draggedID != -1)
        {
            //draggedID is -1 if the slot was unoccupied or the process of drag/drop was completed from dropped() succesfully
            //then last removed "should" be called and if dropped() wasnt called, dragged should not be -1.
            Debug.Log("Item removed from inventory");
            slots[draggedID] = null;
            setItemInSlot(draggedID, null); //set transparent image//set data
            draggedID = -1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if the player is holding the mouse button to move
        if (!spriteController.moving)
            //spriteController.disableMoving = false;
        isOverInventory = false;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!spriteController.moving)
            //spriteController.disableMoving = true;
        isOverInventory = true;
    }
}
