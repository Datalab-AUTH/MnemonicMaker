﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    public Inventory inventory;
    private Globals script;
    private Sprite slotImage;
    private RawImage slotImageReference;
    private RawImage dragDummy;
    private bool isDragged;

    void Start()
    {
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        slotImageReference = transform.Find("RawImage").GetComponent<RawImage>();
        dragDummy = script.dummyDrag.GetComponent<RawImage>();
        setImage(script.transparent);
        isDragged = false;
    }

    private void FixedUpdate()
    {
        if (isDragged)
        {
            dragDummy.transform.position = Input.mousePosition
;        }
    }

    public void setImage(Sprite sprite)
    {
        slotImage = sprite;
        slotImageReference.texture = slotImage.texture;
    }

    public void onDrag(int buttonID)
    {
        if (!isDragged)
        {
            isDragged = true;
            inventory.dragged(buttonID);
        }
        script.spriteController.disableMoving = true;
        //transform.Find("RawImage").position = Input.mousePosition;
        //Set this slot's image to the dummy
        dragDummy.texture = slotImage.texture;
        //Replace the image of the slot with transparent
        slotImageReference.texture = script.transparent.texture;
        dragDummy.transform.position = Input.mousePosition;

    }

    public void OnDrop(int buttonID)
    {
        inventory.dropped(buttonID);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragged = false;
        //if(!inventory.isOverInventory)
        script.spriteController.disableMoving = false;
        //reset dummy
        dragDummy.transform.localPosition = Vector2.zero;
        //reset drag dummy to transparent
        dragDummy.texture = script.transparent.texture;
        //for now
        //if (inventory.draggedID == -99) //dropped on the same slot
        //slotImageReference.texture = slotImage.texture;
        inventory.removed();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!script.spriteController.moving)
        {
            inventory.hover(eventData.pointerEnter.name);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!script.spriteController.moving)
        {
            inventory.hover("Empty");
        }
    }

}
