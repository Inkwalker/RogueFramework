using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    [RequireComponent(typeof(Tilemap))]
    public class Map : MonoBehaviour
    {
        private Tilemap tilemap;

        public Vector2Int Size
        {
            get
            {
                var tilemapSize = tilemap.size;
                return new Vector2Int(tilemapSize.x, tilemapSize.y);
            }
        }

        public BoundsInt Bounds => tilemap.cellBounds;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        public MapTile Get(Vector2Int cell)
        {
            var p = GetTilemapPosition(cell);

            return tilemap.GetTile<MapTile>(p);
        }

        public void Set(Vector2Int cell, MapTile tile)
        {
            var p = GetTilemapPosition(cell);

            tilemap.SetTile(p, tile);
        }

        public bool IsWalkable(Vector2Int cell)
        {
            var p = GetTilemapPosition(cell);

            var tile = tilemap.GetTile<MapTile>(p);

            if (tile == null) return false;

            return tile.Walkable;
        }

        public bool IsTransparent(Vector2Int cell)
        {
            var p = GetTilemapPosition(cell);

            var tile = tilemap.GetTile<MapTile>(p);

            if (tile == null) return false;

            return tile.Transparent;
        }

        private Vector3Int GetTilemapPosition(Vector2Int cell)
        {
            return new Vector3Int(cell.x, cell.y, 0);
        }
    }
}
