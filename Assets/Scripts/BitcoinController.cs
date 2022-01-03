using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEditor;
using UnityEngine;

public class BitcoinController : MonoBehaviour
{
    public GameObject bitcoinParent;
    public GameObject bitcoinParentNight;
    public GameObject chestParent;
    public GameObject chestParentNight;
    private List<Bitcoin> bitcoinList;
    private Bitcoin temp;
    private Bitcoin activeBitcoin;
    public bool lightsOn;

    public TextAsset bitcoinData;

    // Start is called before the first frame update
    void Start()
    {
        bitcoinList = new List<Bitcoin>();
        createBitcoinItems(bitcoinParent);
        createBitcoinItems(bitcoinParentNight);
        createBitcoinItems(chestParent);
        createBitcoinItems(chestParentNight);
        temp = new Bitcoin();
        temp.BitcoinWords = new ItemEdit.ItemInfo[2];
        temp.BitcoinWords[0] = new ItemEdit.ItemInfo();
        temp.BitcoinWords[1] = new ItemEdit.ItemInfo();
        temp.BitcoinWords[0].bitcoinName = "In-development";
        temp.BitcoinWords[1].bitcoinName = "In-development";
        temp.ItemWord = "In-development";
        temp.BitcoinID = -1;
        activeBitcoin = null;
        //switchLights(false); //disable bitcoin lights entirelly
        testBitcoin();
        //logBitcoins();
    }
    /*
    private void logBitcoins()
    {
        string path = "Assets/Resources/bitcoin_data.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine("BitcoinsDay");
        foreach (Transform child in bitcoinParent.transform)
        {
            ItemEdit itemInfo = child.GetComponent<ItemEdit>();
            writer.WriteLine(child.localPosition.x + "," + child.localPosition.y + "-" + itemInfo.id + "," + itemInfo.itemName + ":"+ itemInfo.items[0].bitcoinName + "-" + itemInfo.items[1].bitcoinName);
        }
        writer.WriteLine("\n");
        writer.WriteLine("BitcoinsNight");
        foreach (Transform child in bitcoinParentNight.transform)
        {
            ItemEdit itemInfo = child.GetComponent<ItemEdit>();
            writer.WriteLine(child.localPosition.x + "," + child.localPosition.y + "-" + itemInfo.id + "," + itemInfo.itemName + ":" + itemInfo.items[0].bitcoinName + "-" + itemInfo.items[1].bitcoinName);
        }
        writer.WriteLine("\n");
        writer.WriteLine("chestParent");
        foreach (Transform child in chestParent.transform)
        {
            writer.WriteLine(child.localPosition.x + "," + child.localPosition.y);
            foreach (Transform chestChild in child)
            {
                if (!(chestChild.name == "MinimapTarget" || chestChild.name == "PickUpUI"
                    || chestChild.name == "BitcoinSelfLight" || chestChild.name == "Fade"))
                {
                    ItemEdit itemInfoChest = chestChild.GetComponent<ItemEdit>();
                    if (!itemInfoChest.itemName.Equals(""))
                    {
                        writer.WriteLine(itemInfoChest.id + "," + itemInfoChest.itemName + ":" + itemInfoChest.items[0].bitcoinName + "-" + itemInfoChest.items[1].bitcoinName);
                    }

                }
            }
        }
        writer.WriteLine("\n");
        writer.WriteLine("chestParentNight");
        foreach (Transform child in chestParentNight.transform)
        {
            writer.WriteLine(child.localPosition.x + "," + child.localPosition.y);
            foreach (Transform chestChild in child)
            {
                if (!(chestChild.name == "MinimapTarget" || chestChild.name == "PickUpUI"
                    || chestChild.name == "BitcoinSelfLight" || chestChild.name == "Fade"))
                {
                    ItemEdit itemInfoChest = chestChild.GetComponent<ItemEdit>();
                    if (!itemInfoChest.itemName.Equals(""))
                    {
                        writer.WriteLine(itemInfoChest.id + "," + itemInfoChest.itemName + ":" + itemInfoChest.items[0].bitcoinName + "-" + itemInfoChest.items[1].bitcoinName);
                    }

                }
            }
        }
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("bitcoin_data") as TextAsset;

        //Print the text from the file
        Debug.Log(asset.text);
    }*/

    private void testBitcoin()
    {
        HashSet<string> words = new HashSet<string>();
        foreach(Bitcoin bit in bitcoinList)
        {
            ItemEdit.ItemInfo[] ii = bit.BitcoinWords;
            if (!words.Add(ii[0].bitcoinName))
            {
                Debug.Log("======= " + ii[0].bitcoinName + " FOUND DUPLICATE ======");
            }
            if (!words.Add(ii[1].bitcoinName))
            {
                Debug.Log("======= " + ii[0].bitcoinName + " FOUND DUPLICATE ======");
            }
        }
        Debug.Log("======= UNIQUE WORDS FOUND : "+ words.Count+ " ======");
    }

    public void switchLights(bool on)
    {
        lightsOn = on;
        foreach(Bitcoin bit in bitcoinList)
        {
            GameObject go = bit.BitcoinGO;
            go.transform.Find("BitcoinSelfLight").gameObject.SetActive(lightsOn);
        }
    }

    private void createBitcoinItems(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if(child.tag == "Collectibles")
            {
                Bitcoin newItem = new Bitcoin();
                ItemEdit itemInfo = child.GetComponent<ItemEdit>();
                //set Basic information
                newItem.BitcoinGO = child.gameObject;
                newItem.IsInChest = false;
                newItem.ItemWord = itemInfo.itemName;
                newItem.BitcoinID = itemInfo.id;
                newItem.BitcoinWords = new ItemEdit.ItemInfo[2];
                newItem.BitcoinWords[0] = itemInfo.items[0];
                newItem.BitcoinWords[1] = itemInfo.items[1];
                //set advanced

                bitcoinList.Add(newItem);
                //Debug.Log(child.GetComponent<ItemEdit>().itemName);
            }
            else if(child.tag == "Collectibles-Chest")
            {
                Chest chest = child.GetComponent<Chest>();
                foreach(Transform chestChild in child)
                {
                    if(!(chestChild.name == "MinimapTarget" || chestChild.name == "PickUpUI"
                        || chestChild.name == "BitcoinSelfLight" || chestChild.name == "Fade"))
                    {
                        ItemEdit itemInfo = chestChild.GetComponent<ItemEdit>();
                        if (!itemInfo.itemName.Equals(""))
                        {//not empty chest slots only
                            Bitcoin newItem = new Bitcoin();
                            //set Basic information
                            newItem.BitcoinGO = child.gameObject;
                            newItem.IsInChest = true;
                            newItem.ItemWord = itemInfo.itemName;
                            newItem.BitcoinID = itemInfo.id;
                            newItem.BitcoinWords = new ItemEdit.ItemInfo[2];
                            newItem.BitcoinWords[0] = itemInfo.items[0];
                            newItem.BitcoinWords[1] = itemInfo.items[1];
                            //set advanced

                            bitcoinList.Add(newItem);
                            chest.setContent(newItem);
                            //Debug.Log(chestChild.GetComponent<ItemEdit>().itemName);
                        }
                        
                    }
                }
            }
            
        }
    }

    public List<Bitcoin> getBitcoinList()
    {
        return bitcoinList;
    }

    public void setActiveBitcoin(int bitcoinID)
    {
        activeBitcoin = findByBitcoinID(bitcoinID);
    }

    public void pickBitcoin(bool variant)
    {
        //This method should speak directly with inventory controller and add the coin in the inventory
        Inventory.InventoryItem item = new Inventory.InventoryItem();
        item.itemName = activeBitcoin.ItemWord;
        item.parentID = activeBitcoin.BitcoinID;
        item.star = variant;
        GameObject.Find("ScriptLoader").GetComponent<Globals>().inventoryScript.itemCollected(item);
    }

    public Bitcoin findByBitcoinName(string bitcoinName)
    {
        foreach(Bitcoin bitcoin in bitcoinList)
        {
            if(bitcoin.BitcoinWords[0].bitcoinName.Equals(bitcoinName)
                ||
               bitcoin.BitcoinWords[1].bitcoinName.Equals(bitcoinName))
            {
                return bitcoin;
            }
            if (bitcoin.BitcoinWords[0].bitcoinName==bitcoinName
                ||
               bitcoin.BitcoinWords[1].bitcoinName==bitcoinName)
            {
                return bitcoin;
            }
        }
        return temp;
    }

    public Bitcoin findByBitcoinID(int bitcoinID)
    {
        foreach (Bitcoin bitcoin in bitcoinList)
        {
            if (bitcoin.BitcoinID == bitcoinID)
            {
                return bitcoin;
            }
        }
        return temp;
    }

    public string findWordByBitcoin(int bitcoinID, bool variation)
    {
        foreach (Bitcoin bitcoin in bitcoinList)
        {
            if (bitcoin.BitcoinID == bitcoinID)
            {
                if (bitcoin.BitcoinWords[0].star == variation)
                {
                    return bitcoin.BitcoinWords[0].bitcoinName;
                }
                else if(bitcoin.BitcoinWords[1].star == variation)
                {
                    return bitcoin.BitcoinWords[1].bitcoinName;
                }
            }
        }
        return null;
    }

}
