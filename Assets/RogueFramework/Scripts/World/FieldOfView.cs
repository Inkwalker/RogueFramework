using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    [RequireComponent(typeof(Tilemap))]
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] Level level = default;
        [SerializeField] Tile fogTile = default;
        [SerializeField] Tile exploredTile = default;
        [SerializeField] bool includeWalls = true;

        private Tilemap tilemap;
        private HashSet<Vector2Int> exploredTiles;
        private HashSet<Vector2Int> visibleTiles;
        private HashSet<Vector2Int> dirty;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();

            exploredTiles = new HashSet<Vector2Int>();
            visibleTiles  = new HashSet<Vector2Int>();
            dirty         = new HashSet<Vector2Int>();
        }

        private void Start()
        {
            RebuildTilemap();
        }

        public void ClearFog()
        {
            dirty.UnionWith(visibleTiles);

            visibleTiles.Clear();
            UpdateTiles();
        }

        public void ClearExplored()
        {
            dirty.UnionWith(exploredTiles);

            exploredTiles.Clear();
            UpdateTiles();
        }

        public void Clear()
        {
            dirty.UnionWith(visibleTiles);
            dirty.UnionWith(exploredTiles);

            visibleTiles.Clear();
            exploredTiles.Clear();
            UpdateTiles();
        }

        public void Set(Vector2Int cell, bool visible, bool explored)
        {
            if (visible)
                visibleTiles.Add(cell);
            else
                visibleTiles.Remove(cell);

            if (explored)
                exploredTiles.Add(cell);
            else
                exploredTiles.Remove(cell);

            SetTile(cell, visible, explored);
        }

        public bool IsVisible(Vector2Int cell)
        {
            return visibleTiles.Contains(cell);
        }

        public bool IsExplored(Vector2Int cell)
        {
            return exploredTiles.Contains(cell);
        }

        public void ComputeFoV(Vector2Int position, int distance, bool explore)
        {
            ClearFog();
            AppendFoV(position, distance, explore);
        }

        public void AppendFoV(Vector2Int position, int distance, bool explore)
        {
            var visibleCells = GetVisiblePositions(level, position, distance, includeWalls);

            foreach (var cell in visibleCells)
            {
                bool explored = IsExplored(cell) || explore;

                Set(cell, true, explored);
            }
        }

        public void AppendExplored(Vector2Int position, int distance)
        {
            var visibleCells = GetVisiblePositions(level, position, distance, includeWalls);

            foreach (var cell in visibleCells)
            {
                bool visible = IsVisible(cell);

                Set(cell, visible, true);
            }
        }

        private void UpdateTiles()
        {
            foreach (var cell in dirty)
            {
                var mapTile = level.Map.Get(cell);

                bool explored = exploredTiles.Contains(cell);
                bool visible = visibleTiles.Contains(cell);

                if (mapTile != null)
                {
                    SetTile(cell, visible, explored);
                }
            }

            dirty.Clear();
        }

        public void RebuildTilemap()
        {
            var bounds = level.Map.Bounds;

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    var cell = new Vector2Int(x, y);
                    var mapTile = level.Map.Get(cell);

                    bool explored = exploredTiles.Contains(cell);
                    bool visible = visibleTiles.Contains(cell);

                    if (mapTile != null)
                    {
                        SetTile(cell, visible, explored);
                    }
                }
            }
        }

        private void SetTile(Vector2Int cell, bool visible, bool explored)
        {
            Tile tile;

            if (visible) tile = null;
            else if (explored) tile = exploredTile;
            else tile = fogTile;

            var pos = ToTilemapPosition(cell);

            tilemap.SetTile(pos, tile);
        }

        private bool IsVisible(Vector2Int from, Vector2Int to)
        {
            bool result = true;

            Line(from.x, from.y, to.x, to.y, (x, y) =>
            {
                var tile = level.Map.Get(new Vector2Int(x, y));

                result = tile.Transparent;

                return tile.Transparent;
            });

            return result;
        }

        private static Vector3Int ToTilemapPosition(Vector2Int cellPosition)
        {
            return new Vector3Int(cellPosition.x, cellPosition.y, 0);
        }

        private static IReadOnlyCollection<Vector2Int> GetVisiblePositions(Level level, Vector2Int position, int distance, bool includeWalls)
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
                            var tile = level.Map.Get(p);
                            var entity = level.Entities.Get(p);

                            bool tileTransparent = tile != null && tile.Transparent;
                            bool entityTransparent = entity == null || !entity.BlocksVision;

                            if (tileTransparent || includeWalls)
                                result.Add(p);

                            return tileTransparent && entityTransparent;
                        });
                    }
                }
            }

            return result;
        }

        private delegate bool PlotFunction(int x, int y);
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
