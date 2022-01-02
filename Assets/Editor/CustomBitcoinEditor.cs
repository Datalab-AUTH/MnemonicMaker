using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CustomBitcoinEditor : MonoBehaviour
{
    
    public void Generate()
    {
        clear();
        Debug.Log("Cleared old data");
        GameObject[] tagged = GameObject.FindGameObjectsWithTag("Collectibles");
        GameObject[] taggedChests = GameObject.FindGameObjectsWithTag("Collectibles-Chest");
        string[] itemNames = GetItemNames();
        string[] wordlist = GetWordlist(dotnetstandard_bip39.BIP39Wordlist.English);
        int counter = 0;
        int chestCounter = 0;

        if (tagged != null)
        {
            for (int j = 0; j < tagged.Length; j++)
            {
                tagged[j].GetComponent<ItemEdit>().id = j;
                tagged[j].GetComponent<ItemEdit>().itemName = itemNames[j];
                ItemEdit.ItemInfo[] items = new ItemEdit.ItemInfo[2];
                ItemEdit.ItemInfo itemInfo = new ItemEdit.ItemInfo();
                ItemEdit.ItemInfo itemInfo2 = new ItemEdit.ItemInfo();
                items[0] = itemInfo;
                items[1] = itemInfo2;
                itemInfo.bitcoinName = wordlist[counter];
                itemInfo.star = true;
                itemInfo2.bitcoinName = wordlist[counter+1];
                itemInfo2.star = false;
                tagged[j].GetComponent<ItemEdit>().items = items;
                EditorUtility.SetDirty(tagged[j].GetComponent<ItemEdit>());
                counter=counter + 2;
            }
        }
        
        int remaining = 2048 - counter;
        int remainingItems = 1024 - tagged.Length;
        Debug.Log("Used words outside of chests : " + counter);
        Debug.Log("Remaining for chests (words): " + remaining);
        Debug.Log("Remaining for chests (items): " + remainingItems);
        Debug.Log("Chests available : " + (taggedChests.Length));
        (int,int) ndResult = normal_distribution(remainingItems, taggedChests.Length);
        Debug.Log("Best median selected: " + ndResult.Item1 + " - Difference: " + ndResult.Item2);
        Debug.Log("Running ND simulation .. ");
        int[] ndMatrix = runND(ndResult.Item1, remainingItems, taggedChests.Length);

        if (taggedChests != null)
        {
            for (int j = tagged.Length; j < 1024;)
            {   //for each chest
                Debug.Log("Entering with j: " + j);
                Debug.Log("Chest: " + chestCounter + " - Contain: "+ ndMatrix[chestCounter]);
                Transform chestParent = taggedChests[chestCounter].transform;
                //counter = 4 to skip "MinimapTarget" "PickUpUI" "Fade" "BitcoinSelfLight"
                for (int count = 4; count < 4 + ndMatrix[chestCounter]; count++)
                {
                    Transform slot = chestParent.GetChild(count);
                    if (slot.name == "MinimapTarget" || slot.name == "PickUpUI"
                        || slot.name == "BitcoinSelfLight" || slot.name == "Fade")
                    {
                        Debug.LogWarning("chest : " + chestParent.transform.name + " found a non-slot object");
                        // Not handling error
                        continue;
                    }
                    else
                    {
                        slot.GetComponent<ItemEdit>().id = j;
                        slot.GetComponent<ItemEdit>().itemName = itemNames[j];
                        ItemEdit.ItemInfo[] items = new ItemEdit.ItemInfo[2];
                        ItemEdit.ItemInfo itemInfo = new ItemEdit.ItemInfo();
                        ItemEdit.ItemInfo itemInfo2 = new ItemEdit.ItemInfo();
                        items[0] = itemInfo;
                        items[1] = itemInfo2;
                        itemInfo.bitcoinName = wordlist[counter];
                        itemInfo.star = true;
                        itemInfo2.bitcoinName = wordlist[counter + 1];
                        itemInfo2.star = false;
                        slot.GetComponent<ItemEdit>().items = items;
                        EditorUtility.SetDirty(slot.GetComponent<ItemEdit>());
                        counter = counter + 2;
                        j++;
                    }
                }
                chestCounter++;
                Debug.Log("Final chest ID j: " + (j-1));
            }
        }
        Debug.Log("Created " + counter + " items " );
        
    }

    private (int,int) normal_distribution(int items, int chests)
    {
        int result;
        //at least 1 and at maximum 12
        int[] resultArray = new int[6]; // for each median
        int counter = 0;
        for(int median = 1+3; median <=12-3; median++)
        {
            result = (int)( chests * (0.341 * 2) * (median) +
                            chests * (0.136) * (median-1) +
                            chests * (0.136) * (median+1) +
                            chests * (0.021) * (median-2) +
                            chests * (0.021) * (median+2) +
                            chests * (0.001) * (median-3) +
                            chests * (0.001) * (median+3));
            Debug.Log("Median: " + median + " - Items: " + result);
            //99.6 % 
            resultArray[counter] = result;
            counter++;
        }
        int difference = items;
        int bestMedian = 4;
        for(int i = 0; i< resultArray.Length; i++)
        {
            int local_dif = Mathf.Abs(resultArray[i] - items);
            if (local_dif <= difference)
            {
                difference = local_dif;
                bestMedian = i + 1 + 3;
            }
        }
        return (bestMedian, difference);
    }

    private int[] runND(int median, int target, int loops)
    {
        int[] ndMatrix;
        int result;
        int doWhileLoops = 0;
        do
        {
            ndMatrix = new int[loops];
            result = 0;
            for (int i = 0; i < loops; i++)
            {
                int rand = (int)Random.Range(1, 998);
                if (rand == 1)
                {
                    ndMatrix[i] = median - 3;
                }
                else if (rand > 1 && rand <= 22)
                {
                    ndMatrix[i] = median - 2;
                }
                else if (rand > 22 && rand <= 158)
                {
                    ndMatrix[i] = median - 1;
                }
                else if (rand > 158 && rand <= 499)
                {
                    ndMatrix[i] = median;
                }
                else if (rand > 499 && rand <= 840)
                {
                    ndMatrix[i] = median;
                }
                else if (rand > 840 && rand <= 976)
                {
                    ndMatrix[i] = median + 1;
                }
                else if (rand > 976 && rand <= 997)
                {
                    ndMatrix[i] = median + 2;
                }
                else if (rand == 998)
                {
                    ndMatrix[i] = median + 3;
                }
                result += ndMatrix[i];
            }
            doWhileLoops++;
        } while (target != result);

        Debug.Log("Found result after " + doWhileLoops + " loops of ND trialing");
        return ndMatrix;
    }

    private void clear()
    {
        GameObject[] tagged = GameObject.FindGameObjectsWithTag("Collectibles");
        GameObject[] taggedChests = GameObject.FindGameObjectsWithTag("Collectibles-Chest");
        int chestCounter = 0;

        if (tagged != null)
        {
            for (int j = 0; j < tagged.Length; j++)
            {
                tagged[j].GetComponent<ItemEdit>().id = 0;
                tagged[j].GetComponent<ItemEdit>().itemName = "";
                tagged[j].GetComponent<ItemEdit>().items = null;
                EditorUtility.SetDirty(tagged[j].GetComponent<ItemEdit>());
            }
        }
        if (taggedChests != null)
        {
            for (int j = tagged.Length; j < tagged.Length + (taggedChests.Length * 12); j = j + 12)
            {   //for each chest
                // j = 120 -> 132 -> 144 -> 156 .. etc
                Transform chestParent = taggedChests[chestCounter].transform;
                foreach (Transform slot in chestParent)
                {
                    if (!(slot.name == "MinimapTarget" || slot.name == "PickUpUI"
                        || slot.name == "BitcoinSelfLight" || slot.name == "Fade"))
                    {
                        slot.GetComponent<ItemEdit>().id = 0;
                        slot.GetComponent<ItemEdit>().itemName = "";
                        slot.GetComponent<ItemEdit>().items = null;
                        EditorUtility.SetDirty(slot.GetComponent<ItemEdit>());
                    }

                }
                chestCounter++;
            }
        }
    }

    [CustomEditor(typeof(CustomBitcoinEditor))]
    public class CustomInspectorBitcoinEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
            {
                var generator = (CustomBitcoinEditor)target;
                generator.Generate();
                EditorSceneManager.MarkAllScenesDirty();
            }
        }
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

        // var wordListResults = new System.IO.StreamReader(Application.dataPath + "/Resources/Wordlists/" + wordListFile + ".txt");
        TextAsset worldListResultsAsset = Resources.Load<TextAsset>("Wordlists/" + wordListFile);
        var fileContents = worldListResultsAsset.text;
        //var fileContents = wordListResults.ReadToEnd();
        //wordListResults.Close();

        //fileContents.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

        return fileContents.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] GetItemNames()
    {
        TextAsset itemNamesAsset = Resources.Load<TextAsset>("ItemNames");
        var fileContents = itemNamesAsset.text;

        return fileContents.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
    }
}
