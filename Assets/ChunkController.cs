using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkController : MonoBehaviour
{
    public static int horizontalChunks = 4;
    public static int verticalChunks = 3;
    public static int CHUNK_SIZE_X = 49;
    public static int CHUNK_SIZE_Y = 27;
    public static int CAMERA_SIZE_OFFSET = 3;
    public static int TRIGGER_AURA_X = 24;//16:9 camera ratio, multiplied by 1.5 24
    public static int TRIGGER_AURA_Y = 14;
    private int currentChunkIndex = -1;
    private Chunk currentChunkLoaded = null;
    public GameObject character;
    public Tilemap aTilemap;

    private int save_x;
    private int save_y;
    private List<GameObject> gameObjectsLoaded;
    private List<Chunk> chunksLoaded;
    private Dictionary<int, GameObject> chunksLoadedDict;

    private List<Chunk> chunks;
    private List<World> worlds;
    private Dictionary<string, World> dictWorlds;

    private string currentWorld = "CrabIsland (Night)";
    private string currentDungeon = "";

    public int currentWorldID = -1;

    private World wRef;
    private Dungeon dRef;
    private SpriteController characterRef;
    // Start is called before the first frame update
    
    void Start()
    {
        //squareLen_x = aTilemap.cellBounds.size.x;
        //squareLen_y = aTilemap.cellBounds.size.y;
        save_x = -1;
        save_y = -1;
        worlds = new List<World>();
        gameObjectsLoaded = new List<GameObject>();
        chunksLoaded = new List<Chunk>();
        dictWorlds = new Dictionary<string, World>();
        chunksLoadedDict = new Dictionary<int, GameObject>();
        wRef = null;
        dRef = null;
        characterRef = character.GetComponent<SpriteController>();
        createWorlds();
        buildChunks();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        forceUpdate();
    }

    private void forceUpdate()
    {
        //Get the position of the character every update
        int char_x = (int)character.transform.position.x;
        int char_y = (int)character.transform.position.y;

        //Dont check for touch if char didnt move since last update
        if (!(char_x == save_x && char_y == save_y))
        {
            save_x = char_x;
            save_y = char_y;
            //fix positions ..x - (id)*horizontal*chunk_size
            wRef = getWorld(currentWorld);
            currentWorldID = wRef.id;
            dRef = null;
            string localPath = wRef.name;
            if(wRef.name.Equals("CrabIsland (Night)"))
            {
                localPath = "CrabIsland";
                //only used in loading chunks?
            }
            if (currentDungeon != "")
            {
                dRef = wRef.getDungeon(currentDungeon);
                localPath += "/" + dRef.name;
            }
            /*
            int fixed_x = char_x - wRef.offset_x;
            int fixed_y = char_y;
            if (dRef != null)
            {
                fixed_y = char_y - dRef.offset_y;
            }
            */
            int fixed_x = char_x;
            int fixed_y = char_y;
            //an invisible square aura around the player with len side of TRIGGER_AURA = 8, TRIGGER_AURA < CHUNK_SIZE
            (int, int) triggerTopLeft = (fixed_x - TRIGGER_AURA_X / 2, fixed_y + TRIGGER_AURA_Y / 2);
            (int, int) triggerTopRight = (fixed_x + TRIGGER_AURA_X / 2, fixed_y + TRIGGER_AURA_Y / 2);
            (int, int) triggerBottomLeft = (fixed_x - TRIGGER_AURA_X / 2, fixed_y - TRIGGER_AURA_Y / 2);
            (int, int) triggerBottomRight = (fixed_x + TRIGGER_AURA_X / 2, fixed_y - TRIGGER_AURA_Y / 2);


            if (currentChunkIndex == -1)//could be hardcoded for each world-spawn
            {//played first spawn check all chunks in current world
                //touch all chunks - not good performance
                foreach (Chunk c in ((dRef == null) ? getWorld(currentWorld).chunks : getWorld(currentWorld).getDungeon(currentDungeon).chunks))
                {
                    if (c.touch((fixed_x, fixed_y)))
                    {
                        //Announce entrance
                        //Debug.Log("Entering Chunk - " + c.getID() + " --- Leaving Chunk - " + currentChunkIndex);
                        currentChunkIndex = c.getID();
                        currentChunkLoaded = c;

                        // GameObject newChunk = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/" + localPath + "/Chunk" + currentChunkIndex)) as GameObject;
                        GameObject newChunk = Resources.Load("Prefabs/Worlds/" + localPath + "/Chunk" + currentChunkIndex) as GameObject;
                        //wor
                        Vector3 chunkParentPos = new Vector3();
                        chunkParentPos.z = 0;
                        if (dRef != null)
                        {
                            chunkParentPos.x = dRef.getDungeonBounds().Item1 + newChunk.transform.position.x;
                            chunkParentPos.y = dRef.getDungeonBounds().Item2 + newChunk.transform.position.y;
                        }
                        else
                        {
                            chunkParentPos.x = wRef.getWorldBounds().Item1 + newChunk.transform.position.x;
                            chunkParentPos.y = wRef.getWorldBounds().Item2 + newChunk.transform.position.y;
                        }
                        newChunk = Instantiate(newChunk);
                        newChunk.transform.position = chunkParentPos;
                        gameObjectsLoaded.Add(newChunk);
                        chunksLoadedDict.Add(currentChunkIndex, newChunk);
                        break;
                    }
                }
            }

            //reduce chunk touching if position within the older chunk
            if (currentChunkLoaded.touch((fixed_x, fixed_y))) // Player moved but still on the same chunk as before
            {
                //First unload all neighbours u dont touch anymore (assuming there are any)
                foreach (Chunk c in chunksLoaded)
                {
                    //Now we dont know where this chunk is on the world so we have to check all corners.
                    if (!(c.touch(triggerTopLeft) || c.touch(triggerTopRight) || c.touch(triggerBottomLeft) || c.touch(triggerBottomRight)))
                    {
                        DestroyImmediate(getChunkGameObject(c.getID()));
                        chunksLoadedDict.Remove(c.getID());
                    }
                }
                //check only for neighbours - //Announce entrance touching neighbour
                chunksLoaded.Clear();
                chunksLoaded.AddRange(touchOnlyNeighbours(triggerTopLeft, triggerBottomLeft, triggerTopRight, triggerBottomRight, currentChunkLoaded));
                if (chunksLoaded.Count > 0)
                {
                    //Debug.Log("");
                }
                foreach (Chunk c in chunksLoaded)
                {
                    if (!chunksLoadedDict.ContainsKey(c.getID()))//if its not already loaded as a gameobject
                    {
                        //GameObject newChunk = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/" + localPath + "/Chunk" + c.getID())) as GameObject;
                        GameObject newChunk = Resources.Load("Prefabs/Worlds/" + localPath + "/Chunk" + c.getID()) as GameObject;
                        Debug.Log(" Create : Prefabs / Worlds / " + localPath + " / Chunk" + c.getID());

                        //GameObject.Find("World")

                        Vector3 chunkParentPos = new Vector3();
                        chunkParentPos.z = 0;
                        if (dRef != null)
                        {
                            chunkParentPos.x = dRef.getDungeonBounds().Item1 + newChunk.transform.position.x;
                            chunkParentPos.y = dRef.getDungeonBounds().Item2 + newChunk.transform.position.y;
                        }
                        else
                        {
                            chunkParentPos.x = wRef.getWorldBounds().Item1 + newChunk.transform.position.x;
                            chunkParentPos.y = wRef.getWorldBounds().Item2 + newChunk.transform.position.y;
                        }
                        newChunk = Instantiate(newChunk);
                        newChunk.transform.position = chunkParentPos;
                        gameObjectsLoaded.Add(newChunk);
                        chunksLoadedDict.Add(c.getID(), newChunk);
                    }
                }

            }
            else //TELEPORTATION?
            {//played moved and also changed chunk
             // if (c.getID() != currentChunkIndex)
                Chunk lastChunk = null;
                //check neighbours which one player entered set it an the current chunk
                foreach (Chunk c in chunksLoaded)
                {
                    if (c.touch((fixed_x, fixed_y)))
                    {
                        //Announce entrance
                        //Debug.Log("Entering Chunk - " + c.getID() + " --- Leaving Chunk - " + currentChunkIndex);
                        lastChunk = currentChunkLoaded;
                        currentChunkIndex = c.getID();
                        currentChunkLoaded = c;
                        break;
                    }
                }
                if (lastChunk != null)
                {// always true? set the old chunk as a neighbour of the new chunk entered
                    if (touchOnlyNeighbours(triggerTopLeft, triggerBottomLeft, triggerTopRight, triggerBottomRight, currentChunkLoaded).Contains(lastChunk))
                    {
                        chunksLoaded.Add(lastChunk); // neighbours list
                    }
                }

            }
            /*
             * foreach (GameObject go in gameObjectsLoaded)
                {
                    DestroyImmediate(go);
                    //TO:DO USE LAST CHUNK AS NEIGHBOUR. DONT DESTROY
                    //REST IS THE SAME?

                }
             */
        }
        characterRef.World = currentWorld;
        characterRef.Dungeon = currentDungeon;
        characterRef.Chunk = currentChunkIndex;
        characterRef.nodeID = setToNodeID(currentWorld, currentDungeon, currentChunkIndex);
    }

    public int setToNodeID(string world, string dungeon, int chunk)
    {
        //give nodeIDs
        if (world == "CrabIsland" && dungeon == "")
        {
            return 0;
        }
        else if (world == "CrabIsland" && dungeon == "Dungeon1")
        {
            switch (chunk)
            {
                case 0:
                    return 2;
                case 1:
                    return 3;
                case 2:
                    return 18;
                case 3:
                    return 19;
                case 4:
                    return 4;
                case 5:
                    return 6;
                case 6:
                    return 20;
                case 7:
                    return -1;
                case 8:
                    return 5;
                case 9:
                    return -1;
                case 10:
                    return -1;
                case 11:
                    return -1;
            }
            return -1;
        }
        else if (world == "CrabIsland" && dungeon == "Dungeon2")
        {
            switch (chunk)
            {
                case 0:
                    return 1;
                case 1:
                    return 7;
                case 2:
                    return 8;
                case 3:
                    return 9;
                case 4:
                    return 10;
                case 5:
                    return 11;
                case 6:
                    return 12;
                case 7:
                    return 13;
                case 8:
                    return 14;
                case 9:
                    return 15;
                case 10:
                    return 16;
                case 11:
                    return 17;
            }
            return -1;
        }
        else if (world == "CrabIsland" && dungeon == "Dungeon3")
        {
            switch (chunk)
            {
                case 0:
                    return 21;
                case 1:
                    return 22;
                case 2:
                    return 23;
                case 3:
                    return 24;
                case 4:
                    return 25;
                case 5:
                    return 26;
                case 6:
                    return 27;
                case 7:
                    return 28;
                case 8:
                    return -1;
                case 9:
                    return -1;
                case 10:
                    return -1;
                case 11:
                    return -1;
            }
            return -1;
        }
        else if(world == "CrabIsland (Night)" && dungeon == "")
        {
            return 29;
        }
        else if (world == "CrabIsland (Night)" && dungeon == "Dungeon1")
        {
            switch (chunk)
            {
                case 0:
                    return 31;
                case 1:
                    return 32;
                case 2:
                    return 47;
                case 3:
                    return 48;
                case 4:
                    return 33;
                case 5:
                    return 35;
                case 6:
                    return 49;
                case 7:
                    return -1;
                case 8:
                    return 34;
                case 9:
                    return -1;
                case 10:
                    return -1;
                case 11:
                    return -1;
            }
            return -1;
        }
        else if (world == "CrabIsland (Night)" && dungeon == "Dungeon2")
        {
            switch (chunk)
            {
                case 0:
                    return 30;
                case 1:
                    return 36;
                case 2:
                    return 37;
                case 3:
                    return 38;
                case 4:
                    return 39;
                case 5:
                    return 40;
                case 6:
                    return 41;
                case 7:
                    return 42;
                case 8:
                    return 43;
                case 9:
                    return 44;
                case 10:
                    return 45;
                case 11:
                    return 46;
            }
            return -1;
        }
        else if (world == "CrabIsland (Night)" && dungeon == "Dungeon3")
        {
            switch (chunk)
            {
                case 0:
                    return 50;
                case 1:
                    return 51;
                case 2:
                    return 52;
                case 3:
                    return 53;
                case 4:
                    return 54;
                case 5:
                    return 55;
                case 6:
                    return 56;
                case 7:
                    return 57;
                case 8:
                    return -1;
                case 9:
                    return -1;
                case 10:
                    return -1;
                case 11:
                    return -1;
            }
            return -1;
        }
        else
        {
            return -1;
        }
    }

    public void warp(string world, string dungeon)
    {
        currentChunkIndex = -1;
        currentChunkLoaded = null;
        chunksLoaded.Clear();
        chunksLoadedDict.Clear();
        foreach (GameObject go in gameObjectsLoaded)
        {
            Destroy(go);
        }
        gameObjectsLoaded.Clear();
        currentWorld = world;
        currentDungeon = dungeon;

        forceUpdate();

        /*
         * For pathfinding 
         */
        characterRef.World = currentWorld;
        characterRef.Dungeon = currentDungeon;
        characterRef.Chunk = currentChunkIndex;
        //characterRef.nodeID = setToNodeID(currentWorld, currentDungeon, currentChunkIndex);
    }

    private void createWorlds()
    {
        //Imagine a grid of 1x1 worlds to create offsets
        worlds.Add(new World("CrabIsland", 0,4,3,0,0, "CrabIsland (Night)"));
        dictWorlds.Add("CrabIsland", worlds[0]);
        worlds[0].addDungeon(new Dungeon("Dungeon1", 0, 4, 3,0,1)); //81
        worlds[0].addDungeon(new Dungeon("Dungeon2", 1, 4, 3, 0, 2));
        worlds[0].addDungeon(new Dungeon("Dungeon3", 2, 4, 3, 0, 3));
        worlds.Add(new World("GeminiIsland", 1, 4, 3,1,0, null)); //196
        worlds.Add(null);
        worlds.Add(new World("CrabIsland (Night)", 3, 4, 3, 3, 0, "CrabIsland")); //588
        worlds[3].addDungeon(new Dungeon("Dungeon1", 0, 4, 3, 3, 1));
        worlds[3].addDungeon(new Dungeon("Dungeon2", 1, 4, 3, 3, 2));
        worlds[3].addDungeon(new Dungeon("Dungeon3", 2, 4, 3, 3, 3));
        dictWorlds.Add("CrabIsland (Night)", worlds[3]);
        dictWorlds.Add("GeminiIsland", worlds[1]);
    }

    private void buildChunks()
    {
        (int, int)[] tuppleArray = new (int, int)[4];
        foreach (World w in worlds)
        {//add dungeon functionality
            if(w != null)
            {
                int squareLen_x = w.HORIZONTAL_CHUNK_SIZE * CHUNK_SIZE_X;
                int squareLen_y = w.VERTICAL_CHUNK_SIZE * CHUNK_SIZE_Y;
                int ttx = Mathf.CeilToInt(squareLen_x / (float)CHUNK_SIZE_X);
                int tty = Mathf.CeilToInt(squareLen_y / (float)CHUNK_SIZE_Y);

                chunks = new List<Chunk>();
                (int, int) bounds = w.getWorldBounds(); // this was later added to accomodate the new bitcoin prefab system
                for (int n = 1; n <= tty; n++)
                {
                    for (int k = 1; k <= ttx; k++)
                    {
                        int counter = 0;
                        for (int x = (k - 1) * CHUNK_SIZE_X; x <= k * CHUNK_SIZE_X; x = x + CHUNK_SIZE_X)
                        {
                            for (int y = (n - 1) * CHUNK_SIZE_Y; y <= n * CHUNK_SIZE_Y; y = y + CHUNK_SIZE_Y)
                            {
                                tuppleArray[counter++] = (x + bounds.Item1, y + bounds.Item2);
                            }
                        }
                        Chunk c = new Chunk((n - 1) * (tty + 1) + k - 1,
                        tuppleArray[1], tuppleArray[3], tuppleArray[0], tuppleArray[2]);
                        chunks.Add(c);
                    }

                }
                w.loadChunk(chunks);

                foreach (Dungeon d in w.dungeons)
                {
                    squareLen_x = d.HORIZONTAL_CHUNK_SIZE * CHUNK_SIZE_X;
                    squareLen_y = d.VERTICAL_CHUNK_SIZE * CHUNK_SIZE_Y;
                    ttx = Mathf.CeilToInt(squareLen_x / (float)CHUNK_SIZE_X);
                    tty = Mathf.CeilToInt(squareLen_y / (float)CHUNK_SIZE_Y);

                    chunks = new List<Chunk>();
                    bounds = d.getDungeonBounds(); // this was later added to accomodate the new bitcoin prefab system
                    for (int n = 1; n <= tty; n++)
                    {
                        for (int k = 1; k <= ttx; k++)
                        {
                            int counter = 0;
                            for (int x = (k - 1) * CHUNK_SIZE_X; x <= k * CHUNK_SIZE_X; x = x + CHUNK_SIZE_X)
                            {
                                for (int y = (n - 1) * CHUNK_SIZE_Y; y <= n * CHUNK_SIZE_Y; y = y + CHUNK_SIZE_Y)
                                {
                                    tuppleArray[counter++] = (x + bounds.Item1, y + bounds.Item2);
                                }
                            }
                            Chunk c = new Chunk((n - 1) * (tty + 1) + k - 1,
                            tuppleArray[1], tuppleArray[3], tuppleArray[0], tuppleArray[2]);
                            chunks.Add(c);
                        }

                    }
                    d.loadChunk(chunks);
                }
            }
            
        }
        
    }

    private bool touchIfNotNull(Chunk c, (int,int) tupple)
    {
        if(c != null)
        {
            return c.touch(tupple);
        }
        else
        {
            return false;
        }
    }

    private HashSet<Chunk> touchOnlyNeighbours((int, int) triggerTopLeft, (int, int) triggerBottomLeft, (int, int) triggerTopRight, (int, int) triggerBottomRight, Chunk currentChunkLoaded)
    {
        HashSet<Chunk> touched = new HashSet<Chunk>();
        if (touchIfNotNull(currentChunkLoaded.left, triggerTopLeft))
        {
            touched.Add(currentChunkLoaded.left);
            //Debug.Log("1Neighouur Chunk in range - " + currentChunkLoaded.left.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.topLeft, triggerTopLeft))
        {
            touched.Add(currentChunkLoaded.topLeft);
            //Debug.Log("2Neighouur Chunk in range - " + currentChunkLoaded.topLeft.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.top, triggerTopLeft))
        {
            touched.Add(currentChunkLoaded.top);
            //Debug.Log("3Neighouur Chunk in range - " + currentChunkLoaded.top.getID());
        }

        if (touchIfNotNull(currentChunkLoaded.left, triggerBottomLeft))
        {
            touched.Add(currentChunkLoaded.left);
            //Debug.Log("4Neighouur Chunk in range - " + currentChunkLoaded.left.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.bottomLeft, triggerBottomLeft))
        {
            touched.Add(currentChunkLoaded.bottomLeft);
            //Debug.Log("5Neighouur Chunk in range - " + currentChunkLoaded.bottomLeft.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.bottom, triggerBottomLeft))
        {
            touched.Add(currentChunkLoaded.bottom);
            //Debug.Log("6Neighouur Chunk in range - " + currentChunkLoaded.bottom.getID());
        }

        if (touchIfNotNull(currentChunkLoaded.top, triggerTopRight))
        {
            touched.Add(currentChunkLoaded.top);
            //Debug.Log("7Neighouur Chunk in range - " + currentChunkLoaded.top.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.topRight, triggerTopRight))
        {
            touched.Add(currentChunkLoaded.topRight);
            //Debug.Log("8Neighouur Chunk in range - " + currentChunkLoaded.topRight.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.right, triggerTopRight))
        {
            touched.Add(currentChunkLoaded.right);
            //Debug.Log("9Neighouur Chunk in range - " + currentChunkLoaded.right.getID());
        }

        if (touchIfNotNull(currentChunkLoaded.right, triggerBottomRight))
        {
            touched.Add(currentChunkLoaded.right);
            //Debug.Log("10Neighouur Chunk in range - " + currentChunkLoaded.right.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.bottomRight, triggerBottomRight))
        {
            touched.Add(currentChunkLoaded.bottomRight);
            //Debug.Log("11Neighouur Chunk in range - " + currentChunkLoaded.bottomRight.getID());
        }
        if (touchIfNotNull(currentChunkLoaded.bottom, triggerBottomRight))
        {
            touched.Add(currentChunkLoaded.bottom);
            //Debug.Log("12Neighouur Chunk in range - " + currentChunkLoaded.bottom.getID());
        }
        return touched;
    }

    public World getWorld(string worldName)
    {
        World tempWorld;
        //WEBGL adds an invisible-unprintable character at the end of the requested string
        //So trimming the end fixed this error.
        if (!dictWorlds.TryGetValue(worldName.Trim(), out tempWorld))
        {
            Debug.LogError("The Provided world `" + worldName.Trim() + "` does not exist!");
            return null;
        }
        return tempWorld;
    }

    public World getWorldByID(int worldID)
    {
        World tempWorld = null;
        if (worldID < worlds.Count && worldID>=0) return worlds[worldID];
        return tempWorld;
    }

    public World getCurrentWorld()
    {
        return wRef;
    }

    public Dungeon getCurrentDungeon()
    {
        return dRef;
    }

    public GameObject getChunkGameObject(int chunkID)
    {
        GameObject tempChunk;
        if (!chunksLoadedDict.TryGetValue(chunkID, out tempChunk))
        {
            Debug.LogError("The Provided chunk ID `" + chunkID + "` does not exist!");
            return null;
        }
        return tempChunk;
    }

    public (int, int) getCurrentWorldBounds()
    {
        if (dRef != null)
        {
            return dRef.getDungeonBounds();
        }
        else if (wRef != null)
        {
            return wRef.getWorldBounds();
        }
        else
        {
            return (0, 0); // default camera offset
        }
    }

    /*
     * Pathfinding methods
     */
     public void locateBitcoin(Bitcoin bitcoin)
     {
        //Find World, dungeon and chunkID of this bitcoin.
        bitcoin.World = "Void";
        bitcoin.Chunk = -1;
        bitcoin.Dungeon = "Void";
        foreach (World w in worlds)
        {
            if (w != null)
            {
                foreach (Chunk c in w.chunks)
                {
                    if (c.touch((Convert.ToInt32(bitcoin.Coordinates.Item1), Convert.ToInt32(bitcoin.Coordinates.Item2))))
                    {
                        bitcoin.World = w.name;
                        bitcoin.Chunk = c.getID();
                        bitcoin.Dungeon = "";
                        goto outerloop;
                    }
                }
                foreach (Dungeon d in w.dungeons)
                {
                    foreach (Chunk c in d.chunks)
                    {
                        if (c.touch((Convert.ToInt32(bitcoin.Coordinates.Item1), Convert.ToInt32(bitcoin.Coordinates.Item2))))
                        {
                            bitcoin.World = w.name;
                            bitcoin.Chunk = c.getID();
                            bitcoin.Dungeon = d.name;
                            goto outerloop;
                        }
                    }
                }
            }
            outerloop:
            if (bitcoin.Chunk != -1)
                bitcoin.Extras_nodeID = setToNodeID(bitcoin.World, bitcoin.Dungeon, bitcoin.Chunk);
            else
                bitcoin.Extras_nodeID = -1;
        }  
    }

    public void locateGate(Portal.Gate bitcoin)
    {
        //Find World, dungeon and chunkID of this bitcoin.
        bitcoin.world = "Void";
        bitcoin.chunkID = -1;
        bitcoin.dungeon = "Void";
        foreach (World w in worlds)
        {
            if(w != null)
            {
                foreach (Chunk c in w.chunks)
                {
                    if (c.touch((Convert.ToInt32(bitcoin.doormat.position.x), Convert.ToInt32(bitcoin.doormat.position.y))))
                    {
                        bitcoin.world = w.name;
                        bitcoin.chunkID = c.getID();
                        bitcoin.dungeon = "";
                        goto outerloop;
                    }
                }
                foreach (Dungeon d in w.dungeons)
                {
                    foreach (Chunk c in d.chunks)
                    {
                        if (c.touch((Convert.ToInt32(bitcoin.doormat.position.x), Convert.ToInt32(bitcoin.doormat.position.y))))
                        {
                            bitcoin.world = w.name;
                            bitcoin.chunkID = c.getID();
                            bitcoin.dungeon = d.name;
                            goto outerloop;
                        }
                    }
                }
            }
            outerloop:
            if (bitcoin.chunkID != -1)
                bitcoin.nodeID = setToNodeID(bitcoin.world, bitcoin.dungeon, bitcoin.chunkID);
            else
                bitcoin.nodeID = -1;
        }
               
    }
}
