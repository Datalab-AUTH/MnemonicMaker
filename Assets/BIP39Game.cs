using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BIP39Game : MonoBehaviour
{
    public Inventory inventory;
    private string mnemonic;
    private string demoMnemonic;
    public bool demo;
    public bool normal;
    public bool practice;
    private List<Bitcoin> bitcoinList; //Contains all information for the mnemonic - for practice/demo mode
    private dotnetstandard_bip39.BIP39 bip39;

    //small hack for portals
    public int portalCounter;
    public bool day;
    public bool gameStarted;
    public int warpsAvailable;
    public UnityEngine.UI.Text warpText; 

    private string[] currentMnemonicArray;
    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        bip39 = new dotnetstandard_bip39.BIP39();
        bitcoinList = new List<Bitcoin>();
        day = false;
        portalCounter = 0;

        string[] itemNames = GetItemNames();
        //Get the BIP-39 wordlist for English languague
        string[] wordlist = GetWordlist(dotnetstandard_bip39.BIP39Wordlist.English);
        demoMnemonic = null;
        currentMnemonicArray = null;
        warpsAvailable = -1;
    }
    
    public string getMnemonic()
    {
        return mnemonic;
    }

    public void setMnemonic(string mnemonic)
    {
        this.mnemonic = mnemonic;
    }

    public bool checkMnemonic(string[] collectedWords)
    {
        int counter = 0;
        foreach(string word in collectedWords)
        {
            if (!currentMnemonicArray[counter].Equals(word)) return false;
            counter++;
        }
        return true;
    }

    public void speedBuff()
    {
        SpriteController spc = GameObject.Find("Character").GetComponent<SpriteController>();
        spc.boost = !spc.boost;
        if (spc.boost)
        {
            spc.runSpeed = spc.runSpeedBoost;
        }
        else
        {
            spc.runSpeed = spc.runSpeedNormal;
        }
    }

    public void warpToHouse()
    {
        if(warpsAvailable == -1 || warpsAvailable > 0)
        {
            CinemachineFader cvFader = GameObject.Find("ScriptLoader").GetComponent<CinemachineFader>();
            cvFader.fadeCamera();
            StartCoroutine(DelayAction(1f));
            if (warpsAvailable > 0)
            {
                warpsAvailable--;
                warpText.text = warpsAvailable.ToString();
            }
        }
        
    }

    IEnumerator DelayAction(float delayTime)
    {

        ChunkController controller = GameObject.Find("ScriptLoader").GetComponent<ChunkController>();
        AudioLibrary audioController = GameObject.Find("Audio").GetComponent<AudioLibrary>();
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.

        //Play area sound 
        audioController.changeSound(0);
        int exit;
        //Warp play position
        if (day)
        {
            GameObject.Find("Character").transform.position = new Vector3(82.55f,34.18f,0f);
            controller.warp("CrabIsland", "");
            exit = 0;
            GameObject.Find("Character").GetComponent<SpriteController>().nodeID = 0;
        }
        else
        {
            GameObject.Find("Character").transform.position = new Vector3(588 + 82.55f, 34.18f, 0f);
            controller.warp("CrabIsland (Night)", "");
            exit = 29;
            GameObject.Find("Character").GetComponent<SpriteController>().nodeID = 29;
        }
        //Change camera 
        GameObject.Find("ScriptLoader").GetComponent<CinemachineSwitch>().triggerCameraSwitch(exit);
        if (!GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39.normal)
            GameObject.Find("ScriptLoader").GetComponent<Globals>().pathfinder.GetComponent<Pathfinding>().pathfind();

        GameObject.Find("ScriptLoader").GetComponent<EffectManager>().changeEffects();
    }

    /*
     * In this game mode the user has to move freely in the map in whatever route he has already practiced.
     * After collecting all the items he needs, he can choose to end the game and he can extract his mnemonic phrase.
     * There is no guarantee on whether the items he collected are correct or wrong. Everything depends on user action.
     */
    public void normalGameMode()
    {
        gameStarted = true;
        demo = false;
        practice = false;
        normal = true;
        warpsAvailable = -1;
        //GameObject.Find("ScriptLoader").GetComponent<EffectManager>().changeEffects();
        warpText.text = "";
    }

    /*
     * In this game mode the user is given a random mnemonic.
     * Then he is guided through the route that he has to take to find all his items.
     * Finally when all items are collected practice mode ends.
     */
    public void demoGameMode()
    {
        gameStarted = true;
        demo = true;
        practice = false;
        normal = false;
        warpsAvailable = 5;
        //Get a random mnemonic to test
        mnemonic = null;
        mnemonic = bip39.GenerateMnemonic(128, dotnetstandard_bip39.BIP39Wordlist.English); //12 word
        //mnemonic = "fee ticket tuna crucial hole young decorate light isolate chuckle country axis";
        //mnemonic = "fee ticket tuna crucial hole supply plunge slogan avocado trigger crucial state";
        //mnemonic = "fee ticket tuna crucial hole supply vendor web rely excuse night cannon";
        string[] mnemonicArray = new string[12];
        mnemonicArray = mnemonic.Split(' ');
        print("Demo mnemonic : " + mnemonic);
        if (mnemonic == null)
        {
            print("Problem");
        }

        createMnemonicRoute(mnemonicArray);
        experimentalLocateBitcoins();
        experimentalPrintRoute();

        GameObject.Find("ScriptLoader").GetComponent<Globals>().pathfinder.SetActive(true);
        GameObject.Find("ScriptLoader").GetComponent<Globals>().pathfinder.GetComponent<Pathfinding>().feedPathfinder(bitcoinList);
        //GameObject.Find("ScriptLoader").GetComponent<EffectManager>().changeEffects();
        currentMnemonicArray = mnemonicArray;
        warpText.text = warpsAvailable.ToString();
    }

    /*
     * In this game mode the user already has a mnemonic phrase and he has to select his words in the correct order.
     * Then he is guided through the route that he has to take to find all his items.
     * Finally when all items are collected practice mode ends.
     */
    public void practiceGameMode(string[] mnemonic)
    {
        gameStarted = true;
        demo = false;
        practice = true;
        normal = false;
        warpsAvailable = 5;

        createMnemonicRoute(mnemonic);
        experimentalLocateBitcoins();
        experimentalPrintRoute();

        GameObject.Find("ScriptLoader").GetComponent<Globals>().pathfinder.SetActive(true);
        GameObject.Find("ScriptLoader").GetComponent<Globals>().pathfinder.GetComponent<Pathfinding>().feedPathfinder(bitcoinList);
        //GameObject.Find("ScriptLoader").GetComponent<EffectManager>().changeEffects();
        currentMnemonicArray = mnemonic;
        warpText.text = warpsAvailable.ToString();
    }

    private void createMnemonicRoute(string[] mnemonic)
    {
        Globals script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        
        foreach (string word in mnemonic)
        {
            Bitcoin bitcoin = script.bitcoinController.findByBitcoinName(word);
            if(bitcoin.BitcoinID != -1)
            {
                bitcoin.Extras_bitcoinWord = word;
                bitcoin.extractVariation(word);
            }
            else
            {
                bitcoin.Extras_bitcoinWord = word + " (In-development)";
                bitcoin.Extras_variation = false;
            }
            bitcoinList.Add(bitcoin);
        }
    }

    private void experimentalPrintRoute()
    {
        int counter = 0;
        foreach(Bitcoin bitcoin in bitcoinList)
        {
            string variationPrint;
            if (bitcoin.Extras_variation)
            {
                variationPrint = "Light";
            }
            else
            {
                variationPrint = "Dark";
            }
            Debug.Log(counter + ". Item name: " + bitcoin.ItemWord + ", Variation: " + variationPrint + ", Word: "+ bitcoin.Extras_bitcoinWord + "\n" +
                        "World: " + bitcoin.World + ", Dungeon: "+ bitcoin.Dungeon + ", Chunk: " + bitcoin.Chunk + ", Chest: " + bitcoin.IsInChest+"\n");
            counter++;
        }
    }

    private void experimentalLocateBitcoins()
    {
        ChunkController chunkController = GameObject.Find("ScriptLoader").GetComponent<Globals>().chunkController;
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
        }
    }

    public void setBitcoinList(List<Bitcoin> bitcoinList)
    {
        this.bitcoinList = bitcoinList;
    }

    public string[] getWordlist()
    {
        return GetWordlist(dotnetstandard_bip39.BIP39Wordlist.English);
    }

    public string[] getWordlistOrdered()
    {
        TextAsset worldListResultsAsset = Resources.Load<TextAsset>("Wordlists/english_backup");
        var fileContents = worldListResultsAsset.text;

        return fileContents.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
    }

    /*
     * Using implementation from https://github.com/elucidsoft/dotnetstandard-bip39/blob/master/dotnetstandard-bip39/BIP39.cs
     * the official .NET C# implementation of BIP-39
     */
    private static string[] GetWordlist(dotnetstandard_bip39.BIP39Wordlist wordlist)
    {
        var wordlists = new Dictionary<string, string>
            {
                {dotnetstandard_bip39.BIP39Wordlist.ChineseSimplified.ToString(), "chinese_simplified"},
                {dotnetstandard_bip39.BIP39Wordlist.ChineseTraditional.ToString(), "chinese_traditional"},
                {dotnetstandard_bip39.BIP39Wordlist.English.ToString(), "english"},
                {dotnetstandard_bip39.BIP39Wordlist.French.ToString(), "french"},
                {dotnetstandard_bip39.BIP39Wordlist.Italian.ToString(), "italian"},
                {dotnetstandard_bip39.BIP39Wordlist.Japanese.ToString(), "japanese"},
                {dotnetstandard_bip39.BIP39Wordlist.Korean.ToString(), "korean"},
                {dotnetstandard_bip39.BIP39Wordlist.Spanish.ToString(), "spanish"}
            };

        var wordListFile = wordlists[wordlist.ToString()];
        
        TextAsset worldListResultsAsset = Resources.Load<TextAsset>("Wordlists/" + wordListFile);
        var fileContents = worldListResultsAsset.text;

        return fileContents.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] GetItemNames()
    {
        TextAsset itemNamesAsset = Resources.Load<TextAsset>("ItemNames");
        var fileContents = itemNamesAsset.text;

        return fileContents.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
    }

}
