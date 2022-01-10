using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chest : MonoBehaviour
{
    private List<Bitcoin> chestContent;
    private int chestSlots = 12;
    private Globals script;
    private Animator animator;
    public Animator UI;

    private bool isInRange;
    private bool chestIsOpen;
    //Distibution variables
    public string World;
    public string Dungeon;
    public int Chunk;
    public (double, double) Coordinates;
    /*
     * Data methods
     */
    public List<Bitcoin> getContent()
    {
        return chestContent;
    }

    public void setContent(Bitcoin item)
    {
        chestContent.Add(item);
    }


    /*
     * UI Methods
     */
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        animator = GetComponent<Animator>();
        chestContent = new List<Bitcoin>();
        isInRange = false;
        chestIsOpen = false;
    }

    private void Update()
    {
        if (isInRange &&
            Input.GetKeyDown("e"))
        {
            chestIsOpen = true;

            //fade out the UI
            UI.SetBool("IsNear", false);
            isInRange = false; // to prevent clicking E again before walking in and out of the chest area
            //open the chest animation
            animator.SetBool("ChestNear", true);

            //open chest inventory 
            script.chestInventory.GetComponent<RectTransform>().gameObject.SetActive(true);
            //data stuff.. <<<< do here
            script.chestInventory.setInventoryToChestItems(chestContent);
            //and open inventory for consistency
            script.inventory.GetComponent<RectTransform>().gameObject.SetActive(true);
            script.inventory.GetComponent<Animator>().Play("InventoryShow");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        UI.SetBool("IsNear", true);
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Show the ui
        UI.SetBool("IsNear", false);
        isInRange = false;

        //Open the chest
        if (chestIsOpen)
        {
            script.chestInventory.clearInventory();

            animator.SetBool("ChestNear", false);
            script.chestInventory.GetComponent<RectTransform>().gameObject.SetActive(false);

            script.inventory.GetComponent<Animator>().Play("InventoryHide");
        }
        chestIsOpen = false;
    }

}
