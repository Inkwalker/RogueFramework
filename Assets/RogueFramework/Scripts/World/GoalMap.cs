//Based on code from https://github.com/FaronBracy/RogueSharp

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace RogueFramework
{
    public class GoalMap
    {
        private const int wallValue = int.MinValue;
        private Map map;

        private bool allowDiagonalMovement;
        private bool isRecomputeNeeded;

        private Dictionary<Vector2Int, int> weights;
        private List<WeightedPoint>         goals;
        private HashSet<Vector2Int>         obstacles;

        public GoalMap(Map map, bool allowDiagonalMovement)
        {
            this.map = map;

            weights   = new Dictionary<Vector2Int, int>();
            goals     = new List<WeightedPoint>();
            obstacles = new HashSet<Vector2Int>();

            this.allowDiagonalMovement = allowDiagonalMovement;
        }

        public void AddGoal(Vector2Int position, int weight)
        {
            goals.Add(new WeightedPoint() { Position = position, Weight = weight });
            isRecomputeNeeded = true;
        }

        public void RemoveGoal(Vector2Int position)
        {
            goals.RemoveAll(goal => goal.Position == position);
            isRecomputeNeeded = true;
        }

        public void ClearGoals()
        {
            goals.Clear();
            isRecomputeNeeded = true;
        }

        public void AddObstacle(Vector2Int position)
        {
            obstacles.Add(position);
            isRecomputeNeeded = true;
        }

        public void RemoveObstacle(Vector2Int position)
        {
            obstacles.Remove(position);
            isRecomputeNeeded = true;
        }

        public void ClearObstacles()
        {
            obstacles.Clear();
            isRecomputeNeeded = true;
        }

        public Path FindPath(Vector2Int position)
        {
            ComputeCellWeightsIfNeeded();
            ReadOnlyCollection<Path> paths = FindPaths(position);
            return paths.First();
        }

        public ReadOnlyCollection<Path> FindPaths(Vector2Int position)
        {
            if (goals.Count < 1)
            {
                Debug.LogError("A goal must be set to find a path");
                return null;
            }

            if (!map.IsWalkable(position))
            {
                Debug.LogError($"Source ({position.x}, {position.y}) must be walkable to find a path");
                return null;
            }

            if (!goals.Any(g => map.IsWalkable(g.Position)))
            {
                Debug.LogError("A goal must be walkable to find a path");
                return null;
            }

            ComputeCellWeightsIfNeeded();
            var pathFinder = new GoalMapPathFinder(this);
            ReadOnlyCollection<Path> paths = pathFinder.FindPaths(position);

            if (paths.Count <= 1 && paths[0].Length <= 1)
            {
                Debug.LogError($"A path from Source ({position.x}, {position.y}) to any goal was not found");
                return null;
            }

            return paths;
        }

        private void ComputeCellWeightsIfNeeded()
        {
            if (isRecomputeNeeded)
            {
                ComputeCellWeights();
            }
        }

        private void ComputeCellWeights()
        {
            isRecomputeNeeded = false;

            weights.Clear();

            var bounds = map.Bounds;

            int totalMapCells = bounds.size.x * bounds.size.y;
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    var pos = new Vector2Int(x, y);

                    if (map.IsWalkable(pos))
                    {
                        weights[pos] = totalMapCells;
                    }
                    else
                    {
                        weights[pos] = wallValue;
                    }
                }
            }

            foreach (WeightedPoint goal in goals)
            {
                weights[goal.Position] = goal.Weight;
            }

            foreach (Vector2Int obstacle in obstacles)
            {
                weights[obstacle] = wallValue;
            }

            bool didCellWeightsChange = true;
            while (didCellWeightsChange)
            {
                didCellWeightsChange = false;
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    for (int x = bounds.xMin; x < bounds.xMax; x++)
                    {
                        var pos = new Vector2Int(x, y);

                        if (weights[pos] == wallValue)
                        {
                            continue;
                        }

                        List<WeightedPoint> neighbors = GetLowestWeightNeighbors(pos);
                        if (neighbors != null && neighbors.Count != 0)
                        {
                            int lowestValueFloorNeighbor = neighbors[0].Weight;
                            if (weights[pos] > lowestValueFloorNeighbor + 1)
                            {
                                weights[pos] = lowestValueFloorNeighbor + 1;
                                didCellWeightsChange = true;
                            }
                        }
                    }
                }
            }
        }

        private List<WeightedPoint> GetNeighbors(Vector2Int position)
        {
            var neighbors = new List<WeightedPoint>();

            var cardinal = new Vector2Int[4]
            {
                new Vector2Int(position.x, position.y + 1), // NORTH
                new Vector2Int(position.x + 1, position.y), // EAST
                new Vector2Int(position.x, position.y - 1), // SOUTH
                new Vector2Int(position.x - 1, position.y)  // WEST
            };

            foreach (var pos in cardinal)
            {
                if (weights.TryGetValue(pos, out int w) && w != wallValue)
                {
                    neighbors.Add(new WeightedPoint
                    {
                        Position = pos,
                        Weight = w
                    });
                }
            }

            if (allowDiagonalMovement)
            {
                var diagonal = new Vector2Int[4]
                {
                    new Vector2Int(position.x + 1, position.y + 1), // NORTH EAST
                    new Vector2Int(position.x - 1, position.y + 1), // NORTH WEST
                    new Vector2Int(position.x + 1, position.y - 1), // SOUTH EAST
                    new Vector2Int(position.x - 1, position.y - 1)  // SOUTH WEST
                };

                foreach (var pos in diagonal)
                {
                    if (weights.TryGetValue(pos, out int w) && w != wallValue)
                    {
                        neighbors.Add(new WeightedPoint
                        {
                            Position = pos,
                            Weight = w
                        });
                    }
                }
            }

            return neighbors;
        }

        private List<WeightedPoint> GetLowestWeightNeighbors(Vector2Int position)
        {
            List<WeightedPoint> neighbors = GetNeighbors(position);
            if (neighbors.Count <= 0)
            {
                return null;
            }
            int? targetWeight = null;
            foreach (WeightedPoint neighbor in neighbors)
            {
                if (targetWeight.HasValue)
                {
                    if (neighbor.Weight < targetWeight)
                    {
                        targetWeight = neighbor.Weight;
                    }
                }
                else
                {
                    targetWeight = neighbor.Weight;
                }
            }
            if (targetWeight >= weights[position])
            {
                // There are not any neighbors that have a smaller weight than the current cell
                return null;
            }
            var lowestWeightNeighbors = new List<WeightedPoint>();
            foreach (WeightedPoint neighbor in neighbors)
            {
                if (targetWeight.HasValue && neighbor.Weight == targetWeight.Value)
                {
                    lowestWeightNeighbors.Add(neighbor);
                }
            }
            return lowestWeightNeighbors;
        }

        private struct WeightedPoint : IEquatable<WeightedPoint>
        {
            public int Weight { get; set; }
            public Vector2Int Position { get; set; }

            public bool Equals(WeightedPoint other)
            {
                return Position.Equals(other.Position) && Weight == other.Weight;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                return obj is WeightedPoint other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Position.GetHashCode() * 398) ^ Weight;
                }
            }
        }

        private class GoalMapPathFinder
        {
            private readonly GoalMap goalMap;
            private readonly List<Path> paths;
            private readonly Stack<Vector2Int> currentPath;
            private readonly HashSet<Vector2Int> visited;

            public GoalMapPathFinder(GoalMap goalMap)
            {
                this.goalMap = goalMap;

                paths       = new List<Path>();
                currentPath = new Stack<Vector2Int>();
                visited     = new HashSet<Vector2Int>();
            }

            public ReadOnlyCollection<Path> FindPaths(Vector2Int position)
            {
                this.paths.Clear();
                currentPath.Clear();
                visited.Clear();

                RecursivelyFindPaths(position);

                var paths = new List<Path>();
                foreach (Path path in this.paths)
                {
                    paths.Add(path);
                }
                return new ReadOnlyCollection<Path>(paths);
            }

            private void RecursivelyFindPaths(Vector2Int position)
            {
                Vector2Int currentCell = position;
                if (visited.Add(currentCell))
                {
                    currentPath.Push(currentCell);
                    List<WeightedPoint> neighbors = goalMap.GetLowestWeightNeighbors(position);
                    if (neighbors != null)
                    {
                        foreach (WeightedPoint neighbor in neighbors)
                        {
                            RecursivelyFindPaths(neighbor.Position);
                        }
                    }
                    else
                    {
                        // We reached our destination so remove that from the list of visited cells in case there is another path here.
                        visited.Remove(currentCell);
                        paths.Add(new Path(currentPath.Reverse()));
                    }
                    currentPath.Pop();
                }
            }
        }
    }
}
