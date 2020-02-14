using System;
using System.Collections;
using UnityEngine;

namespace RogueFramework
{
    [SelectionBase]
    public class Entity : MonoBehaviour
    {
        [SerializeField] bool blocksMovement = true;
        [SerializeField] bool blocksVision = false;

        private AEntityComponent[] components;
        public AEntityComponent[] Components
        {
            get
            {
                if (components == null)
                    components = GetComponentsInChildren<AEntityComponent>();
                return components;
            }
        }

        public Vector2 Position
        {
            get
            {
                if (Level == null) return Vector2.zero;
                Vector3 local = Level.Grid.WorldToLocal(transform.position);
                return Level.Grid.LocalToCellInterpolated(local);
            }
            set
            {
                if (Level == null) return;
                var pos = Level.Grid.CellToLocalInterpolated(value);
                transform.position = Level.Grid.LocalToWorld(pos);
            }

        }
        public Vector2Int Cell
        {
            get
            {
                if (Level == null) return Vector2Int.zero;
                var gridPos = Level.Grid.WorldToCell(transform.position);
                return new Vector2Int(gridPos.x, gridPos.y);
            }
            set
            {
                if (Level == null) return;
                Position = value + new Vector2(0.5f, 0.5f);
            }
        }
        public Level Level { get; private set; }
        public bool BlocksMovement { get => blocksMovement; set => blocksMovement = value; }
        public bool BlocksVision { get => blocksVision; set => blocksVision = value; }

        public T GetEntityComponent<T>(bool includeDisabled = false) where T : AEntityComponent
        {
            return Array.Find(Components, c => c is T && (c.enabled || includeDisabled)) as T;
        }

        public virtual void OnAddedToLevel(Level level)
        {
            Level = level;

            foreach (var component in Components)
            {
                component.OnLevelChanged();
            }
        }

        public void Tick()
        {
            foreach (var component in Components)
            {
                component.OnTick();
            }
        }
    }
}
