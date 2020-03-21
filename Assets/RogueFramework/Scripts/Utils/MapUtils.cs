using UnityEngine;

namespace RogueFramework
{
    public static class MapUtils
    {
        public static bool IsNeighborCells(Vector2Int A, Vector2Int B)
        {
            int dx = Mathf.Abs(A.x - B.x);
            int dy = Mathf.Abs(A.y - B.y);

            return (dx <= 1 && dy <= 1) && A != B;
        }

        public static Vector2Int[] GetNeighborCells(Vector2Int cell, bool includeDiagonal)
        {
            if (includeDiagonal)
            {
                return new Vector2Int[]
                {
                    cell + new Vector2Int(0, 1),
                    cell + new Vector2Int(1, 0),
                    cell + new Vector2Int(0, -1),
                    cell + new Vector2Int(-1, 0)
                };
            }
            else
            {
                return new Vector2Int[]
                {
                    cell + new Vector2Int(0, 1),
                    cell + new Vector2Int(1, 1),
                    cell + new Vector2Int(1, 0),
                    cell + new Vector2Int(1, -1),
                    cell + new Vector2Int(0, -1),
                    cell + new Vector2Int(-1, -1),
                    cell + new Vector2Int(-1, 0),
                    cell + new Vector2Int(-1, 1)
                };
            }
        }

        public static Vector2Int To8Dir(Vector2 dir)
        {
            dir.Normalize();

            float t_X = Mathf.Cos(67.5f * Mathf.Deg2Rad);
            float t_Y = Mathf.Sin(22.5f * Mathf.Deg2Rad);

            int x = Mathf.Abs(dir.x) > t_X ? (int)Mathf.Sign(dir.x) : 0;
            int y = Mathf.Abs(dir.y) > t_Y ? (int)Mathf.Sign(dir.y) : 0;

            return new Vector2Int(x, y);
        }

        public static Vector2Int To4Dir(Vector2 dir)
        {
            dir.Normalize();

            float threshold = Mathf.Sin(45 * Mathf.Deg2Rad);

            int x = Mathf.Abs(dir.x) > threshold ? (int)Mathf.Sign(dir.x) : 0;
            int y = Mathf.Abs(dir.y) > threshold ? (int)Mathf.Sign(dir.y) : 0;

            return new Vector2Int(x, y);
        }
    }
}
