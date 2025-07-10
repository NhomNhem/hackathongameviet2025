using UnityEngine;
using System.Collections.Generic;

public class MazeRenderer : MonoBehaviour
{
    [Header("Prefabs Mê cung")] public GameObject pathPrefab;
    public GameObject wallPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;

    private Dictionary<Vector2Int, GameObject> spawnedMazeCells = new Dictionary<Vector2Int, GameObject>();
    private MazeData currentMazeData;

    public void Render(MazeData mazeData)
    {
        this.currentMazeData = mazeData;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        spawnedMazeCells.Clear();

        var offsetX = -mazeData.width / 2f + 0.5f;
        var offsetY = -mazeData.height / 2f + 0.5f;

        for (var y = 0; y < mazeData.height; y++)
        {
            for (var x = 0; x < mazeData.width; x++)
            {
                var position = new Vector2Int(x, y);
                var worldPosition = new Vector3(position.x + offsetX, position.y + offsetY, 0);

                var type = mazeData.GetCellType(x, y);
                var mazePrefab = GetPrefabForType(type);
                if (mazePrefab != null)
                {
                    var cellInstance = Instantiate(mazePrefab, this.transform);
                    cellInstance.transform.localPosition = worldPosition;
                    cellInstance.name = $"Maze_Cell_{x}_{y}";
                    spawnedMazeCells.Add(position, cellInstance);
                }
            }
        }
    }

    private GameObject GetPrefabForType(CellType type)
    {
        switch (type)
        {
            case CellType.Path: return pathPrefab;
            case CellType.Wall: return wallPrefab;
            case CellType.Start: return startPrefab;
            case CellType.End: return endPrefab;
            default: return null;
        }
    }
}