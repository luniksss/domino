using UnityEngine;

public class DominoView : MonoBehaviour
{
    [Header("Отображение")]
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float gap = 0.05f;
    [SerializeField] private Color cellColor = new Color(0.90f, 0.85f, 0.70f);
    [SerializeField] private Color selectedColor = new Color(0.50f, 0.80f, 1.00f);

    private int[,] _cells;
    private int _width, _height;
    private GameObject[,] _cellObjects;
    private SpriteRenderer[,] _renderers;
    private Sprite _whiteSprite;

    public void Build(int[,] cells, int width, int height)
    {
        _cells = cells;
        _width = width;
        _height = height;

        if (_whiteSprite == null)
        {
            _whiteSprite = CreateWhiteSprite();
        }

        ClearOld();
        CreateCells();
        CenterField();
    }

    private void ClearOld()
    {
        if (_cellObjects == null)
        {
            return;
        }
        foreach (var go in _cellObjects)
        {
            if (go)
            {
                Destroy(go);
            }
        }
        _cellObjects = null;
        _renderers = null;
    }

    private void CreateCells()
    {
        _cellObjects = new GameObject[_width, _height];
        _renderers = new SpriteRenderer[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var go = new GameObject($"Cell_{x}_{y}");
                go.transform.SetParent(transform, false);
                go.transform.localPosition = new Vector3(
                    x * (cellSize + gap),
                    y * (cellSize + gap),
                    0f);

                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = _whiteSprite;
                sr.color = cellColor;
                sr.sortingOrder = 0;
                _renderers[x, y] = sr;

                var col = go.AddComponent<BoxCollider2D>();
                col.size = Vector2.one * cellSize;

                var textGo = new GameObject("Text");
                textGo.transform.SetParent(go.transform, false);
                textGo.transform.localPosition = new Vector3(0, 0, -0.01f);
                var tm = textGo.AddComponent<TextMesh>();
                tm.text = _cells[x, y].ToString();
                tm.fontSize = 80;
                tm.characterSize = 0.1f;
                tm.anchor = TextAnchor.MiddleCenter;
                tm.alignment = TextAlignment.Center;
                tm.color = Color.black;

                _cellObjects[x, y] = go;
            }
        }
    }

    private Sprite CreateWhiteSprite()
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }

    private void CenterField()
    {
        float totalW = _width * cellSize + (_width - 1) * gap;
        float totalH = _height * cellSize + (_height - 1) * gap;
        transform.position = new Vector3(
            -totalW / 2f + cellSize / 2f,
            -totalH / 2f + cellSize / 2f,
            0f);
    }

    public Vector2Int? GetCellAt(Vector2 worldPos)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_cellObjects[x, y].GetComponent<BoxCollider2D>().OverlapPoint(worldPos))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }

    public void SetSelected(Vector2Int cell, bool selected)
    {
        if (cell.x < 0 || cell.x >= _width || cell.y < 0 || cell.y >= _height)
        {
            return;
        }
        _renderers[cell.x, cell.y].color = selected ? selectedColor : cellColor;
    }

    public void ClearHighlights()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _renderers[x, y].color = cellColor;
            }
        }
    }

    public void SetMatched(Vector2Int cell, bool matched)
    {
        if (cell.x < 0 || cell.x >= _width || cell.y < 0 || cell.y >= _height)
        {
            return;
        }

        _renderers[cell.x, cell.y].color = matched
            ? new Color(0.6f, 0.9f, 0.6f)
            : cellColor;
    }
}