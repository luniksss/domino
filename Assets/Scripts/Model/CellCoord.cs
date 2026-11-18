using UnityEngine;

[System.Serializable]
public class CellCoord
{
    public int x;
    public int y;

    public CellCoord(int x, int y) 
    { 
        this.x = x; 
        this.y = y; 
    }
}