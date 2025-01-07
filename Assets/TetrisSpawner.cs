using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs; // Array of Tetromino prefabs
    private TetrisGrid grid; // Reference to the TetrisGrid
    private GameObject nextPiece;

    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>(); // Find the grid instance in the scene
        if (grid == null)
        {
            Debug.LogError("TetrisGrid not found in the scene. Ensure a TetrisGrid object exists.");
            return;
        }

        SpawnPiece(); // Initial spawn
    }

    public void SpawnPiece()
    {
        // Calculate the top-center spawn position based on grid dimensions
        Vector3 spawnPosition = new Vector3(
            Mathf.Floor(grid.width / 2f), // Horizontal center of the grid
            grid.height - 1,             // Top row of the grid
            0                            // Z position for 2D
        );

        if (nextPiece != null)
        {
            nextPiece.SetActive(true);
            nextPiece.transform.position = spawnPosition;
        }
        else
        {
            nextPiece = InstantiateRandomPiece();
            nextPiece.transform.position = spawnPosition;
        }

        // Prepare the next piece for preview
        nextPiece = InstantiateRandomPiece();
        nextPiece.SetActive(false); // Deactivate until it's the active piece
    }

    private GameObject InstantiateRandomPiece()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        return Instantiate(tetrominoPrefabs[index]);
    }
}