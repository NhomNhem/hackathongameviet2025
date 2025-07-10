using System.Collections.Generic;
using UnityEngine;

public class TeamAI
{
    public PlayerAction GetNextMove(MazeData mazeData, Vector2Int currentPosition, Vector2Int endPosition)
    {
        // Directions and corresponding actions
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        PlayerAction[] actions = { PlayerAction.MOVE_UP, PlayerAction.MOVE_DOWN, PlayerAction.MOVE_LEFT, PlayerAction.MOVE_RIGHT };

        // BFS setup
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(currentPosition);
        visited.Add(currentPosition);

        bool found = false;

        while (queue.Count > 0)
        {
            var pos = queue.Dequeue();
            if (pos == endPosition)
            {
                found = true;
                break;
            }

            for (int i = 0; i < directions.Length; i++)
            {
                var next = pos + directions[i];
                if (!visited.Contains(next) && mazeData.GetCellType(next.x, next.y) != CellType.Wall)
                {
                    queue.Enqueue(next);
                    visited.Add(next);
                    cameFrom[next] = pos;
                }
            }
        }

        if (!found)
            return PlayerAction.STAY;

        // Reconstruct path
        Vector2Int step = endPosition;
        while (cameFrom.ContainsKey(step) && cameFrom[step] != currentPosition)
        {
            step = cameFrom[step];
        }

        // Determine direction
        Vector2Int move = step - currentPosition;
        for (int i = 0; i < directions.Length; i++)
        {
            if (move == directions[i])
                return actions[i];
        }

        return PlayerAction.STAY;
    }
}


