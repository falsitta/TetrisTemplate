using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    private TetrisGrid grid;
    private float dropInterval = 1.0f; // Time between automatic drops
    private float dropTimer;

    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
        dropTimer = dropInterval;
    }

    void Update()
    {
        HandleInput(); // Handle player input
        HandleAutomaticDrop(); // Automatically move down
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(Vector3.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Move(Vector3.right);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Move(Vector3.down);
        if (Input.GetKeyDown(KeyCode.Space)) RotatePiece();
    }

    private void HandleAutomaticDrop()
    {
        dropTimer -= Time.deltaTime;

        if (dropTimer <= 0)
        {
            Move(Vector3.down);
            dropTimer = dropInterval; // Reset the timer
        }
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction;

        if (!IsValidPosition())
        {
            transform.position -= direction; // Revert the move if invalid

            if (direction == Vector3.down) // If moving down fails
            {
                LockPiece(); // Lock the piece in place
            }
        }
    }

    private void RotatePiece()
    {
        transform.Rotate(0, 0, 90);

        if (!IsValidPosition())
        {
            transform.Rotate(0, 0, -90); // Revert rotation if invalid
        }
    }

    private bool IsValidPosition()
    {
        foreach (Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);

            if (grid.IsCellOccupied(position))
            {
                return false; // Blocked or out of bounds
            }
        }
        return true; // Valid position
    }

    private void LockPiece()
    {
        foreach (Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);
            grid.AddBlockToGrid(block, position); // Add block to grid
        }

        grid.ClearFullLines(); // Check and clear full lines
        FindObjectOfType<TetrisSpawner>().SpawnPiece(); // Spawn a new piece
        Destroy(this); // Remove this piece's script
    }
}