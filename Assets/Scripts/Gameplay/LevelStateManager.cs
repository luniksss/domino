using UnityEngine;
using System.Collections.Generic;

public class LevelStateManager : MonoBehaviour
{
    public static LevelStateManager Instance { get; private set; }
    public int fieldWidth;
    public int fieldHeight;
    public int[,] field;
    public List<Wall> walls = new List<Wall>();
    public List<CellCoord> iceCells = new List<CellCoord>();
    public List<CellCoord> blockedCells = new List<CellCoord>();
    public List<Domino> foundDominoes = new List<Domino>();

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return; 
        }

        Instance = this;
    }

    public void StartNewLevel(int width, int height)
    {
        fieldWidth = width;
        fieldHeight = height;
        field = new int[width, height];
        walls.Clear();
        iceCells.Clear();
        blockedCells.Clear();
        foundDominoes.Clear();
    }

    public string SerializeToJson()
    {
        SaveData data = new SaveData();
        data.width = fieldWidth;
        data.height = fieldHeight;
        data.foundDominoes = foundDominoes;

        data.fieldFlat = new int[fieldWidth * fieldHeight];
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                data.fieldFlat[y * fieldWidth + x] = field[x, y];
            }
        }

        return JsonUtility.ToJson(data);
    }

    public void DeserializeFromJson(string json)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        fieldWidth = data.width;
        fieldHeight = data.height;
        foundDominoes = data.foundDominoes;

        field = new int[fieldWidth, fieldHeight];
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                field[x, y] = data.fieldFlat[y * fieldWidth + x];
            }
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public int width;
        public int height;
        public int[] fieldFlat;
        public List<Domino> foundDominoes;
    }
}