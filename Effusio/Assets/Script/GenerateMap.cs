using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GenerateMap : MonoBehaviour
{
    public TileBase wallTile;
    public TileBase wallMiddleTile;
    public TileBase wallLeftTile;
    public TileBase wallRightTile;
    public TileBase groundSoloTile;
    public TileBase groundMiddleTile;
    public TileBase groundLeftTile;
    public TileBase groundRightTile;
    public Tilemap GroundTilemap;
    public GameObject potalObj;
    public int width;
    public int height;
    public List<Pair> loc = new List<Pair>();
    float chanceToStartAlive = 0.45f;
    // Start is called before the first frame update
    void Start()
    {
        width = 110;
        height = 20;
        initialiseMap();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void initialiseMap()
    {
        bool[,] cellmap = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float rand = Random.Range(0, 1f);
                if (rand < chanceToStartAlive)
                {
                    cellmap[x, y] = true;
                }
            }

        }

        for (int i = 0; i < 6; i++)
        {
            cellmap = doSimulationStep(cellmap, width, height);
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j == 0 || j == width - 1 || i == 0 || i == height - 1)
                    cellmap[j, i] = true;
            }
        }


        for (int i = 0; i < 6; i++)
        {
            cellmap = doSimulationStep(cellmap, width, height);
        }

        cellmap[1, 1] = false;
        cellmap[1, 2] = false;
        cellmap[1, 3] = false;
        cellmap[2, 1] = false;
        cellmap[2, 2] = false;
        cellmap[2, 3] = false;
        cellmap[3, 1] = false;
        cellmap[3, 2] = false;
        cellmap[3, 3] = false;

        int[,] room = divideRoom(cellmap, width, height, 1);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (room[j, i] == -1)
                {
                    cellmap[j, i] = true;
                }
                else
                {
                    cellmap[j, i] = false;
                }
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j == 0 || j == width - 1 || i == 0 || i == height - 1)
                    cellmap[j, i] = true;
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (cellmap[j, i])
                {
                    if (i < height - 1 && !cellmap[j, i + 1])
                    {
                        if (j < width - 1 && j > 0 && !cellmap[j + 1, i] && !cellmap[j - 1, i])
                            GroundTilemap.SetTile(new Vector3Int(j, i, 1), groundSoloTile);
                        else if (j < width - 1 && j > 0 && !cellmap[j + 1, i])
                            GroundTilemap.SetTile(new Vector3Int(j, i, 1), groundRightTile);
                        else if (j < width - 1 && j > 0 && !cellmap[j - 1, i])
                            GroundTilemap.SetTile(new Vector3Int(j, i, 1), groundLeftTile);
                        else
                            GroundTilemap.SetTile(new Vector3Int(j, i, 0), groundMiddleTile);
                    }
                    else
                    {
                        if (i > 0 && !cellmap[j, i - 1] && j > 0 && j < width - 1 && !cellmap[j + 1, i] && !cellmap[j - 1, i])
                            GroundTilemap.SetTile(new Vector3Int(j, i, 1), wallMiddleTile);
                        else if (i > 0 && !cellmap[j, i - 1] && j > 0 && j < width - 1 && !cellmap[j + 1, i])
                            GroundTilemap.SetTile(new Vector3Int(j, i, 1), wallRightTile);
                        else if (i > 0 && !cellmap[j, i - 1] && j > 0 && j < width - 1 && !cellmap[j - 1, i])
                            GroundTilemap.SetTile(new Vector3Int(j, i, 1), wallLeftTile);
                        else
                            GroundTilemap.SetTile(new Vector3Int(j, i, 1), wallTile);
                    }
                }
                else if (i < height - 1 && j < width - 1 && (!cellmap[j + 1, i] && !cellmap[j, i + 1] && !cellmap[j + 1, i + 1]))
                {
                    Pair pair = new Pair(j, i);
                    float rand = Random.Range(0, 20f);
                    if (rand < chanceToStartAlive && j > 10)
                    {
                        loc.Add(pair);
                    }
                }
            }
        }

        bool potal = true;
        for (int i = 0; i < height; i++)
        {
            for (int j = width - 1; j > 0; j--)
            {
                if (cellmap[j, i])
                {
                    //벽일시 넘어감
                }
                else if (potal)
                {
                    potalObj.transform.position = new Vector3Int(j - 1, i + 1, 3);
                    potal = false;
                    break;
                }
                else
                {
                }
            }
        }
        GameObject[] monsterClone = Resources.LoadAll<GameObject>("Enemy/Prefabs/" + SceneManager.GetActiveScene().name);

        for (int i = 0, k = 0; i < loc.Count; i++, k++)
        {
            if (k == 3) { k = 0; }
            Vector3 summonLoc = new Vector3(loc[i].x, loc[i].y, 0);
            
            monsterClone[k].tag = "Enemy";
            Instantiate(monsterClone[k], summonLoc, Quaternion.identity);
            
        }
    }

    public bool[,] doSimulationStep(bool[,] oldMap, int width, int height)
    {
        bool[,] newMap = new bool[width, height];
        //Loop over each row and column of the map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int nbs = countAliveNeighbours(oldMap, x, y, width, height);
                //The new value is based on our simulation rules
                //First, if a cell is alive but has too few neighbours, kill it.
                if (oldMap[x, y])
                {
                    if (nbs < 4)
                    {
                        newMap[x, y] = false;
                    }
                    else
                    {
                        newMap[x, y] = true;
                    }
                } //Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
                else
                {
                    if (nbs > 4)
                    {
                        newMap[x, y] = true;
                    }
                    else
                    {
                        newMap[x, y] = false;
                    }
                }
            }
        }
        return newMap;
    }

    public int countAliveNeighbours(bool[,] map, int x, int y, int width, int height)
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbour_x = x + i;
                int neighbour_y = y + j;
                //If we're looking at the middle point
                if (i == 0 && j == 0)
                {
                    //Do nothing, we don't want to add ourselves in!
                }
                //In case the index we're looking at it off the edge of the map
                else if (neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= width || neighbour_y >= height)
                {
                    count = count + 1;
                }
                //Otherwise, a normal check of the neighbour
                else if (map[neighbour_x, neighbour_y])
                {
                    count = count + 1;
                }
            }
        }
        return count;
    }

    public int[,] divideRoom(bool[,] originMap, int width, int height, int cnt)
    {
        int[,] room = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (originMap[i, j])
                    room[i, j] = -1;
                else
                    room[i, j] = 0;
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (room[i, j] == 0)
                {
                    setRoom(room, cnt, i, j);
                    cnt++;
                }
            }
        }

        if (cnt > 1)
        {
            tunnel(room, cnt - 1, width, height);
        }
        return room;
    }

    public void setRoom(int[,] room, int cnt, int cellX, int cellY)
    {
        if (room[cellX, cellY] == 0)
        {
            room[cellX, cellY] = cnt;
            setRoom(room, cnt, cellX, cellY - 1);
            setRoom(room, cnt, cellX - 1, cellY);
            setRoom(room, cnt, cellX, cellY + 1);
            setRoom(room, cnt, cellX + 1, cellY);
        }
    }

    public void tunnel(int[,] room, int roomNum, int width, int height)
    {
        int[,] roomLoc = new int[2, roomNum];
        int[] roomArea = new int[roomNum];
        for (int o = 0; o < roomNum; o++)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (room[j, i] == o + 1)
                    {
                        roomArea[o]++;
                    }
                }
            }
        }

        int temp = 0;
        int k = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (room[j, i] == k + 1)
                {
                    temp++;
                }
                if (k < roomNum && temp == (roomArea[k] / 2))
                {
                    roomLoc[0, k] = i;
                    roomLoc[1, k] = j;
                    temp = 0;
                    i = 0;
                    j = 0;
                    k++;
                }
            }
        }

        for (int i = 0; i < roomNum - 1; i++)
        {
            while (roomLoc[1, i] != roomLoc[1, i + 1])
            {
                if (roomLoc[1, i] >= roomLoc[1, i + 1])
                {
                    room[roomLoc[1, i] - 1, roomLoc[0, i]] = 0;
                    roomLoc[1, i]--;
                }
                else
                {
                    room[roomLoc[1, i] + 1, roomLoc[0, i]] = 0;
                    roomLoc[1, i]++;
                }
            }
            while (roomLoc[0, i] != roomLoc[0, i + 1])
            {
                if (roomLoc[0, i] >= roomLoc[0, i + 1])
                {
                    room[roomLoc[1, i], roomLoc[0, i] - 1] = 0;
                    roomLoc[0, i]--;
                }
                else
                {
                    room[roomLoc[1, i], roomLoc[0, i] + 1] = 0;
                    roomLoc[0, i]++;
                }
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = width - 1; j > 0; j--)
            {
                if (room[j, i] == roomNum)
                {
                    room[j, i] = 9;
                    return;
                }
            }
        }
    }
}
