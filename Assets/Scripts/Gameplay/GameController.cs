using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public LevelGenerator generator;
    public BoardView view;

    private CellCoord dragStartCell;
    private CellCoord dragEndCell;

    private List<CellCoord> freeCells = new List<CellCoord>();

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        bool isContinue = PlayerPrefs.GetInt("IsContinueLoad", 0) == 1;
        string savedJson = SaveManager.Instance.Data.levelStateJson;

        if (isContinue && !string.IsNullOrEmpty(savedJson))
        {
            LevelStateManager.Instance.DeserializeFromJson(savedJson);
        }
        else
        {
            generator.GenerateField(LevelStateManager.Instance);
        }

        view.BuildBoard(LevelStateManager.Instance);

        if (isContinue && !string.IsNullOrEmpty(savedJson))
        {
            foreach (var d in LevelStateManager.Instance.foundDominoes)
            {
                view.DrawDomino(d.startCell, d.endCell, d.valueA, d.valueB);
            }
        }

        RecalculateFreeCells();
    }

    private void RecalculateFreeCells()
    {
        freeCells.Clear();

        for (int x = 0; x < LevelStateManager.Instance.fieldWidth; x++)
        {
            for (int y = 0; y < LevelStateManager.Instance.fieldHeight; y++)
            {
                bool isOccupied = false;
                foreach (var d in LevelStateManager.Instance.foundDominoes)
                {
                    if ((d.startCell.x == x && d.startCell.y == y) ||
                        (d.endCell.x == x && d.endCell.y == y))
                    {
                        isOccupied = true;
                        break;
                    }
                }

                if (!isOccupied)
                {
                    freeCells.Add(new CellCoord(x, y));
                }
            }
        }
    }

    private bool IsCellFree(CellCoord cell)
    {
        foreach (var c in freeCells)
        {
            if (c.x == cell.x && c.y == cell.y) return true;
        }
        return false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                string objName = hit.collider.gameObject.name;
                if (objName.StartsWith("tile_"))
                {
                    if (Time.time - lastClickTime < doubleClickThreshold)
                    {
                        Domino dominoToRemove = null;
                        foreach (var d in LevelStateManager.Instance.foundDominoes)
                        {
                            string dominoName1 = $"tile_{d.valueA}_{d.valueB}_0";
                            string dominoName2 = $"tile_{d.valueB}_{d.valueA}_0";

                            if (objName == dominoName1 || objName == dominoName2)
                            {
                                dominoToRemove = d;
                                break;
                            }
                        }

                        if (dominoToRemove != null)
                        {
                            GameObject dominoObj = hit.collider.gameObject;
                            if (dominoObj != null)
                            {
                                DestroyDomino(dominoToRemove, dominoObj);
                            }
                        }
                    }
                    lastClickTime = Time.time;
                }
                else if (objName.StartsWith("Cell_"))
                {
                    Vector2Int? cell = view.GetGridCellUnderMouse(Camera.main);
                    if (cell.HasValue)
                    {
                        OnCellPressed(new CellCoord(cell.Value.x, cell.Value.y));
                    }
                }
            }
            else
            {
                Vector2Int? cell = view.GetGridCellUnderMouse(Camera.main);
                if (cell.HasValue)
                {
                    OnCellPressed(new CellCoord(cell.Value.x, cell.Value.y));
                }
            }
        }

        if (Input.GetMouseButton(0) && dragStartCell != null)
        {
            Vector2Int? cell = view.GetGridCellUnderMouse(Camera.main);
            if (cell.HasValue)
            {
                OnCellDragged(new CellCoord(cell.Value.x, cell.Value.y));
            }
        }

        if (Input.GetMouseButtonUp(0) && dragStartCell != null)
        {
            Vector2Int? cell = view.GetGridCellUnderMouse(Camera.main);
            if (cell.HasValue)
            {
                OnCellReleased(new CellCoord(cell.Value.x, cell.Value.y));
            }
            else
            {
                dragStartCell = null;
            }
        }
    }

    public void OnCellPressed(CellCoord cell)
    {
        if (IsCellFree(cell))
        {
            dragStartCell = cell;
            dragEndCell = cell;
        }
        else
        {
            dragStartCell = null;
        }
    }

    public void OnCellDragged(CellCoord cell)
    {
        if (dragStartCell == null)
        {
            return;
        }

        if (IsCellFree(cell) && IsAdjacent(dragStartCell, cell) && IsValidDomino(dragStartCell, cell))
        {
            dragEndCell = cell;
        }
    }

    public void OnCellReleased(CellCoord endCell)
    {
        if (dragStartCell != null && (dragStartCell.x != endCell.x || dragStartCell.y != endCell.y))
        {
            if (IsCellFree(dragStartCell) && IsCellFree(endCell) && IsValidDomino(dragStartCell, endCell))
            {
                int valA = LevelStateManager.Instance.field[dragStartCell.x, dragStartCell.y];
                int valB = LevelStateManager.Instance.field[endCell.x, endCell.y];

                Domino newDomino = new Domino(dragStartCell, endCell, valA, valB);

                if (IsAlreadyFound(newDomino))
                {
                    Debug.Log($"Кость {valA}:{valB} уже собрана!");
                }
                else
                {
                    LevelStateManager.Instance.foundDominoes.Add(newDomino);
                    Debug.Log($"Создана доминошка: {valA}:{valB}");

                    view.DrawDomino(dragStartCell, endCell, valA, valB);

                    freeCells.RemoveAll(c => (c.x == dragStartCell.x && c.y == dragStartCell.y));
                    freeCells.RemoveAll(c => (c.x == endCell.x && c.y == endCell.y));

                    CheckWinCondition();
                }
            }
        }

        dragStartCell = null;
        dragEndCell = null;
    }

    private bool IsAdjacent(CellCoord a, CellCoord b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return (dx + dy) == 1;
    }

    private bool IsValidDomino(CellCoord start, CellCoord end)
    {
        if (!IsAdjacent(start, end))
        {
            return false;
        }
        if (IsCellBlocked(start) || IsCellBlocked(end))
        {
            return false;
        }
        return true;
    }

    private bool IsAlreadyFound(Domino newDomino)
    {
        foreach (var found in LevelStateManager.Instance.foundDominoes)
        {
            bool matchStraight = (found.valueA == newDomino.valueA && found.valueB == newDomino.valueB);
            bool matchReversed = (found.valueA == newDomino.valueB && found.valueB == newDomino.valueA);
            if (matchStraight || matchReversed)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCellBlocked(CellCoord c) => false;

    private void CheckWinCondition()
    {
        if (freeCells.Count == 0)
        {
            SaveManager.Instance.Data.levelStateJson = "";           

            int sizeIndex = SaveManager.Instance.Data.currentFieldSizeIndex;
            int reward = LevelConfig.FieldSizes[sizeIndex].rewardCoins;
            SaveManager.Instance.AddCoins(reward);

            SaveManager.Instance.Data.currentLevel++;
            SaveManager.Instance.Save();

            GameLauncher.LoadMenuWithVictory(reward);
        }
    }

    private void DestroyDomino(Domino d, GameObject dominoObject)
    {
        if (dominoObject == null || !LevelStateManager.Instance.foundDominoes.Remove(d))
        {
            return;
        }

        Debug.Log($"Уничтожение доминошки: {d.valueA}:{d.valueB}");

        freeCells.Add(d.startCell);
        freeCells.Add(d.endCell);

        Destroy(dominoObject);
    }

    public void SaveCurrentState()
    {
        string json = LevelStateManager.Instance.SerializeToJson();
        SaveManager.Instance.Data.levelStateJson = json;
        SaveManager.Instance.Data.hasSavedGame = true;
        SaveManager.Instance.Save();
    }
}