using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int gridPosition;

    public void Initialize(Vector2Int startPos)
    {
        gridPosition = startPos;
    }

    public void Move(Vector2Int direction)
    {
        gridPosition += direction;
    }
}