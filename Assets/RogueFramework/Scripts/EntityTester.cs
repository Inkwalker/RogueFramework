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

        // Update is called once per frame
        void Update()
        {
            Vector3Int? cell = null;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cell = entity.Cell + Vector3Int.left;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cell = entity.Cell - Vector3Int.left;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                cell = entity.Cell + Vector3Int.up;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                cell = entity.Cell - Vector3Int.up;
            }

            if (cell.HasValue)
            {
                var tile = entity.Level.Tilemap.GetTile<MapTile>(cell.Value);

                if (tile != null && tile.Walkable)
                    entity.Position = entity.Level.Grid.cellSize * 0.5f + cell.Value;
            }
        }
    }
}
