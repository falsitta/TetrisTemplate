using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    //public List<GameObject> tetrominoes;
    public GameObject[] tetrominoPrefabs;
    private TetrisGrid grid;
    private GameObject nextPiece;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
        if (grid == null)
        {
            Debug.LogError("NO GRID!");
            return;
        }

        //spawn initial piece here
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        //calculate the center of the grid based on dimensions
        Vector3 spawnPosition = new Vector3(
            Mathf.Floor(grid.width / 2), //x
            grid.height,             //y 
            0);                          //z

        if (nextPiece != null)
        {
            nextPiece.SetActive(true);
            nextPiece.transform.position = spawnPosition;
        }
        else
        {
            //spawn random piece
            nextPiece = InstantiateRandomPiece();
            nextPiece.transform.position = spawnPosition;
        }

        //prepare next piece
        //spawn next piece here
        nextPiece = InstantiateRandomPiece();
        nextPiece.SetActive(false); //deactivate until its time to use it
    }

    private GameObject InstantiateRandomPiece()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        return Instantiate(tetrominoPrefabs[index]);
    }
}
