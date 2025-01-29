using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    private TetrisGrid grid;
    private float dropInterval = 1f;
    private float dropTimer;
    bool isLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
        dropTimer = dropInterval;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAutomaticDrop();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { Move(Vector3.left); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { Move(Vector3.right); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { Move(Vector3.down); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { RotatePiece(); }
        if (Input.GetKeyDown(KeyCode.Space)) { FullDrop(); }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;

        if (!IsValidPosition())
        {
            transform.position -= direction;

            if (direction == Vector3.down)
            {
                LockPiece();
            }
        }
    }

    //drops the piece to the bottom immediately
    private void FullDrop()
    {
        do
        {
            Move(Vector3.down);
        } while (isLocked == false);
    }

    private void RotatePiece()
    {
        //store the original position and rotation for rollback
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        transform.Rotate(0, 0, 90);

        if (!IsValidPosition())
        {
            if (!TryWallKick(originalPosition, originalRotation))
            {
                //revert if no wallkick works
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                Debug.Log("Rotation invalid, reverting rotation/position");
            }
            else
            {
                Debug.Log("Rotation/position adjusted with wall kick");
            }
        }
    }

    private bool IsValidPosition()
    {
        foreach (Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);

            if (grid.IsCellOccupied(position))
            {
                return false; //blocked or out of bounds
            }
        }
        return true;
    }

    private void HandleAutomaticDrop()
    {
        dropTimer -= Time.deltaTime;

        if (dropTimer <= 0)
        {
            Move(Vector3.down);
            dropTimer = dropInterval; //reset timer
        }
    }

    private void LockPiece()
    {
        isLocked = true;
        foreach (Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);
            grid.AddBlockToGrid(block, position); //add block to grid
        }

        grid.ClearFullLines(); //check for any full lines
        if (FindObjectOfType<TetrisSpawner>())
        {
            FindObjectOfType<TetrisSpawner>().SpawnPiece(); //spawn new piece
        }
        Destroy(this); //remove this script
    }

    private bool TryWallKick(Vector3 originalPosition, Quaternion originalRotation)
    {
        //define wall kicks (srs guidelines)
        Vector2Int[] wallKickOffsets = new Vector2Int[]
            {
                new Vector2Int(1,0), //Move Right by 1
                new Vector2Int(-1,0), //Move Left
                new Vector2Int(0,-1), //Move Down
                new Vector2Int(1,-1), //Move Diagonally right-down
                new Vector2Int(-1,-1), //Move Diagonally left-down
                
                new Vector2Int(2,0), //Move Right by 2
                new Vector2Int(-2,0), //Move Left
                new Vector2Int(0,-2), //Move Down
                new Vector2Int(2,-1), //Move Diagonally right-down
                new Vector2Int(-2,-1), //Move Diagonally left-down
                new Vector2Int(2,-2), //Move Diagonally right-down
                new Vector2Int(-2,-2), //Move Diagonally left-down
                
                new Vector2Int(3,0), //Move Right by 3
                new Vector2Int(-3,0), //Move Left
                new Vector2Int(0,-3), //Move Down
                new Vector2Int(3,-1), //Move Diagonally right-down
                new Vector2Int(-3,-1), //Move Diagonally left-down
                new Vector2Int(3,-2), //Move Diagonally right-down
                new Vector2Int(-3,-2), //Move Diagonally left-down
                new Vector2Int(3,-3), //Move Diagonally right-down
                new Vector2Int(-3,-3), //Move Diagonally left-down
            };

        foreach (Vector2Int offset in wallKickOffsets)
        {
            //apply offset to the piece
            transform.position += (Vector3Int)offset;

            //check if the new position is valid
            if (IsValidPosition())
            {
                return true;
            }
            //revert position if invalid
            transform.position -= (Vector3Int)offset;

        }
        return false;
        //loop through all of the offsets to see which one is valid
    }
}
