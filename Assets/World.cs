
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public string name;
    public int id;
    public int HORIZONTAL_CHUNK_SIZE;
    public int VERTICAL_CHUNK_SIZE;
    public List<Dungeon> dungeons;
    public List<Chunk> chunks;
    private Dictionary<string, Dungeon> dictDungeons;
    private int x_bound;
    private int y_bound; //bound -> the bottom left corner pos
    //public int offset_x;
    private string worldVariant;
    
    public World(string name, int id, int horizontal, int vertical, int x_offset, int y_offset, string worldVariant)
    {
        this.name = name;
        this.id = id;
        HORIZONTAL_CHUNK_SIZE = horizontal;
        VERTICAL_CHUNK_SIZE = vertical;
        x_bound = x_offset * HORIZONTAL_CHUNK_SIZE * 49; //look at chunkcontroller.cs
        y_bound = y_offset * VERTICAL_CHUNK_SIZE * 27; //look at chunkcontroller.cs
        dictDungeons = new Dictionary<string, Dungeon>();
        dungeons = new List<Dungeon>();
        //this.offset_x = offset_x;
        this.worldVariant = worldVariant;
    }

    public Dungeon getDungeon(string dungeonName)
    {
        Dungeon tempDungeon;
        //WEBGL adds an invisible-unprintable character at the end of the requested string
        //So trimming the end fixed this error.
        if (!dictDungeons.TryGetValue(dungeonName.Trim(), out tempDungeon))
        {
            Debug.LogError("The Provided world `" + dungeonName.Trim() + "` does not exist!");
            return null;
        }
        return tempDungeon;
    }

    public void addDungeon(Dungeon dungeon)
    {
        dungeons.Add(dungeon);
        dictDungeons.Add(dungeon.name, dungeon);
    }

    public string getWorldVariant()
    {
        return worldVariant;
    }

    public void loadChunk(List<Chunk> chunks)
    {
        this.chunks = chunks;
        for(int i = 0; i< HORIZONTAL_CHUNK_SIZE*VERTICAL_CHUNK_SIZE; i++)
        {
            if ((i % HORIZONTAL_CHUNK_SIZE) == 0)//Right chunks
            {
                chunks[i].buildNeighbours((i + HORIZONTAL_CHUNK_SIZE >= 0 && i + HORIZONTAL_CHUNK_SIZE < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + HORIZONTAL_CHUNK_SIZE] : null,
                                          (i - HORIZONTAL_CHUNK_SIZE >= 0 && i - HORIZONTAL_CHUNK_SIZE < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - HORIZONTAL_CHUNK_SIZE] : null,
                                          null,
                                          (i + 1 >= 0 && i + 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + 1] : null,
                                          null,
                                          (i + HORIZONTAL_CHUNK_SIZE + 1 >= 0 && i + HORIZONTAL_CHUNK_SIZE + 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + HORIZONTAL_CHUNK_SIZE + 1] : null,
                                          null,
                                          (i - HORIZONTAL_CHUNK_SIZE + 1 >= 0 && i - HORIZONTAL_CHUNK_SIZE + 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - HORIZONTAL_CHUNK_SIZE + 1] : null);
            }
            else if((i % HORIZONTAL_CHUNK_SIZE) == HORIZONTAL_CHUNK_SIZE - 1)//Left chunks
            {
                chunks[i].buildNeighbours((i + HORIZONTAL_CHUNK_SIZE >= 0 && i + HORIZONTAL_CHUNK_SIZE < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + HORIZONTAL_CHUNK_SIZE] : null,
                                          (i - HORIZONTAL_CHUNK_SIZE >= 0 && i - HORIZONTAL_CHUNK_SIZE < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - HORIZONTAL_CHUNK_SIZE] : null,
                                          (i - 1 >= 0 && i - 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - 1] : null,
                                          null,
                                          (i + HORIZONTAL_CHUNK_SIZE - 1 >= 0 && i + HORIZONTAL_CHUNK_SIZE - 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + HORIZONTAL_CHUNK_SIZE - 1] : null,
                                          null,
                                          (i - HORIZONTAL_CHUNK_SIZE - 1 >= 0 && i - HORIZONTAL_CHUNK_SIZE - 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - HORIZONTAL_CHUNK_SIZE - 1] : null,
                                          null);
            }
            else
            {
                chunks[i].buildNeighbours((i + HORIZONTAL_CHUNK_SIZE >= 0 && i + HORIZONTAL_CHUNK_SIZE < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + HORIZONTAL_CHUNK_SIZE] : null,
                                          (i - HORIZONTAL_CHUNK_SIZE >= 0 && i - HORIZONTAL_CHUNK_SIZE < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - HORIZONTAL_CHUNK_SIZE] : null,
                                          (i - 1 >= 0 && i - 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - 1] : null,
                                          (i + 1 >= 0 && i + 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + 1] : null,
                                          (i + HORIZONTAL_CHUNK_SIZE - 1 >= 0 && i + HORIZONTAL_CHUNK_SIZE - 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + HORIZONTAL_CHUNK_SIZE - 1] : null,
                                          (i + HORIZONTAL_CHUNK_SIZE + 1 >= 0 && i + HORIZONTAL_CHUNK_SIZE + 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i + HORIZONTAL_CHUNK_SIZE + 1] : null,
                                          (i - HORIZONTAL_CHUNK_SIZE - 1 >= 0 && i - HORIZONTAL_CHUNK_SIZE - 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - HORIZONTAL_CHUNK_SIZE - 1] : null,
                                          (i - HORIZONTAL_CHUNK_SIZE + 1 >= 0 && i - HORIZONTAL_CHUNK_SIZE + 1 < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE)
                                          ? chunks[i - HORIZONTAL_CHUNK_SIZE + 1] : null);
            }
        }
    }

    public (int,int) getWorldBounds()
    {
        return (x_bound, y_bound);
    }

}
