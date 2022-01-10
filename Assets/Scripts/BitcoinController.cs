using System;
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
        activeBitcoin = temp;
        //switchLights(false); //disable bitcoin lights entirelly
        testBitcoin();
        //logBitcoins();
        //itemDistribution();
    }
    /*
    private void itemDistribution()
    {
        ChunkController chunkController = GameObject.Find("ScriptLoader").GetComponent<Globals>().chunkController;
        //items per chunk
        int[] overworld_day = new int[12];
        int[] dungeon1_day = new int[12];
        int[] dungeon2_day = new int[12];
        int[] dungeon3_day = new int[12];
        int[] overworld_night = new int[12];
        int[] dungeon1_night = new int[12];
        int[] dungeon2_night = new int[12];
        int[] dungeon3_night = new int[12];
        int overworld_day_count = 0;
        int dungeon1_day_count = 0;
        int dungeon2_day_count = 0;
        int dungeon3_day_count = 0;
        int overworld_night_count = 0;
        int dungeon1_night_count = 0;
        int dungeon2_night_count = 0;
        int dungeon3_night_count = 0;
        //
        int day_items = 0;
        int night_items = 0;
        //bitcoins per chunk
        int[] bitcoin_overworld_day = new int[12];
        int[] bitcoin_dungeon1_day = new int[12];
        int[] bitcoin_dungeon2_day = new int[12];
        int[] bitcoin_dungeon3_day = new int[12];
        int[] bitcoin_overworld_night = new int[12];
        int[] bitcoin_dungeon1_night = new int[12];
        int[] bitcoin_dungeon2_night = new int[12];
        int[] bitcoin_dungeon3_night = new int[12];

        int bitcoin_overworld_day_count = 0;
        int bitcoin_dungeon1_day_count = 0;
        int bitcoin_dungeon2_day_count = 0;
        int bitcoin_dungeon3_day_count = 0;
        int bitcoin_overworld_night_count = 0;
        int bitcoin_dungeon1_night_count = 0;
        int bitcoin_dungeon2_night_count = 0;
        int bitcoin_dungeon3_night_count = 0;
        //bitcoin in chests per chunk
        int[] chest_overworld_day = new int[12];
        int[] chest_dungeon1_day = new int[12];
        int[] chest_dungeon2_day = new int[12];
        int[] chest_dungeon3_day = new int[12];
        int[] chest_overworld_night = new int[12];
        int[] chest_dungeon1_night = new int[12];
        int[] chest_dungeon2_night = new int[12];
        int[] chest_dungeon3_night = new int[12];
        int chest_overworld_day_count = 0;
        int chest_dungeon1_day_count = 0;
        int chest_dungeon2_day_count = 0;
        int chest_dungeon3_day_count = 0;
        int chest_overworld_night_count = 0;
        int chest_dungeon1_night_count = 0;
        int chest_dungeon2_night_count = 0;
        int chest_dungeon3_night_count = 0;
        //
        int day_bitcoins = 0;
        int night_bitcoins = 0;
        int day_chests = chestParent.transform.childCount;
        int night_chests = chestParentNight.transform.childCount;
        //chests per chunk
        int[] chests_overworld_day = new int[12];
        int[] chests_dungeon1_day = new int[12];
        int[] chests_dungeon2_day = new int[12];
        int[] chests_dungeon3_day = new int[12];
        int[] chests_overworld_night = new int[12];
        int[] chests_dungeon1_night = new int[12];
        int[] chests_dungeon2_night = new int[12];
        int[] chests_dungeon3_night = new int[12];
        int chests_overworld_day_count = 0;
        int chests_dungeon1_day_count = 0;
        int chests_dungeon2_day_count = 0;
        int chests_dungeon3_day_count = 0;
        int chests_overworld_night_count = 0;
        int chests_dungeon1_night_count = 0;
        int chests_dungeon2_night_count = 0;
        int chests_dungeon3_night_count = 0;
        //
        int day_itemInChests = 0;
        int night_itemInChests = 0;

        foreach (Bitcoin bitcoin in bitcoinList)
        {
            if (bitcoin.BitcoinID != -1)
            {
                Vector3 bitcoinPos = new Vector3(bitcoin.BitcoinGO.transform.localPosition.x + bitcoin.BitcoinGO.transform.parent.localPosition.x,
                                                 bitcoin.BitcoinGO.transform.localPosition.y + bitcoin.BitcoinGO.transform.parent.localPosition.y,
                                                 bitcoin.BitcoinGO.transform.localPosition.z + bitcoin.BitcoinGO.transform.parent.localPosition.z);
                bitcoin.Coordinates = (bitcoinPos.x, bitcoinPos.y);
                chunkController.locateBitcoin(bitcoin);
            }
            else
            {
                bitcoin.World = "In-development";
                bitcoin.Chunk = -1;
                bitcoin.Dungeon = "In-development";
                bitcoin.Extras_nodeID = -1;
            }

            if (bitcoin.World.Contains("Night"))
            {
                night_items++;
                if (bitcoin.IsInChest) night_itemInChests++;
                else night_bitcoins++;
                switch (bitcoin.Dungeon){
                    case "":
                        overworld_night[bitcoin.Chunk]++;
                        break;
                    case "Dungeon1":
                        dungeon1_night[bitcoin.Chunk]++;
                        break;
                    case "Dungeon2":
                        dungeon2_night[bitcoin.Chunk]++;
                        break;
                    case "Dungeon3":
                        dungeon3_night[bitcoin.Chunk]++;
                        break;
                }
                if (bitcoin.IsInChest)
                {
                    switch (bitcoin.Dungeon)
                    {
                        case "":
                            chest_overworld_night[bitcoin.Chunk]++;
                            break;
                        case "Dungeon1":
                            chest_dungeon1_night[bitcoin.Chunk]++;
                            break;
                        case "Dungeon2":
                            chest_dungeon2_night[bitcoin.Chunk]++;
                            break;
                        case "Dungeon3":
                            chest_dungeon3_night[bitcoin.Chunk]++;
                            break;
                    }
                }
                else
                {
                    switch (bitcoin.Dungeon)
                    {
                        case "":
                            bitcoin_overworld_night[bitcoin.Chunk]++;
                            break;
                        case "Dungeon1":
                            bitcoin_dungeon1_night[bitcoin.Chunk]++;
                            break;
                        case "Dungeon2":
                            bitcoin_dungeon2_night[bitcoin.Chunk]++;
                            break;
                        case "Dungeon3":
                            bitcoin_dungeon3_night[bitcoin.Chunk]++;
                            break;
                    }
                }
            }
            else
            {
                day_items++;
                if (bitcoin.IsInChest) day_itemInChests++;
                else day_bitcoins++;
                switch (bitcoin.Dungeon)
                {
                    case "":
                        overworld_day[bitcoin.Chunk]++;
                        break;
                    case "Dungeon1":
                        dungeon1_day[bitcoin.Chunk]++;
                        break;
                    case "Dungeon2":
                        dungeon2_day[bitcoin.Chunk]++;
                        break;
                    case "Dungeon3":
                        dungeon3_day[bitcoin.Chunk]++;
                        break;
                }
                if (bitcoin.IsInChest)
                {
                    switch (bitcoin.Dungeon)
                    {
                        case "":
                            chest_overworld_day[bitcoin.Chunk]++;
                            break;
                        case "Dungeon1":
                            chest_dungeon1_day[bitcoin.Chunk]++;
                            break;
                        case "Dungeon2":
                            chest_dungeon2_day[bitcoin.Chunk]++;
                            break;
                        case "Dungeon3":
                            chest_dungeon3_day[bitcoin.Chunk]++;
                            break;
                    }
                }
                else
                {
                    switch (bitcoin.Dungeon)
                    {
                        case "":
                            bitcoin_overworld_day[bitcoin.Chunk]++;
                            break;
                        case "Dungeon1":
                            bitcoin_dungeon1_day[bitcoin.Chunk]++;
                            break;
                        case "Dungeon2":
                            bitcoin_dungeon2_day[bitcoin.Chunk]++;
                            break;
                        case "Dungeon3":
                            bitcoin_dungeon3_day[bitcoin.Chunk]++;
                            break;
                    }
                }
            }
        }

        foreach(Transform tr in chestParentNight.transform)
        {
            Chest chest = tr.GetComponent<Chest>();
            Vector3 bitcoinPos = new Vector3(tr.localPosition.x + tr.parent.localPosition.x,
                                                 tr.localPosition.y + tr.parent.localPosition.y,
                                                 tr.localPosition.z + tr.parent.localPosition.z);
            chest.Coordinates = (bitcoinPos.x, bitcoinPos.y);
            chunkController.locateChest(chest);
            switch (chest.Dungeon)
            {
                case "":
                    chests_overworld_night[chest.Chunk]++;
                    break;
                case "Dungeon1":
                    chests_dungeon1_night[chest.Chunk]++;
                    break;
                case "Dungeon2":
                    chests_dungeon2_night[chest.Chunk]++;
                    break;
                case "Dungeon3":
                    chests_dungeon3_night[chest.Chunk]++;
                    break;
            }
        }

        foreach (Transform tr in chestParent.transform)
        {
            Chest chest = tr.GetComponent<Chest>();
            Vector3 bitcoinPos = new Vector3(tr.localPosition.x + tr.parent.localPosition.x,
                                                 tr.localPosition.y + tr.parent.localPosition.y,
                                                 tr.localPosition.z + tr.parent.localPosition.z);
            chest.Coordinates = (bitcoinPos.x, bitcoinPos.y);
            chunkController.locateChest(chest);
            switch (chest.Dungeon)
            {
                case "":
                    chests_overworld_day[chest.Chunk]++;
                    break;
                case "Dungeon1":
                    chests_dungeon1_day[chest.Chunk]++;
                    break;
                case "Dungeon2":
                    chests_dungeon2_day[chest.Chunk]++;
                    break;
                case "Dungeon3":
                    chests_dungeon3_day[chest.Chunk]++;
                    break;
            }
        }

        Array.ForEach(overworld_day, delegate (int i) { overworld_day_count += i; });
        Array.ForEach(dungeon1_day, delegate (int i) { dungeon1_day_count += i; });
        Array.ForEach(dungeon2_day, delegate (int i) { dungeon2_day_count += i; });
        Array.ForEach(dungeon3_day, delegate (int i) { dungeon3_day_count += i; });
        Array.ForEach(overworld_night, delegate (int i) { overworld_night_count += i; });
        Array.ForEach(dungeon1_night, delegate (int i) { dungeon1_night_count += i; });
        Array.ForEach(dungeon2_night, delegate (int i) { dungeon2_night_count += i; });
        Array.ForEach(dungeon3_night, delegate (int i) { dungeon3_night_count += i; });

        Array.ForEach(bitcoin_overworld_day, delegate (int i) { bitcoin_overworld_day_count += i; });
        Array.ForEach(bitcoin_dungeon1_day, delegate (int i) { bitcoin_dungeon1_day_count += i; });
        Array.ForEach(bitcoin_dungeon2_day, delegate (int i) { bitcoin_dungeon2_day_count += i; });
        Array.ForEach(bitcoin_dungeon3_day, delegate (int i) { bitcoin_dungeon3_day_count += i; });
        Array.ForEach(bitcoin_overworld_night, delegate (int i) { bitcoin_overworld_night_count += i; });
        Array.ForEach(bitcoin_dungeon1_night, delegate (int i) { bitcoin_dungeon1_night_count += i; });
        Array.ForEach(bitcoin_dungeon2_night, delegate (int i) { bitcoin_dungeon2_night_count += i; });
        Array.ForEach(bitcoin_dungeon3_night, delegate (int i) { bitcoin_dungeon3_night_count += i; });

        Array.ForEach(chest_overworld_day, delegate (int i) { chest_overworld_day_count += i; });
        Array.ForEach(chest_dungeon1_day, delegate (int i) { chest_dungeon1_day_count += i; });
        Array.ForEach(chest_dungeon2_day, delegate (int i) { chest_dungeon2_day_count += i; });
        Array.ForEach(chest_dungeon3_day, delegate (int i) { chest_dungeon3_day_count += i; });
        Array.ForEach(chest_overworld_night, delegate (int i) { chest_overworld_night_count += i; });
        Array.ForEach(chest_dungeon1_night, delegate (int i) { chest_dungeon1_night_count += i; });
        Array.ForEach(chest_dungeon2_night, delegate (int i) { chest_dungeon2_night_count += i; });
        Array.ForEach(chest_dungeon3_night, delegate (int i) { chest_dungeon3_night_count += i; });

        Array.ForEach(chests_overworld_day, delegate (int i) { chests_overworld_day_count += i; });
        Array.ForEach(chests_dungeon1_day, delegate (int i) { chests_dungeon1_day_count += i; });
        Array.ForEach(chests_dungeon2_day, delegate (int i) { chests_dungeon2_day_count += i; });
        Array.ForEach(chests_dungeon3_day, delegate (int i) { chests_dungeon3_day_count += i; });
        Array.ForEach(chests_overworld_night, delegate (int i) { chests_overworld_night_count += i; });
        Array.ForEach(chests_dungeon1_night, delegate (int i) { chests_dungeon1_night_count += i; });
        Array.ForEach(chests_dungeon2_night, delegate (int i) { chests_dungeon2_night_count += i; });
        Array.ForEach(chests_dungeon3_night, delegate (int i) { chests_dungeon3_night_count += i; });

        string path = "Assets/Resources/item_distribution.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Total Items: " + (day_items + night_items));
        writer.WriteLine("Day Items: " + (day_items));
        writer.WriteLine("Night Items: " + (night_items));
        writer.WriteLine("\n");
        writer.WriteLine("Total Bitcoins: " + (day_bitcoins + night_bitcoins));
        writer.WriteLine("Day Bitcoins: " + (day_bitcoins));
        writer.WriteLine("Night Bitcoins: " + (night_bitcoins));
        writer.WriteLine("\n");
        writer.WriteLine("Total Chests: " + (day_chests + night_chests));
        writer.WriteLine("Day Chests: " + (day_chests ));
        writer.WriteLine("Night Chests: " + (night_chests));
        writer.WriteLine("\n");
        writer.WriteLine("Total Bitcoins in Chests: " + (day_itemInChests + night_itemInChests));
        writer.WriteLine("Day Bitcoins in Chests: " + (day_itemInChests));
        writer.WriteLine("Night Bitcoins in Chests: " + (night_itemInChests));
        writer.WriteLine("\n ========================== \n");
        writer.WriteLine("Items per chunk");
        writer.WriteLine("---Day");
        writer.WriteLine("------Overworld - " + overworld_day_count);
        writer.WriteLine("---------"+ string.Join(" ", overworld_day));
        writer.WriteLine("------Dungeon 1 - " + dungeon1_day_count);
        writer.WriteLine("---------" + string.Join(" ", dungeon1_day));
        writer.WriteLine("------Dungeon 2 - " + dungeon2_day_count);
        writer.WriteLine("---------" + string.Join(" ", dungeon2_day));
        writer.WriteLine("------Dungeon 3 - " + dungeon3_day_count);
        writer.WriteLine("---------" + string.Join(" ", dungeon3_day));
        writer.WriteLine("---Night");
        writer.WriteLine("------Overworld - " + overworld_night_count);
        writer.WriteLine("---------" + string.Join(" ", overworld_night));
        writer.WriteLine("------Dungeon 1 - " + dungeon1_night_count);
        writer.WriteLine("---------" + string.Join(" ", dungeon1_night));
        writer.WriteLine("------Dungeon 2 - " + dungeon2_night_count);
        writer.WriteLine("---------" + string.Join(" ", dungeon2_night));
        writer.WriteLine("------Dungeon 3 - " + dungeon3_night_count);
        writer.WriteLine("---------" + string.Join(" ", dungeon3_night));
        writer.WriteLine("\n ========================== \n");
        writer.WriteLine("Bitcoins per chunk");
        writer.WriteLine("---Day");
        writer.WriteLine("------Overworld - " + bitcoin_overworld_day_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_overworld_day));
        writer.WriteLine("------Dungeon 1 - " + bitcoin_dungeon1_day_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_dungeon1_day));
        writer.WriteLine("------Dungeon 2 - " + bitcoin_dungeon2_day_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_dungeon2_day));
        writer.WriteLine("------Dungeon 3 - " + bitcoin_dungeon3_day_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_dungeon3_day));
        writer.WriteLine("---Night");
        writer.WriteLine("------Overworld - " + bitcoin_overworld_night_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_overworld_night));
        writer.WriteLine("------Dungeon 1 - " + bitcoin_dungeon1_night_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_dungeon1_night));
        writer.WriteLine("------Dungeon 2 - " + bitcoin_dungeon2_night_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_dungeon2_night));
        writer.WriteLine("------Dungeon 3 - " + bitcoin_dungeon3_night_count);
        writer.WriteLine("---------" + string.Join(" ", bitcoin_dungeon3_night));
        writer.WriteLine("\n ========================== \n");
        writer.WriteLine("Chests per chunk");
        writer.WriteLine("---Day");
        writer.WriteLine("------Overworld - " + chests_overworld_day_count);
        writer.WriteLine("---------" + string.Join(" ", chests_overworld_day));
        writer.WriteLine("------Dungeon 1 - " + chests_dungeon1_day_count);
        writer.WriteLine("---------" + string.Join(" ", chests_dungeon1_day));
        writer.WriteLine("------Dungeon 2 - " + chests_dungeon2_day_count);
        writer.WriteLine("---------" + string.Join(" ", chests_dungeon2_day));
        writer.WriteLine("------Dungeon 3 - " + chests_dungeon3_day_count);
        writer.WriteLine("---------" + string.Join(" ", chests_dungeon3_day));
        writer.WriteLine("---Night");
        writer.WriteLine("------Overworld - " + chests_overworld_night_count);
        writer.WriteLine("---------" + string.Join(" ", chests_overworld_night));
        writer.WriteLine("------Dungeon 1 - " + chests_dungeon1_night_count);
        writer.WriteLine("---------" + string.Join(" ", chests_dungeon1_night));
        writer.WriteLine("------Dungeon 2 - " + chests_dungeon2_night_count);
        writer.WriteLine("---------" + string.Join(" ", chests_dungeon2_night));
        writer.WriteLine("------Dungeon 3 - " + chests_dungeon3_night_count);
        writer.WriteLine("---------" + string.Join(" ", chests_dungeon3_night));
        writer.WriteLine("\n ========================== \n");
        writer.WriteLine("Bitcoins in chests per chunk");
        writer.WriteLine("---Day");
        writer.WriteLine("------Overworld - " + chest_overworld_day_count);
        writer.WriteLine("---------" + string.Join(" ", chest_overworld_day));
        writer.WriteLine("------Dungeon 1 - " + chest_dungeon1_day_count);
        writer.WriteLine("---------" + string.Join(" ", chest_dungeon1_day));
        writer.WriteLine("------Dungeon 2 - " + chest_dungeon2_day_count);
        writer.WriteLine("---------" + string.Join(" ", chest_dungeon2_day));
        writer.WriteLine("------Dungeon 3 - " + chest_dungeon3_day_count);
        writer.WriteLine("---------" + string.Join(" ", chest_dungeon3_day));
        writer.WriteLine("---Night");
        writer.WriteLine("------Overworld - " + chest_overworld_night_count);
        writer.WriteLine("---------" + string.Join(" ", chest_overworld_night));
        writer.WriteLine("------Dungeon 1 - " + chest_dungeon1_night_count);
        writer.WriteLine("---------" + string.Join(" ", chest_dungeon1_night));
        writer.WriteLine("------Dungeon 2 - " + chest_dungeon2_night_count);
        writer.WriteLine("---------" + string.Join(" ", chest_dungeon2_night));
        writer.WriteLine("------Dungeon 3 - " + chest_dungeon3_night_count);
        writer.WriteLine("---------" + string.Join(" ", chest_dungeon3_night));

        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("bitcoin_data") as TextAsset;
    }
     */
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
        if (bitcoinID == -1) return temp;
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
        if (bitcoinID == -1) return null;
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
