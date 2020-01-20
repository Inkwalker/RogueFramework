using System;
using UnityEngine;

namespace RogueFramework
{
    public class Entity : MonoBehaviour
    { 
        public Vector2 Position
        {
            get
            {
                Vector3 local = Level.Grid.WorldToLocal(transform.position);
                return Level.Grid.LocalToCellInterpolated(local);
            }
            set
            {
                var pos = Level.Grid.CellToLocalInterpolated(value);
                transform.position = Level.Grid.LocalToWorld(pos);
            }

        }
        public Vector2Int Cell
        {
            get
            {
                var gridPos = Level.Grid.WorldToCell(transform.position);
                return new Vector2Int(gridPos.x, gridPos.y);
            }
        }
        public Level Level { get; private set; }

        public virtual void OnAddedToLevel(Level level)
        {
            Level = level;
        }
    }
}
