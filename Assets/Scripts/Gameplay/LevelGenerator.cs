using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public void GenerateField(LevelStateManager state)
    {
        int sizeIndex = SaveManager.Instance.Data.currentFieldSizeIndex;
        var config = LevelConfig.FieldSizeBySizeIndex(sizeIndex);
        int width = config.width;
        int height = config.height;

        Debug.Log($"<color=cyan>Генерация поля {config.displayName}: Ширина {width}, Высота {height}</color>");

        state.StartNewLevel(width, height);

        List<List<CellCoord>> pairs = GenerateRandomPairs(width, height);
        List<Domino> availableDominoes = GetAvailableDominoes(width * height / 2);
        ShuffleList(availableDominoes);

        state.solutionPairs = new List<List<CellCoord>>(pairs);

        string solutionLog = "<color=yellow>=== РЕШЕНИЕ УРОВНЯ ===</color>\n";

        for (int i = 0; i < pairs.Count; i++)
        {
            var pair = pairs[i];
            var domino = availableDominoes[i];

            state.field[pair[0].x, pair[0].y] = domino.valueA;
            state.field[pair[1].x, pair[1].y] = domino.valueB;

            solutionLog += $"Кость {domino.valueA}:{domino.valueB} -> " +
                           $"({pair[0].x},{pair[0].y}) и ({pair[1].x},{pair[1].y})\n";
        }

        Debug.Log(solutionLog);
    }

    private List<List<CellCoord>> GenerateRandomPairs(int width, int height)
    {
        List<List<CellCoord>> pairs = new List<List<CellCoord>>();
        bool[,] visited = new bool[width, height];

        List<CellCoord> allCells = new List<CellCoord>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                allCells.Add(new CellCoord(x, y));
            }
        }

        ShuffleList(allCells);

        foreach (var cell in allCells)
        {
            if (visited[cell.x, cell.y])
            {
                continue;
            }

            if (FindPairForCell(cell, width, height, visited, pairs))
            {
                continue;
            }
            else
            {
                return GenerateRandomPairs(width, height);
            }
        }

        return pairs;
    }

    private bool FindPairForCell(CellCoord cell, int width, int height, bool[,] visited, List<List<CellCoord>> pairs)
    {
        List<CellCoord> neighbors = new List<CellCoord>();

        if (cell.x + 1 < width && !visited[cell.x + 1, cell.y])
        {
            neighbors.Add(new CellCoord(cell.x + 1, cell.y));
        }
        if (cell.x - 1 >= 0 && !visited[cell.x - 1, cell.y])
        {
            neighbors.Add(new CellCoord(cell.x - 1, cell.y));
        }
        if (cell.y + 1 < height && !visited[cell.x, cell.y + 1])
        {
            neighbors.Add(new CellCoord(cell.x, cell.y + 1));
        }
        if (cell.y - 1 >= 0 && !visited[cell.x, cell.y - 1])
        {
            neighbors.Add(new CellCoord(cell.x, cell.y - 1));
        }

        ShuffleList(neighbors);

        foreach (var neighbor in neighbors)
        {
            visited[cell.x, cell.y] = true;
            visited[neighbor.x, neighbor.y] = true;
            pairs.Add(new List<CellCoord> { cell, neighbor });

            return true;
        }

        return false;
    }

    private List<Domino> GetAvailableDominoes(int count)
    {
        List<Domino> dominoes = new List<Domino>();

        int maxValue = 0;
        while ((maxValue + 1) * (maxValue + 2) / 2 < count)
        {
            maxValue++;
        }

        Debug.Log($"Для {count} костей используем числа от 0 до {maxValue}");

        for (int a = 0; a <= maxValue; a++)
        {
            for (int b = a; b <= maxValue; b++)
            {
                dominoes.Add(new Domino(null, null, a, b));
            }
        }

        ShuffleList(dominoes);

        if (dominoes.Count > count)
        {
            dominoes = dominoes.GetRange(0, count);
        }

        return dominoes;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}