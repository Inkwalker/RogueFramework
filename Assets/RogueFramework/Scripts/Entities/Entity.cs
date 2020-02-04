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
        public bool BlocksMovement { get => blocksMovement; set => blocksMovement = value; }
        public bool BlocksVision { get => blocksVision; set => blocksVision = value; }

        private void Awake()
        {
            components = GetComponentsInChildren<AEntityComponent>();
        }

        private void OnDestroy()
        {
            if (Level != null && Level.Entities != null)
            {
                Level.Entities.Remove(this);
            }
        }

        public T GetEntityComponent<T>(bool includeDisabled = false) where T : AEntityComponent
        {
            return Array.Find(components, c => c is T && (c.enabled || includeDisabled)) as T;
        }

        public virtual void OnAddedToLevel(Level level)
        {
            Level = level;

            foreach (var component in components)
            {
                component.OnLevelChanged();
            }
        }

        public void Tick()
        {
            foreach (var component in components)
            {
                component.OnTick();
            }
        }
    }
}
