using System.Collections.Generic;
using UnityEngine;

public class TeamAI
{
    public PlayerAction GetNextMove(MazeData mazeData, Vector2Int currentPosition, Vector2Int endPosition)
    {
        // --- LOGIC CỦA ĐỘI THI BẮT ĐẦU TỪ ĐÂY ---

        // Ví dụ một AI ngẫu nhiên
        return (PlayerAction) PlayerAction.STAY;

        // --- LOGIC KẾT THÚC TẠI ĐÂY ---
    }
}