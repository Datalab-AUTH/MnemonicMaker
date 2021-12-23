using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordlistController : MonoBehaviour
{
    private string activeLetter;
    private BIP39Game bipReference;
    public RectTransform worldlistContainer;
    public RectTransform worldlistSelectedContainer;
    public GameObject confirmButton;
    public MainMenu mainMenu;
    private List<GameObject> listItemsGO;
    private Dictionary<int, GameObject> listWordsSelectedGOdict;
    private string[] currentMnemonic; //define size.
    private int currentIndexInMnemonic;
    // Start is called before the first frame update
    void Start()
    {
        bipReference = GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39;
        activeLetter = "A";
        listItemsGO = new List<GameObject>();
        filterList();
        currentMnemonic = new string[12];
        currentIndexInMnemonic = 0;
        listWordsSelectedGOdict = new Dictionary<int, GameObject>();
        confirmButton.SetActive(false);
    }

    public void setFilterByLetter(string letter)
    {
        if(activeLetter == letter)
        {
            activeLetter = "";
        }
        else
        {
            activeLetter = letter;
        }
        filterList();
    }

    public void selectWord(string word)
    {
        if(currentIndexInMnemonic != currentMnemonic.Length)
        {
            GameObject newItem = Resources.Load("Prefabs/UI/ListItem") as GameObject;
            newItem = Instantiate(newItem);
            //ADD TO MNEMONIC
            currentMnemonic[currentIndexInMnemonic] = word; //int to string mapping
            listWordsSelectedGOdict.Add(currentIndexInMnemonic, newItem);//game object to int mapping
            currentIndexInMnemonic++;

            newItem.transform.SetParent(worldlistSelectedContainer.transform, false);
            newItem.GetComponentInChildren<Text>().text = word;
            newItem.GetComponent<FilterByLetter>().setWordToButton(word);
            newItem.GetComponent<FilterByLetter>().setSelected(true, currentIndexInMnemonic - 1);//-1 cause we added it up before.
            if(currentIndexInMnemonic == currentMnemonic.Length)
            {
                confirmButton.SetActive(true);
            }
        }
    }

    public void removeWord(int index_of_word)
    {
        //destroy
        //find the go in DIC
        GameObject deleteGO = getGO(index_of_word);
        if(deleteGO != null)
        {
            listWordsSelectedGOdict.Remove(index_of_word); //remove from dict
            Destroy(deleteGO); //destroy GO from list
            string[] newMnemonic = new string[12]; // define size ?
            for(int i=0; i<12; i++)
            {
                if (i < index_of_word)
                {
                    newMnemonic[i] = currentMnemonic[i];
                }
                else if(i == index_of_word)
                {
                    for(int j=i; j<currentMnemonic.Length-1; j++)
                    {
                        newMnemonic[j] = currentMnemonic[j + 1];
                    }
                    break;
                }
                
            }
            currentMnemonic = newMnemonic;
            //remake the dict
            Dictionary<int,GameObject> newDict = new Dictionary<int, GameObject>();
            int counter = 0;
            for(int i=0; i<currentMnemonic.Length; i++)
            {
                GameObject go = getGO(i);
                if (go != null)
                {
                    newDict.Add(counter, go);
                    //reset id in go
                    go.GetComponent<FilterByLetter>().setSelected(true, counter);
                    counter++;
                }
            }
            listWordsSelectedGOdict = new Dictionary<int, GameObject>(newDict);
            currentIndexInMnemonic--;
            
        }
        if (currentIndexInMnemonic != currentMnemonic.Length)
        {
            confirmButton.SetActive(false);
        }
    }

    private GameObject getGO(int key)
    {
        GameObject tempGO;
        //WEBGL adds an invisible-unprintable character at the end of the requested string
        //So trimming the end fixed this error.
        if (!listWordsSelectedGOdict.TryGetValue(key, out tempGO))
        {
            Debug.Log("[this is not an error] GAMEOBJECT LIST LOOKUP: The Provided index: `" + key + "` does not exist!");
            return null; 
        }
        return tempGO;
    }

    private void filterList()
    {
        string[] wordlist = bipReference.getWordlistOrdered();
        char[] characters = activeLetter.ToLower().ToCharArray();
        foreach (GameObject go in listItemsGO)
        {
            Destroy(go);
        }
        if (activeLetter == "")
        {

            foreach (string word in wordlist)
            {
                GameObject newItem = Resources.Load("Prefabs/UI/ListItem") as GameObject;
                newItem = Instantiate(newItem);
                listItemsGO.Add(newItem);
                newItem.transform.SetParent(worldlistContainer.transform, false);
                newItem.GetComponentInChildren<Text>().text = word;
                newItem.GetComponent<FilterByLetter>().setWordToButton(word);
            }

            /*
            for(int i=0; i < 2048; i++)
            {
                GameObject newItem = Resources.Load("Prefabs/UI/ListItem") as GameObject;
                newItem = Instantiate(newItem);
                newItem.transform.parent = worldlistContainer.transform;
                newItem.transform.localScale = new Vector3(1, 1, 1);
                newItem.GetComponentInChildren<Text>().text = "loren ipsum";
            }*/
        }
        else
        {
            bool foundLetterOnce = false;
            foreach (string word in wordlist)
            {
                if (word[0] == characters[0])
                {
                    foundLetterOnce = true;
                    GameObject newItem = Resources.Load("Prefabs/UI/ListItem") as GameObject;
                    newItem = Instantiate(newItem);
                    listItemsGO.Add(newItem);
                    newItem.transform.SetParent(worldlistContainer.transform, false);
                    newItem.GetComponentInChildren<Text>().text = word;
                    newItem.GetComponent<FilterByLetter>().setWordToButton(word);
                }
                else
                {
                    if (foundLetterOnce) break;
                }
                
            }
        }

    }

    public void confirmMnemonic()
    {
        //clear prefabs
        foreach (GameObject go in listItemsGO)
        {
            Destroy(go);
        }
        foreach(GameObject go in listWordsSelectedGOdict.Values)
        {
            Destroy(go);
        }

        Debug.Log("MNEMONIC : " + currentMnemonic);
        //setup mnemonic in bip39
        //signal close
        GameObject.Find("ScriptLoader").GetComponent<Globals>().mainMenuScript.startGamePracticeMode(currentMnemonic);

    }
}
