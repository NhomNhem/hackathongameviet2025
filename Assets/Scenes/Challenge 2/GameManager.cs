using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerAction
{
    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT,
    MOVE_RIGHT,
    STAY
}

public class GameManager : MonoBehaviour
{
    [Header("Tham chiếu")] public MazeData currentMaze;
    public MazeRenderer mazeRenderer;
    public PlayerController playerPrefab;

    [Header("Cấu hình")] public float turnDelay = 1f;

    private PlayerController playerInstance;
    private TeamAI teamAI;
    private Vector2Int startPosition;
    private Vector2Int endPosition;

    void Start()
    {
        SetupGame();
        StartCoroutine(GameLoop());
    }

    void SetupGame()
    {
        GenerateMaze();

        var pathPositions = new List<Vector2Int>();
        for (var i = 0; i < currentMaze.grid.Length; i++)
        {
            if (currentMaze.grid[i] == CellType.Path)
            {
                pathPositions.Add(new Vector2Int(i % currentMaze.width, i / currentMaze.width));
            }
        }

        if (pathPositions.Count >= 2)
        {
            var startIndex = Random.Range(0, pathPositions.Count);
            startPosition = pathPositions[startIndex];

            pathPositions.RemoveAt(startIndex);

            var endIndex = Random.Range(0, pathPositions.Count);
            endPosition = pathPositions[endIndex];
        }
        else
        {
            startPosition = new Vector2Int(1, 1);
            endPosition = new Vector2Int(currentMaze.width - 2, currentMaze.height - 2);
        }

        currentMaze.grid[GetIndexFromPos(startPosition)] = CellType.Start;
        currentMaze.grid[GetIndexFromPos(endPosition)] = CellType.End;

        teamAI = new TeamAI();
        mazeRenderer.Render(currentMaze);

        playerInstance = Instantiate(playerPrefab);
        playerInstance.Initialize(startPosition);
        playerInstance.transform.position = GetWorldPositionFromGrid(startPosition);
    }

    private void GenerateMaze()
    {
        for (var i = 0; i < currentMaze.grid.Length; i++)
        {
            currentMaze.grid[i] = CellType.Wall;
        }

        var stack = new Stack<Vector2Int>();
        var startNode = new Vector2Int(1, 1);
        stack.Push(startNode);

        currentMaze.grid[GetIndexFromPos(startNode)] = CellType.Path;

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            var neighbors = GetUnvisitedNeighbors(currentNode);

            if (neighbors.Count > 0)
            {
                stack.Push(currentNode);
                var chosenNeighbor = neighbors[Random.Range(0, neighbors.Count)];

                RemoveWall(currentNode, chosenNeighbor);

                currentMaze.grid[GetIndexFromPos(chosenNeighbor)] = CellType.Path;
                stack.Push(chosenNeighbor);
            }
        }
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            if (playerInstance == null)
            {
                yield break;
            }

            if (playerInstance.gridPosition == endPosition)
            {
                Debug.Log("CHIẾN THẮNG! Đã tìm thấy lối ra!");
                Destroy(playerInstance.gameObject);
                yield break;
            }

            var action = teamAI.GetNextMove(currentMaze, playerInstance.gridPosition, endPosition);
            ExecuteAction(action);
            yield return new WaitForSeconds(turnDelay);
        }
    }

    private void ExecuteAction(PlayerAction action)
    {
        if (playerInstance == null) return;

        var direction = Vector2Int.zero;
        switch (action)
        {
            case PlayerAction.MOVE_UP:
                direction = Vector2Int.up;
                break;
            case PlayerAction.MOVE_DOWN:
                direction = Vector2Int.down;
                break;
            case PlayerAction.MOVE_LEFT:
                direction = Vector2Int.left;
                break;
            case PlayerAction.MOVE_RIGHT:
                direction = Vector2Int.right;
                break;
            case PlayerAction.STAY: return;
        }

        var targetPos = playerInstance.gridPosition + direction;
        var targetCell = currentMaze.GetCellType(targetPos.x, targetPos.y);

        if (targetCell != CellType.Wall)
        {
            playerInstance.Move(direction);
        }

        playerInstance.transform.position = GetWorldPositionFromGrid(playerInstance.gridPosition);
    }

    private Vector3 GetWorldPositionFromGrid(Vector2Int gridPos)
    {
        var mazeContainerPosition = mazeRenderer.transform.position;
        var offsetX = -currentMaze.width / 2f + 0.5f;
        var offsetY = -currentMaze.height / 2f + 0.5f;
        return new Vector3(gridPos.x + offsetX, gridPos.y + offsetY, 0) + mazeContainerPosition;
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int pos)
    {
        var neighbors = new List<Vector2Int>();
        var directions = new Vector2Int[]
            { new Vector2Int(0, 2), new Vector2Int(0, -2), new Vector2Int(2, 0), new Vector2Int(-2, 0) };

        foreach (var dir in directions)
        {
            var neighborPos = pos + dir;
            if (IsInsideMaze(neighborPos) && currentMaze.GetCellType(neighborPos.x, neighborPos.y) == CellType.Wall)
            {
                neighbors.Add(neighborPos);
            }
        }

        return neighbors;
    }

    private void RemoveWall(Vector2Int current, Vector2Int neighbor)
    {
        var wallX = (current.x + neighbor.x) / 2;
        var wallY = (current.y + neighbor.y) / 2;
        currentMaze.grid[GetIndexFromPos(new Vector2Int(wallX, wallY))] = CellType.Path;
    }

    private int GetIndexFromPos(Vector2Int pos) => pos.y * currentMaze.width + pos.x;

    private bool IsInsideMaze(Vector2Int pos) =>
        pos.x >= 0 && pos.x < currentMaze.width && pos.y >= 0 && pos.y < currentMaze.height;
}