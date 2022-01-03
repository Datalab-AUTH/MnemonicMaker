using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ItemEdit : MonoBehaviour
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public int id;
    [SerializeField]
    public ItemInfo[] items;

    [System.Serializable]
    public class ItemInfo
    {
        [SerializeField]
        public string bitcoinName;
        [SerializeField]
        public bool star;
    }
    
}
