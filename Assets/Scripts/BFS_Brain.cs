using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS_Brain : MonoBehaviour
{
    public GameObject nodesDay;
    public GameObject nodesNight;
    public GameObject portalsDay; //used for automatic edging.
    public GameObject portalsNight; //used for automatic edging.
    public GameObject portalDayNight; //used for automatic edging.
    private List<Portal> portalList;
    private List<BFS_Nodes> nodes;
    private int[] vertices;
    private Tuple<int, int>[] edges;
    private BFS bfs;
    // Start is called before the first frame update
    void Start()
    {
        portalList = new List<Portal>();
        portalList.Add(portalDayNight.GetComponent<Portal>());
        foreach (Transform node in portalsDay.transform)
        {
            portalList.Add(node.GetComponent<Portal>());
        }
        foreach (Transform node in portalsNight.transform)
        {
            portalList.Add(node.GetComponent<Portal>());
        }

        nodes = new List<BFS_Nodes>();
        foreach (Transform node in nodesDay.transform)
        {
            nodes.Add(node.GetComponent<BFS_Nodes>());
        }
        foreach (Transform node in nodesNight.transform)
        {
            nodes.Add(node.GetComponent<BFS_Nodes>());
        }

        vertices = createVertices();
        edges = createEdges();
        bfs = new BFS(vertices,edges);
        //findNextNode(2, 5);
    }

    public (int,int) findNextNode(int currentNode, int finalNode)
    {
        Debug.Log("Initiating route from " + currentNode + " to " + finalNode);
        if (currentNode != finalNode)
        {
            List<int> path = bfs.findPath(currentNode, finalNode);
            Debug.Log("Path from " + path[0] + " to " + path[1]);
            return (path[0], path[1]);
        }
        else
        {
            return (currentNode, finalNode);
        }
    }

    public GameObject findExit(int currentNode, int exitNode)
    {
        foreach (Portal portal in portalList)
        {
            Portal.Gate entry = portal.entryNode;
            Portal.Gate exit = portal.exitNode;
            if(entry.gate.transform.name == "DUMMY" || exit.gate.transform.name == "DUMMY")
            {
                Debug.Log("Edges: skip dummy");
                continue;
            }
            if(entry.nodeID == currentNode && exit.nodeID == exitNode)
            {
                return entry.gate;
            }
            else if(entry.nodeID == exitNode && exit.nodeID == currentNode)
            {
                return exit.gate;
            }
        }
        return null;
    }

    private GameObject findNodeByID(int nodeID)
    {
        foreach (BFS_Nodes node in nodes)
        {
            if(node.nodeID == nodeID)
            {
                return node.gameObject;
            }
        }
        return null;
    }

    private int[] createVertices()
    {
        int[] vertices = new int[nodes.Count];
        int counter = 0;
        foreach (BFS_Nodes node in nodes)
        {
            int nodeID = node.nodeID;
            vertices[counter] = nodeID;
            counter++;
        }
        return vertices;
    }

    private Tuple<int, int>[] createEdges()
    {
        HashSet<Tuple<int, int>> set = new HashSet<Tuple<int, int>>();
        foreach (Portal portal in portalList)
        {
            Portal.Gate entry = portal.entryNode;
            Portal.Gate exit = portal.exitNode;
            if (entry != null && exit != null)
            {
                Tuple<int, int> tuple = Tuple.Create(portal.entryNode.nodeID, portal.exitNode.nodeID);
                set.Add(tuple);
            }
            
        }
        Tuple<int, int>[] tupleArray = new Tuple<int, int>[set.Count];
        int counter = 0;
        foreach (Tuple<int, int> tuple in set)
        {
            tupleArray[counter] = tuple;
            counter++;
        }
        return tupleArray;
    }
}
