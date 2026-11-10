using UnityEngine;

public class DominoGameController : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private DominoView view;

    private DominoGenerator _generator;
    private int _width, _height;

    private bool _dragging;
    private Vector2Int _startCell;
    private Vector2Int _lastCell;

    public void ExitToMenu()
    {
        GameLauncher.LoadMenuScene();
    }

    private void Start()
    {
        int idx = SaveManager.Instance.Data.currentFieldSizeIndex;

        var cfg = LevelConfig.FieldSizes[idx];
        _width = cfg.width;
        _height = cfg.height;

        _generator = new DominoGenerator(_width, _height);
        _generator.GenerateDefault();
        view.Build(_generator.Cells, _width, _height);

        Debug.Log($"Игра запущена: {cfg.displayName} ({_width}×{_height}), награда {cfg.rewardCoins}");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cell = WorldToCell(Input.mousePosition);
            if (cell.HasValue)
            {
                _dragging = true;
                _startCell = cell.Value;
                _lastCell = cell.Value;
                view.ClearHighlights();
                view.SetSelected(cell.Value, true);
            }
        }

        if (_dragging && Input.GetMouseButton(0))
        {
            var cell = WorldToCell(Input.mousePosition);
            if (cell.HasValue && cell.Value != _lastCell && IsAdjacent(_lastCell, cell.Value))
            {
                _lastCell = cell.Value;
                view.SetSelected(cell.Value, true);
            }
        }

        if (_dragging && Input.GetMouseButtonUp(0))
        {
            _dragging = false;
            Debug.Log($"Соединение: {_startCell} → {_lastCell}");
        }
    }

    private Vector2Int? WorldToCell(Vector3 screenPos)
    {
        if (Camera.main == null)
        {
            return null;
        }

        var world = Camera.main.ScreenToWorldPoint(screenPos);

        return view.GetCellAt(world);
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
        => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;
}