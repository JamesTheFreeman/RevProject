using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public GameObject grid;                 // Grid
    public GameObject sGrid;                // Sub-Grid
    public GameObject mGrid;                // Micro-Grid
    public GameObject room;                 // Room
    public GameObject walls;                // Room walls
    public GameObject bridge;               // Bridge/door between rooms
    public int mapSize = 50;                // Size of the map (>= 500)
    public int RNGStartVal = 150;           // Value of % chance of wall at start
    public int RNGIncrmnt = 2;              // % chance increase of bridge per non-bridge
    private int[][] coords;                 // Coordinate value array   
    private RoomObject[] rms;               // Room class array

    // Start is called before the first frame update
    void Start()
    {
        coords = new int[1000][];           // Initialize x-section of array
        for (int i = 0; i < 1000; i ++)     // Initialize y-section of array
            coords[i] = new int[1000];
        rms = new RoomObject[500];

        genMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void genMap()
    {
        int x = 500;
        int y = 500;

        makeRoom(x, y, 1); // First room

        for (int i = 2; i <= mapSize; i++)
        {
            // Variables
            int lastID          = -5;
            int lastX           = -5;
            int lastY           = -5;
            int lastDirection   = -5;
            int direction       = -5;
            int wallRNG         = RNGStartVal;

            x = 500;
            y = 500;

            while (true)
            {
                lastDirection = direction;

                // Selects next direction to travel
                while (true)
                {
                    do {direction = Random.Range(0, 4);}
                    // Makes sure you aren't backtracking
                    while (direction - 2 == lastDirection || direction + 2 == lastDirection);
                    // Checks that path isn't blocked
                    if (rms[coords[x][y]].adj[direction] >= 0) break;
                }

                // Update variables
                lastX = x;
                lastY = y;
                lastID = rms[coords[lastX][lastY]].id;

                // Adjust x & y values
                x = moveXY(x, y, direction)[0];
                y = moveXY(x, y, direction)[1];

                // Bridge chance increases
                wallRNG -= RNGIncrmnt;

                // Empty space found
                if (coords[x][y] == 0)
                {
                    // Make room/bridge
                    makeRoom(x, y, i);
                    genBridge((float)(x + lastX) / (float)2, (float)(y + lastY) / (float)2);

                    // Set IDs in adjacency arrays for bridge
                    rms[i].adj[inverseDirection(direction)] = lastID;
                    rms[lastID].adj[direction] = i;

                    // Next i value
                    break;
                }
                // Gap between rooms found
                else if (rms[lastID].adj[direction] == 0)
                {
                    int rnd = Random.Range(0, 101);

                    // wallRNG = % chance of a wall
                    if (rnd <= wallRNG)
                    {
                        /* BREAKS THE DAMN GAME
                        // Blocks path
                        rms[coords[x][y]].adj[inverseDirection(direction)] = -5;
                        rms[lastID].adj[direction] = -5;
                        */
                        
                        continue;
                    }
                    else
                    {
                        // Bridges path
                        genBridge((float)(x + lastX) / (float)2, (float)(y + lastY) / (float)2);
                        rms[coords[x][y]].adj[inverseDirection(direction)] = lastID;
                        rms[lastID].adj[direction] = coords[x][y];
                        
                        continue;
                    }
                    /*
                    // Restart
                    i -= 1;
                    break;
                    */
                }
                // Nothing found, carry on
                else continue;
            }
        }
    }

    private int inverseDirection(int dir)
    {
        switch(dir)
        {
            case 0:
                return 2;
            case 1:
                return 3;
            case 2:
                return 0;
            case 3:
                return 1;
            default:
                return -5;
        }
    }
    private int[] moveXY(int x, int y, int dir)
    {
        int[] xy = new int[2];
        switch(dir)
        {
            case 0:
                xy[0] = x;
                xy[1] = y + 1;
                break;
            case 1:
                xy[0] = x + 1;
                xy[1] = y;
                break;
            case 2:
                xy[0] = x;
                xy[1] = y - 1;
                break;
            case 3:
                xy[0] = x - 1;
                xy[1] = y;
                break;
        }
        return xy;
    }

    private void makeRoom(int x, int y, int id)
    {
        // Info inserted into arrays
        rms[id] = new RoomObject(x, y, id);
        coords[x][y] = id;

        // Create room in game
        GameObject rm = Instantiate(room);
        rm.transform.SetParent(grid.transform);
        rm.transform.position = new Vector3(x, y, 1);

        // Create walls for room in game
        GameObject wll = Instantiate(walls);
        wll.transform.SetParent(sGrid.transform);
        wll.transform.position = new Vector3(x*1, y*1, 0);
        wll.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    private void genBridge(float x, float y)
    {
        GameObject brge = Instantiate(bridge);
        brge.transform.SetParent(mGrid.transform);
        brge.transform.position = new Vector3(x, y, -2);
        brge.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    // Checks for bridge in given direction
    public bool checkBridge(int x, int y, int dir)
    {
        int check = rms[coords[x][y]].adj[dir];
        if (check > 0) return true;
        else return false;
    }
}