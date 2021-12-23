using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DetectObject : MonoBehaviour
{
    public Texture2D crosshair;
    public Texture2D crosshairActive;
    public GameObject UI;
    private bool isActive;
    private bool isInRange;
    private Globals script;
    private Animator animator;

    void Start()
    {
       
        isActive = false;
        isInRange = false;
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        animator = UI.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInRange &&
            Input.GetKeyDown("e"))
        {
            script.bitcoinController.setActiveBitcoin(GetComponent<ItemEdit>().id);
            script.mainMenuScript.openSelectionWindow();
            //and open inventory for consistency
            script.inventory.GetComponent<RectTransform>().gameObject.SetActive(true);
            script.inventory.GetComponent<Animator>().Play("InventoryShow");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        script.itemPanel.SetActive(true);
        animator.SetBool("IsNear", true);
        script.itemDisplay.text = gameObject.GetComponent<ItemEdit>().itemName;
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        script.itemPanel.SetActive(false);
        animator.SetBool("IsNear", false);
        script.itemDisplay.text = "";
        isInRange = false;
    }

    void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; //prevent ui clicks
        if (!isActive)
        {
            script.itemPanel.SetActive(true);
            Cursor.SetCursor(crosshairActive, Vector2.zero, CursorMode.Auto);
            isActive = true;
            script.itemDisplay.text = gameObject.GetComponent<ItemEdit>().itemName;

        }
    }
     
    void OnMouseExit()
    {
        script.itemDisplay.text = "";
        script.itemPanel.SetActive(false);
        isActive = false;
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);
    }
}
