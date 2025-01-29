using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 20;

    public Transform[,] grid;
    public Transform[,] debugGrid;

    private TetrisManager tetrisManager;
    // Start is called before the first frame update
    void Start()
    {
        tetrisManager = FindObjectOfType<TetrisManager>();
        grid = new Transform[width, height + 3];
        debugGrid = new Transform[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Create a new GameObject at this position, you can add a custom prefab instead of new GameObject() if desired
                GameObject cell = new GameObject($"Cell ({i},{j})");
                cell.transform.position = new Vector3(i, j, 0); // Position the cell at (i, j)
                debugGrid[i, j] = cell.transform;
            }
        }
    }

    public bool IsCellOccupied(Vector2Int position)
    {
        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height + 3)
        {
            return true;
        }
        return grid[position.x, position.y] != null;
    }

    public bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false; //If any cells are empty, the line is not full
            }
        }
        return true; //all cells in the row are filled
    }

    public void ClearFullLines()
    {
        int linesCleared = 0;
        //checks all rows to see if they are full, if so remove it and move all above down
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                ShiftRowsDown(y);
                y--; //recheck current row after shifting
                linesCleared++;
            }
        }
        if (linesCleared > 0)
        {
            tetrisManager.CalculateScore(linesCleared);
        }

    }

    public void ClearLine(int y) //remove any game objects on a line, will be used above
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject); //destroys gameobjects in a cell
            grid[x, y] = null; //removes any reference to gameobjects
        }
    }

    public void ShiftRowsDown(int clearedRow)
    {
        //takes whatever row was full, and moves anything above it down by one
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

    //debug cell grid, does not work in a build, only with gizmos enabled
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        if (debugGrid != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gizmos.DrawWireCube(debugGrid[i, j].position, Vector3.one);
                }
            }
        }
    }
}
