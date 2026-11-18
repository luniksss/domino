using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public void GenerateField(LevelStateManager state)
    {
        int sizeIndex = SaveManager.Instance.Data.currentFieldSizeIndex;

        var config = LevelConfig.FieldSizes[sizeIndex];

        int width = config.width;
        int height = config.height;

        Debug.Log($"Генерация поля {config.displayName}: Ширина {width}, Высота {height}");

        state.StartNewLevel(width, height);   

        if (width == 5 && height == 4)
        {
            int[,] presetArray = new int[,]
            {
                {2, 1, 3, 1},
                {3, 3, 2, 1},
                {0, 1, 1, 3},
                {0, 0, 2, 0},
                {3, 0, 2, 2}
            };
            state.field = presetArray;
            Debug.Log("Загружен тестовый массив 5x4.");
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    state.field[x, y] = (x + y) % 7;
                }
            }
            Debug.Log($"Сгенерировано поле {width}x{height} с дефолтными значениями.");
        }
    }
}