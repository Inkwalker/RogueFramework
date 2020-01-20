using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class EntityTester : MonoBehaviour
    {
        private Entity entity;

        private void Awake()
        {
            entity = GetComponent<Entity>();
        }

        private IEnumerator Start()
        {
            yield return null; //Skip a frame

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
        }
    }
}
