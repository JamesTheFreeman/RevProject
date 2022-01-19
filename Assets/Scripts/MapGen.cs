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
    public int mapSize = 50;                // Size of the map
    private int[][] coords;                 // Coordinate value array
    private RoomObject[] rms;               // Room class array

    // Start is called before the first frame update
    void Start()
    {
        coords = new int[1000][];          // Initialize x-section of array
        for (int i = 0; i < 1000; i ++)      // Initialize y-section of array
            coords[i] = new int[1000];
        rms = new RoomObject[100];

        genMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void genMap()
    {
       for (int i = 1; i <= mapSize; i++)
       {
           // Tracking variables for room generation
           int X = 100;
           int Y = 100;
           int lastR = 999;
           int lastX = 999;
           int lastY = 999;
           do
           {
               if (coords[X][Y] == 0)
               {
                    makeRoom(X, Y, i);
                    if(lastR != 999) makeBridge(lastX, lastY, X, Y, lastR);
                    break;
               }
               else if (lastR != 999)
               {
                    if (Random.Range(0, 5) > 0)
                    {
                        burnBridges(lastX, lastY, X, Y, lastR);
                        i -= 1;
                        break;
                    }
                    bool newBridge = makeBridge(lastX, lastY, X, Y, lastR);
                    if (newBridge)
                    {
                        i -= 1;
                        break;
                    }
               }
               int r; // NESW value
               do
               {
                    r = Random.Range(0, 4); // Random int 0-3 for NESW
                    if (lastR - 2 != r && lastR + 2 != r)
                    {
                        switch(r)
                        {
                            case 0:
                                lastR = 2;
                                break;
                            case 1:
                                lastR = 3;
                                break;
                            case 2:
                                lastR = 0;
                                break;
                            case 3:
                                lastR = 1;
                                break;
                        }
                    }
                    else continue;
               } 
               while (false);
               switch(r)
               {
                    case 0:
                        lastX = X;
                        lastY = Y;
                        X += 1;
                        break;
                    case 1:
                        lastX = X;
                        lastY = Y;
                        Y += 1;
                        break;
                    case 2:
                        lastX = X;
                        lastY = Y;
                        X -= 1;
                        break;
                    case 3:
                        lastX = X;
                        lastY = Y;
                        Y -= 1;
                        break;
               }
           } 
           while(true);
       }
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

    private bool makeBridge(int lx, int ly, int x, int y, int lr)
    {
        int A = coords[lx][ly];
        int B = coords[x][y];
        switch(lr)
        {
            case 0:
                if (rms[A].getS() != 0) return false;
                rms[A].setS(B);
                rms[B].setN(A);
                break;
            case 1:
                if (rms[A].getW() != 0) return false;
                rms[A].setW(B);
                rms[B].setE(A);
                break;
            case 2:
                if (rms[A].getN() != 0) return false;
                rms[A].setN(B);
                rms[B].setS(A);
                break;
            case 3:
                if (rms[A].getE() != 0) return false;
                rms[A].setE(B);
                rms[B].setW(A);
                break;
        }
        genBridge((float)(x + lx) / (float)2, (float)(y + ly) / (float)2);
        return true;
    }

    private void genBridge(float x, float y)
    {
        GameObject brge = Instantiate(bridge);
        brge.transform.SetParent(mGrid.transform);
        brge.transform.position = new Vector3(x, y, -2);
        brge.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    // Blocks bridge slot
    private void burnBridges(int lx, int ly, int x, int y, int lr)
    {
        int A = coords[lx][ly];
        int B = coords[x][y];
        switch(lr)
        {
            case 0:
                if (rms[A].getS() != 0) return;
                rms[A].setS(5);
                rms[B].setN(5);
                break;
            case 1:
                if (rms[A].getW() != 0) return;
                rms[A].setW(5);
                rms[B].setE(5);
                break;
            case 2:
                if (rms[A].getN() != 0) return;
                rms[A].setN(5);
                rms[B].setS(5);
                break;
            case 3:
                if (rms[A].getE() != 0) return;
                rms[A].setE(5);
                rms[B].setW(5);
                break;
        }
    }
}
