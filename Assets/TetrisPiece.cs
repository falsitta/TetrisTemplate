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
        // Store the original position and rotation for rollback
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        // Rotate the piece clockwise
        transform.Rotate(0, 0, 90);

        // If the new position is invalid, try wall kicks
        if (!IsValidPosition())
        {
            if (!TryWallKick(originalPosition, originalRotation))
            {
                // Revert if no wall kick works
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                Debug.Log("Rotation invalid, reverted.");
            }
            else
            {
                Debug.Log("Rotation adjusted with wall kick.");
            }
        }
        else
        {
            Debug.Log("Rotation successful.");
        }
    }

    private bool TryWallKick(Vector3 originalPosition, Quaternion originalRotation)
    {
        // Define wall kick offsets (SRS guidelines)
        Vector2Int[] wallKickOffsets = new Vector2Int[]
        {
        new Vector2Int(1, 0),  // Move right
        new Vector2Int(-1, 0), // Move left
        new Vector2Int(0, -1), // Move down
        new Vector2Int(1, -1), // Move diagonally right-down
        new Vector2Int(-1, -1) // Move diagonally left-down
        };

        foreach (var offset in wallKickOffsets)
        {
            // Apply the offset
            transform.position += ((Vector3Int)offset);

            // Check if the new position is valid
            if (IsValidPosition())
            {
                return true; // Found a valid position
            }

            // Revert the offset if invalid
            transform.position -= (Vector3Int)offset;
        }

        return false; // No valid position found
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