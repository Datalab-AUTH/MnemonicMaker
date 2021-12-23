using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterByLetter : MonoBehaviour
{
    public WordlistController wcReference;
    public string word;
    public int id = -1;
    public bool isSelected = false;

    //for filtering.
    public void click(string arg)
    {
        wcReference.setFilterByLetter(arg);
    }

    public void setWordToButton(string arg)
    {
        word = arg;
    }

    //for confirmation
    public void confirmMnemonic()
    {
        GameObject.Find("List").GetComponent<WordlistController>().confirmMnemonic();
    }

    public void selectWord()
    {
        if (isSelected)
        {
            //destroy by ID 
            GameObject.Find("List").GetComponent<WordlistController>().removeWord(id);
        }
        else
        {
            GameObject.Find("List").GetComponent<WordlistController>().selectWord(word);
        }
    }

    public void setSelected(bool flag, int indexInArray)
    {
        isSelected = flag;
        id = indexInArray;
    }
}
