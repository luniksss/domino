using UnityEngine;

[System.Serializable]
public class Domino
{
    public CellCoord startCell;
    public CellCoord endCell;
    public int valueA;
    public int valueB;

    public Domino(CellCoord start, CellCoord end, int valA, int valB)
    {
        startCell = start;
        endCell = end;
        valueA = valA;
        valueB = valB;
    }
}
