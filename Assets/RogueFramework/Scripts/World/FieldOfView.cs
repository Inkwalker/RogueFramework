using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    [RequireComponent(typeof(Tilemap))]
    public class FieldOfView : MonoBehaviour
    {
        private Tilemap tilemap;

        [SerializeField] Map map = default;
        [SerializeField] Tile fogTile = default;
        [SerializeField] bool includeWalls = true;

        private delegate bool PlotFunction(int x, int y);

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            Clear();
        }

        public void Clear()
        {
            tilemap.ClearAllTiles();
            var bounds = map.Bounds;

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    var cell = new Vector2Int(x, y);

                    Set(cell, false);
                }
            }
        }

        public void Set(Vector2Int cell, bool visible)
        {
            var pos = ToTilemapPosition(cell);
            var tile = visible ? null : fogTile;

            tilemap.SetTile(pos, tile);
        }

        public bool Get(Vector2Int cell)
        {
            var pos = ToTilemapPosition(cell);
            return tilemap.GetTile(pos) == null;
        }

        public void ComputeFoV(Vector2Int position, int distance)
        {
            Clear();
            AppendFoV(position, distance);
        }

        public void AppendFoV(Vector2Int position, int distance)
        {
            var visibleCells = GetVisiblePositions(map, position, distance, includeWalls);

            foreach (var cell in visibleCells)
            {
                Set(cell, true);
            }
        }

        private bool IsVisible(Vector2Int from, Vector2Int to)
        {
            bool result = true;

            Line(from.x, from.y, to.x, to.y, (x, y) =>
            {
                var tile = map.Get(new Vector2Int(x, y));

                result = tile.Transparent;

                return tile.Transparent;
            });

            return result;
        }

        private static Vector3Int ToTilemapPosition(Vector2Int cellPosition)
        {
            return new Vector3Int(cellPosition.x, cellPosition.y, 0);
        }

        private static IReadOnlyCollection<Vector2Int> GetVisiblePositions(Map map, Vector2Int position, int distance, bool includeWalls)
        {
            var result = new HashSet<Vector2Int>();

            var p0 = new Vector2Int(position.x - distance, position.y - distance);
            var p1 = new Vector2Int(position.x + distance, position.y + distance);

            for (int x = p0.x; x <= p1.x; x++)
            {
                for (int y = p0.y; y <= p1.y; y++)
                {
                    //for every tile on the perimeter
                    if ((x == p0.x || x == p1.x) || (y == p0.y || y == p1.y))
                    {
                        Line(position.x, position.y, x, y, 
                        (pX, pY) =>
                        {
                            var p = new Vector2Int(pX, pY);
                            var tile = map.Get(p);

                            if (tile.Transparent || includeWalls)
                                result.Add(p);

                            return tile.Transparent;
                        });
                    }
                }
            }

            return result;
        }

        private static void Line(int x0, int y0, int x1, int y1, PlotFunction plot)
        {
            int w = x1 - x0;
            int h = y1 - y0;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Mathf.Abs(w);
            int shortest = Mathf.Abs(h);
            if (!(longest > shortest))
            {
                longest = Mathf.Abs(h);
                shortest = Mathf.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                if (plot(x0, y0) == false) return;

                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x0 += dx1;
                    y0 += dy1;
                }
                else
                {
                    x0 += dx2;
                    y0 += dy2;
                }
            }
        }
    }
}
