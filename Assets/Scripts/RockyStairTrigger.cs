using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyStairTrigger : MonoBehaviour
{
    private GameObject character;
    public bool belowStairs;
    public int floorGround;
    public int floorCeiling;
    public bool second_floor;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (belowStairs)
        {
            character.GetComponent<SpriteController>().changeFloor(floorGround);
        }
        else
        {
            character.GetComponent<SpriteController>().changeFloor(floorCeiling);
        }
        
    }
}
