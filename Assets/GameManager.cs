using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private TetrisSpawner spawner;
    private ScoreManager scoreManager;
    private TetrisGrid grid;

    void Start()
    {
        spawner = FindObjectOfType<TetrisSpawner>();
        scoreManager = FindObjectOfType<ScoreManager>();
        grid = FindObjectOfType<TetrisGrid>();
    }

    public void ClearLines()
    {
        int linesCleared = 0;
        for (int y = 0; y < grid.height; y++)
        {
            if (grid.IsLineFull(y))
            {
                grid.ClearLine(y);
                grid.ShiftRowsDown(y);
                linesCleared++;
                y--; // Recheck the shifted row
            }
        }
        if (linesCleared > 0)
        {
            scoreManager.AddScore(linesCleared);
        }
    }

    public void CheckGameOver()
    {
        if (grid.IsCellOccupied(new Vector2Int(5, grid.height - 1)))
        {
            Debug.Log("Game Over!");
            enabled = false; // Stop gameplay
        }
    }
}
