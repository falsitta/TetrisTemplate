using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int Position;
    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}