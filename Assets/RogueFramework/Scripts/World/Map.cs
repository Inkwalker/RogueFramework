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
            var p = new Vector3Int(cell.x, cell.y, 0);

            return tilemap.GetTile<MapTile>(p);
        }

        public void Set(Vector2Int cell, MapTile tile)
        {
            var p = new Vector3Int(cell.x, cell.y, 0);

            tilemap.SetTile(p, tile);
        }
    }
}
