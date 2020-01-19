using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class Map
    {
        private Dictionary<Vector3Int, ACell> cells = new Dictionary<Vector3Int, ACell>();

        public event Action<Vector3Int> onCellChanged;

        public bool ContainsCell(Vector3Int position)
        {
            return cells.ContainsKey(position);
        }

        public ACell Get(Vector3Int position)
        {
            cells.TryGetValue(position, out ACell result);

            return result;
        }

        public void Set(Vector3Int position, ACell cell)
        {
            cells[position] = cell;

            onCellChanged?.Invoke(position);
        }
    }
}
