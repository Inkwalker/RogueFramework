using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class EntityTester : MonoBehaviour
    {
        private Entity entity;
        private GoalMap pathfinding;

        private void Awake()
        {
            entity = GetComponent<Entity>();
        }

        private IEnumerator Start()
        {
            yield return null; //Skip a frame

            pathfinding = new GoalMap(entity.Level.Map, false);
            pathfinding.AddGoal(new Vector2Int(2, 2), 1);
            pathfinding.AddGoal(new Vector2Int(7, 2), 2);

            entity.Level.FoV.ComputeFoV(entity.Cell, 5);
        }

        void Update()
        {
            Vector2Int? cell = null;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cell = entity.Cell + Vector2Int.left;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cell = entity.Cell - Vector2Int.left;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                cell = entity.Cell + Vector2Int.up;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                cell = entity.Cell - Vector2Int.up;
            }

            if (cell.HasValue)
            {
                var tile = entity.Level.Map.Get(cell.Value);

                if (tile != null && tile.Walkable)
                {
                    entity.Position = cell.Value + new Vector2(0.5f, 0.5f);

                    entity.Level.FoV.ComputeFoV(entity.Cell, 5);
                }
            }

            if (pathfinding != null)
            {
                var path = pathfinding.FindPath(entity.Cell);

                if (path != null && path.Length > 0)
                {
                    for (int i = 0; i < path.Length - 1; i++)
                    {
                        var from = entity.Level.Grid.GetCellCenterWorld(new Vector3Int(path.Steps[i].x, path.Steps[i].y, 0));
                        var to = entity.Level.Grid.GetCellCenterWorld(new Vector3Int(path.Steps[i + 1].x, path.Steps[i + 1].y, 0));

                        Debug.DrawLine(from, to, Color.yellow);
                    }
                }
            }
        }
    }
}
