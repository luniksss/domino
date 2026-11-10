public class DominoGenerator
{
    public int Width { get; }
    public int Height { get; }
    public int[,] Cells { get; }

    public DominoGenerator(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new int[width, height];
    }

    public void GenerateDefault()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cells[x, y] = (x + y) % 7;
            }
        }
    }
}