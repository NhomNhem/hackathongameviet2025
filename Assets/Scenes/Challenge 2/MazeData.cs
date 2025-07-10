using UnityEngine;

public enum CellType
{
    Path,
    Wall,
    Start,
    End
}

[CreateAssetMenu(fileName = "NewMazeData", menuName = "Game Việt/Maze Data")]
public class MazeData : ScriptableObject
{
    [Header("Kích thước mê cung")] public int width = 10;
    public int height = 10;

    [Header("Dữ liệu Grid")] public CellType[] grid;

    public CellType GetCellType(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return CellType.Wall;
        return grid[y * width + x];
    }

    private void OnValidate()
    {
        if (grid == null || grid.Length != width * height)
        {
            grid = new CellType[width * height];
        }
    }
}