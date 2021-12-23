using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bitcoin
{

    private GameObject bitcoinGO;
    private bool isInChest;
    private ItemEdit.ItemInfo[] bitcoinWords;
    private string itemWord;
    private int bitcoinID;
    private (double,double) coordinates;
    private string world;
    private string dungeon;
    private int chunk;
    /*
     * Pathfinding extras
     */
    private string extras_bitcoinWord;
    private bool extras_variation;
    private int extras_nodeID;

    public Bitcoin()
    {
        //constructor
    }

    /*
     * This method is only used in pathfinding. We are storing the word and we only need to derive
     * the variation of the coin and store those 2 extra variables.
     */
    public void extractVariation(string word)
    {
        foreach (ItemEdit.ItemInfo iInfo in bitcoinWords)
        {
            if (iInfo.bitcoinName.Equals(word))
                extras_variation = iInfo.star;
        }
    }

    public string ItemWord { get => itemWord; set => itemWord = value; }
    public GameObject BitcoinGO { get => bitcoinGO; set => bitcoinGO = value; }
    public int BitcoinID { get => bitcoinID; set => bitcoinID = value; }
    public string World { get => world; set => world = value; }
    public string Dungeon { get => dungeon; set => dungeon = value; }
    public int Chunk { get => chunk; set => chunk = value; }
    public ItemEdit.ItemInfo[] BitcoinWords { get => bitcoinWords; set => bitcoinWords = value; }
    public bool IsInChest { get => isInChest; set => isInChest = value; }
    public string Extras_bitcoinWord { get => extras_bitcoinWord; set => extras_bitcoinWord = value; }
    public bool Extras_variation { get => extras_variation; set => extras_variation = value; }
    public (double, double) Coordinates { get => coordinates; set => coordinates = value; }
    public int Extras_nodeID { get => extras_nodeID; set => extras_nodeID = value; }
}
