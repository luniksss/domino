using UnityEngine;

public class BoardView : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform boardParent;
    public GameObject dominoPrefab;
    public Sprite[] allDominoSprites;

    public void BuildBoard(LevelStateManager state)
    {
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }

        float cellSize = 1.0f;
        float gap = 0.0f;

        float startX = -(state.fieldWidth * (cellSize + gap)) / 2f + cellSize / 2f;
        float startY = (state.fieldHeight * (cellSize + gap)) / 2f - cellSize / 2f;

        for (int x = 0; x < state.fieldWidth; x++)
        {
            for (int y = 0; y < state.fieldHeight; y++)
            {
                GameObject cell = Instantiate(cellPrefab, boardParent);

                cell.name = $"Cell_{x}_{y}";
                cell.transform.localPosition = new Vector3(
                    startX + x * (cellSize + gap),
                    startY - y * (cellSize + gap),
                    0);

                cell.GetComponentInChildren<TMPro.TextMeshPro>().text = state.field[x, y].ToString();
            }
        }
    }

    public Vector2Int? GetGridCellUnderMouse(Camera cam)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            string[] parts = hit.collider.gameObject.name.Split('_');
            if (parts.Length == 3)
            {
                int x = int.Parse(parts[1]);
                int y = int.Parse(parts[2]);
                return new Vector2Int(x, y);
            }
        }
        return null;
    }

    public void DrawDomino(CellCoord start, CellCoord end, int valA, int valB)
    {
        GameObject cellA = GameObject.Find($"Cell_{start.x}_{start.y}");
        GameObject cellB = GameObject.Find($"Cell_{end.x}_{end.y}");

        if (cellA == null || cellB == null)
        {
            return;
        }

        Vector3 centerPos = (cellA.transform.position + cellB.transform.position) / 2f;
        centerPos.z = -1f;

        GameObject newDominoObj = Instantiate(dominoPrefab, boardParent);
        newDominoObj.transform.position = centerPos;

        int minVal = Mathf.Min(valA, valB);
        int maxVal = Mathf.Max(valA, valB);
        string targetName = $"tile_{minVal}_{maxVal}_0";

        Sprite mySprite = null;
        foreach (var s in allDominoSprites)
        {
            if (s.name == targetName) 
            { 
                mySprite = s; 
                break; 
            }
        }

        if (mySprite != null)
        {
            newDominoObj.GetComponent<SpriteRenderer>().sprite = mySprite;
        }

        bool isMinInStart = (valA == minVal);
        int minY = isMinInStart ? start.y : end.y;
        int otherY = isMinInStart ? end.y : start.y;

        int minX = isMinInStart ? start.x : end.x;
        int otherX = isMinInStart ? end.x : start.x;

        if (start.y != end.y)
        {
            if (minY > otherY)
            {
                newDominoObj.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        else
        {
            if (minX > otherX)
            {
                newDominoObj.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                newDominoObj.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        newDominoObj.name = $"tile_{minVal}_{maxVal}_0";
    }
}