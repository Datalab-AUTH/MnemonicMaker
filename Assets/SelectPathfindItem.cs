using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPathfindItem : MonoBehaviour
{
    public int id;
    
    public void click()
    {
        //GameObject.Find("PathfindControl").GetComponent<Pathfinding>().pathfindSelected(id);

        GameObject.Find("ScriptLoader").GetComponent<Globals>().pathfinder.GetComponent<Pathfinding>().pathfindSelected(id);
    }
}
