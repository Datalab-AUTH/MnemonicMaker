using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk { 
    public Chunk top;
    public Chunk left;
    public Chunk bottom;
    public Chunk right;
    public Chunk topLeft;
    public Chunk topRight;
    public Chunk bottomLeft;
    public Chunk bottomRight;
    private int chunkID;
    private (int,int) cellTopLeft;
    private (int, int) cellTopRight;
    private (int, int) cellBottomLeft;
    private (int, int) cellBottomRight;

    private HashSet<(int, int)> hasCells;
    private List<Chunk> neighbours;

    public Chunk(int chunkID, (int, int) cellTopLeft, (int, int) cellTopRight, (int, int) cellBottomLeft, (int, int) cellBottomRight)
    {
        this.chunkID = chunkID;
        this.cellTopLeft = cellTopLeft;
        this.cellTopRight = cellTopRight;
        this.cellBottomLeft = cellBottomLeft;
        this.cellBottomRight = cellBottomRight;
        /*
        Debug.Log("Chunk: " + chunkID +
            "\n- Bottom Left : " + cellBottomLeft +
            "\n- Top Left : " + cellTopLeft +
            "\n- Bottom Right : " + cellBottomRight +
            "\n- Top Right : " + cellTopRight);
        */
        buildSet();
    }


    private void buildSet()
    {
        hasCells = new HashSet<(int, int)>();
        for(int x= cellBottomLeft.Item1; x<= cellTopRight.Item1; x++)
        {
            for(int y= cellBottomLeft.Item2; y<= cellTopRight.Item2; y++)
            {
                hasCells.Add((x, y));
                //Debug.Log((x, y));
            }
        }
        
    }

    public void buildNeighbours(Chunk top, Chunk bottom, Chunk left, Chunk right, Chunk topLeft, Chunk topRight, Chunk bottomLeft, Chunk bottomRight)
    {
        neighbours = new List<Chunk>();
        this.top = top;
        this.bottom = bottom;
        this.left = left;
        this.right = right;
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;

        //not used for now bad performance?
        neighbours.Add(top);
        neighbours.Add(bottom);
        neighbours.Add(left);
        neighbours.Add(right);
        neighbours.Add(topLeft);
        neighbours.Add(topRight);
        neighbours.Add(bottomLeft);
        neighbours.Add(bottomRight);
    }

    public bool touch((int,int)loc)
    {
        return hasCells.Contains(loc);
    }

    public int getID()
    {
        return chunkID;
    }

    public List<Chunk> getNeighbours()
    {
        return neighbours;
    }
}
