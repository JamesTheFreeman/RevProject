using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject
{
    private int x;              // X-coord
    private int y;              // Y-coord
    private int id;             // This room ID
    private int[] adj;          // Adjacent room IDs | 0,1,2,3 -> N,E,S,W

    public RoomObject(int xV, int yV, int idV)
    {
        x = xV;
        y = yV;
        id = idV;
        adj = new int[4];
    }

    public void setN(int idv) {adj[0] = idv;}
    public void setE(int idv) {adj[1] = idv;}
    public void setS(int idv) {adj[2] = idv;}
    public void setW(int idv) {adj[3] = idv;}

    public int getN() {return adj[0];}
    public int getE() {return adj[1];}
    public int getS() {return adj[2];}
    public int getW() {return adj[3];}
}
