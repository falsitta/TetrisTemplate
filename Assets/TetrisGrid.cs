using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 20;
    private Transform[,] grid;

    void Start()
    {
        grid = new Transform[width, height];
    }

    public bool IsCellOccupied(Vector2Int position)
    {
        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height)
        {
            return true; // Out of bounds
        }
        return grid[position.x, position.y] != null;
    }

    public void AddToGrid(Transform piece)
    {
        foreach (Transform block in piece)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);
            grid[position.x, position.y] = block;
        }
    }

    public bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false; // If any cell in the row is empty, the line is not full
            }
        }
        return true; // All cells in the row are filled
    }

    public void ClearLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void ClearFullLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                ShiftRowsDown(y);
                y--; // Recheck the current row after shifting
            }
        }
    }

    public void ShiftRowsDown(int clearedRow)
    {
        for (int y = clearedRow; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = grid[x, y + 1];
                if (grid[x, y] != null)
                {
                    grid[x, y].position += Vector3.down;
                }
                grid[x, y + 1] = null;
            }
        }
    }
    public void AddBlockToGrid(Transform block, Vector2Int position)
    {
        grid[position.x, position.y] = block;
    }
}
