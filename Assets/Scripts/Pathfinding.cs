using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour
{

    public bool pathfindPanelActive;
    public GameObject pathfindList;
    private int currentPathfindSelected;
    private Animator animator;
    public GameObject pathfindHoverContainer;

    public Text panel;
    public Text panel_target;
    public GameObject character;
    public GameObject up_arrow;
    public GameObject down_arrow;
    public GameObject left_arrow;
    public GameObject right_arrow;
    public int object_to_pathfind = -1;

    private int latest_object;
    private (int, int) target_pos;
    private (int, int) character_pos;

    private List<Bitcoin> itemsToPathfind;
    private Bitcoin lastActivePathfind;
    private GlowController glowController;
    public BFS_Brain bfsBrain;
    private bool pathfindUpdateLock;
    private GameObject pathfindTarget;
    public GameObject[] listItems;
    public Sprite normalCoin;
    public Sprite darkCoin;
    public Sprite completedButton;

    // Start is called before the first frame update
    void Awake()
    {
        //awake is important or we get null
        Debug.Log("Pathfind start ()");
        latest_object = object_to_pathfind;
        pathfindPanelActive = false;
        currentPathfindSelected = -1;
        animator = GetComponent<Animator>();
        itemsToPathfind = new List<Bitcoin>();
        lastActivePathfind = null;
        glowController = GameObject.Find("ScriptLoader").GetComponent<GlowController>();
        pathfindUpdateLock = false;
        pathfindTarget = null;

        up_arrow.SetActive(false);
        down_arrow.SetActive(false);
        left_arrow.SetActive(false);
        right_arrow.SetActive(false);
    }

    public GameObject getTarget()
    {
        return pathfindTarget;
    }

    public void feedPathfinder(List<Bitcoin> bitcoinList)
    {
        listItems = new GameObject[12];
        itemsToPathfind = bitcoinList;
        for (int i = 0; i < 12; i++)
        {
            string suffix = "";
            if (bitcoinList[i].World.Contains("Night"))
            {
                suffix = "Night";
            }
            else
            {
                suffix = "Day";
            }
            GameObject newItem = Resources.Load("Prefabs/UI/PathfindItem"+suffix) as GameObject;
            newItem = Instantiate(newItem);
            listItems[i] = newItem;
            newItem.transform.SetParent(pathfindList.transform, false);
            newItem.GetComponent<SelectPathfindItem>().id = i;

            listItems[i].transform.Find("Active").transform.Find("Text").GetComponent<Text>().text = bitcoinList[i].ItemWord;
            listItems[i].transform.Find("Disabled").transform.Find("Text").GetComponent<Text>().text = bitcoinList[i].ItemWord;
            //GetComponent<Text>().color = new Color(37, 42, 67);
            Sprite image = listItems[i].transform.Find("Active").transform.Find("Image").GetComponent<Image>().sprite;
            Sprite image2 = listItems[i].transform.Find("Disabled").transform.Find("Image").GetComponent<Image>().sprite;
            if (bitcoinList[i].Extras_variation)
            {
                image = normalCoin;
                image2 = normalCoin;
            }
            else
            {
                image = darkCoin;
                image2 = darkCoin;
            }
            listItems[i].transform.Find("Active").transform.Find("Image").GetComponent<Image>().sprite = image;
            listItems[i].transform.Find("Disabled").transform.Find("Image").GetComponent<Image>().sprite = image2;
        }
    }

    public bool crossCheckPathfinder(int index, Inventory.InventoryItem item)
    {
        if(itemsToPathfind[index].BitcoinID == item.parentID &&
           itemsToPathfind[index].Extras_variation == item.star)
        {
            listItems[index].transform.Find("Active").gameObject.SetActive(false);
            listItems[index].transform.Find("Disabled").gameObject.SetActive(true);
            return true;
        }
        listItems[index].transform.Find("Active").gameObject.SetActive(true);
        listItems[index].transform.Find("Disabled").gameObject.SetActive(false);
        return false;
    }

    public void pathfindPopup()
    {
        if (pathfindPanelActive)
        {
            animator.Play("PathfindHide");
            pathfindPanelActive = false;
        }
        else if (!pathfindPanelActive)
        {
            animator.Play("PathfindShow");
            pathfindPanelActive = true;
        }
    }

    public void pathfindSelected(int listID)
    {
        if (currentPathfindSelected != -1)
        {
            listItems[currentPathfindSelected].transform.Find("Active").transform.Find("ActiveBorder").gameObject.SetActive(false);
        }

        if(currentPathfindSelected == listID)
        {
            currentPathfindSelected = -1;
            lastActivePathfind = null;
        }
        else
        {
            currentPathfindSelected = listID;
            listItems[listID].transform.Find("Active").transform.Find("ActiveBorder").gameObject.SetActive(true);


            Bitcoin bitcoinSelected = itemsToPathfind[currentPathfindSelected];
            if (bitcoinSelected.Chunk != -1) //if it exists in the game at current time
            {
                lastActivePathfind = bitcoinSelected;
            }
        }
        pathfind();
    }

    public void confirmPathfind()
    {
        Debug.Log("Removed");
    }

    public void onHoverPopup()
    {
        if (pathfindUpdateLock)
        {
            pathfindHoverContainer.SetActive(true);
            Sprite image = pathfindHoverContainer.transform.Find("VariantImage").GetComponent<Image>().sprite;
            if (lastActivePathfind.Extras_variation)
            {
                 image = normalCoin;
            }
            else
            {
                image = darkCoin;
            }
            pathfindHoverContainer.transform.Find("VariantImage").GetComponent<Image>().sprite = image;
            pathfindHoverContainer.transform.Find("PathfindItemName").GetComponent<Text>().text = lastActivePathfind.ItemWord;
        }
        
    }

    public void onExitHoverPopup()
    {
        pathfindHoverContainer.SetActive(false);
    }

    public void customInventoryUpdate()
    {

    }

    /*
     * Either new object is selected or player moved with a warp.
     */
    public void pathfind()
    {
        if (lastActivePathfind != null)
        {
            if (pathfindTarget != null)
            {
                //set last pathfind target to default material
                pathfindTarget.GetComponent<GlowController>().setDefaultMaterial();
            }

            (int, int) route = bfsBrain.findNextNode(character.GetComponent<SpriteController>().nodeID, lastActivePathfind.Extras_nodeID);
            if(route.Item1 == route.Item2)
            {
                pathfindTarget = lastActivePathfind.BitcoinGO;
            }
            else
            {
                pathfindTarget = bfsBrain.findExit(route.Item1, route.Item2);
            }
            target_pos = ((int)pathfindTarget.transform.position.x, (int)pathfindTarget.transform.position.y);
            pathfindUpdateLock = true;
            pathfindTarget.GetComponent<GlowController>().setGlowMaterial();
        }
        else
        {
            pathfindUpdateLock = false;
            if (pathfindTarget != null)
            {
                //set last pathfind target to default material
                pathfindTarget.GetComponent<GlowController>().setDefaultMaterial();
                pathfindTarget = null;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!pathfindUpdateLock)
        {
            up_arrow.SetActive(false);
            down_arrow.SetActive(false);
            left_arrow.SetActive(false);
            right_arrow.SetActive(false);
            //panel.text = "";
            //panel_target.text = "";
        }
        else
        {
            up_arrow.SetActive(true);
            down_arrow.SetActive(true);
            left_arrow.SetActive(true);
            right_arrow.SetActive(true);

            //Get the position of the character every update
            character_pos = ((int)character.transform.position.x, (int)character.transform.position.y);

            //panel.text = "X: " + character_pos.Item1 + "\n Y: " + character_pos.Item2;
            //panel_target.text = "X: " + target_pos.Item1 + "\n Y: " + target_pos.Item2;

            //change for when the item is on "screen" to removing arrows
            if(character_pos.Item1 < target_pos.Item1)
            {
                left_arrow.SetActive(false);
                right_arrow.SetActive(true);
            }
            else
            {
                left_arrow.SetActive(true);
                right_arrow.SetActive(false);
            }

            if (character_pos.Item2 < target_pos.Item2)
            {
                up_arrow.SetActive(true);
                down_arrow.SetActive(false);
            }
            else
            {
                up_arrow.SetActive(false);
                down_arrow.SetActive(true);
            }
        }
        
       

    }
}
