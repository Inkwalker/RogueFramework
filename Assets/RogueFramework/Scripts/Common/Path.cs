using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class Path
    {
        private List<Vector2Int> steps;

        public int Length => steps.Count;
        public List<Vector2Int> Steps => steps;

        public Path(IEnumerable<Vector2Int> steps)
        {
            this.steps = new List<Vector2Int>(steps);
        }
    }
}
