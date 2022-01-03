
using System.Collections.Generic;

public class Dungeon
{
    public string name;
    public int id;
    public int HORIZONTAL_CHUNK_SIZE;
    public int VERTICAL_CHUNK_SIZE;
    public List<Chunk> chunks;
    private int x_bound;
    private int y_bound; //bound -> the bottom left corner pos
    //public int offset_y;

    public Dungeon(string name, int id, int horizontal, int vertical, int x_offset, int y_offset)
    {
        this.name = name;
        this.id = id;
        HORIZONTAL_CHUNK_SIZE = horizontal;
        VERTICAL_CHUNK_SIZE = vertical;
        x_bound = x_offset * HORIZONTAL_CHUNK_SIZE * 49; //look at chunkcontroller.cs
        y_bound = y_offset * VERTICAL_CHUNK_SIZE * 27; //look at chunkcontroller.cs
        //this.offset_y = offset_y;
        chunks = new List<Chunk>();
    }

    public void loadChunk(List<Chunk> chunks)
    {
        this.chunks = chunks;
        for (int i = 0; i < HORIZONTAL_CHUNK_SIZE * VERTICAL_CHUNK_SIZE; i++)
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
            else if ((i % HORIZONTAL_CHUNK_SIZE) == HORIZONTAL_CHUNK_SIZE - 1)//Left chunks
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

    public (int, int) getDungeonBounds()
    {
        return (x_bound, y_bound);
    }
}
