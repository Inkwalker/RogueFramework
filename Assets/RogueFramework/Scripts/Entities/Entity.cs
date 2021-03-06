﻿using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    [SelectionBase]
    public class Entity : MonoBehaviour
    {
        [SerializeField] bool blocksMovement = true;
        [SerializeField] bool blocksVision = false;

        private List<AEntityComponent> components;

        public IReadOnlyList<AEntityComponent> Components
        {
            get
            {
                if (components == null)
                {
                    components = new List<AEntityComponent>();
                    SearchEntityComponents();
                }

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

        private void OnTransformChildrenChanged()
        {
            SearchEntityComponents();
        }

        public T GetEntityComponent<T>(bool includeDisabled = false) where T : AEntityComponent
        {
            foreach (var item in Components)
            {
                if (item is T && (item.enabled || includeDisabled)) return item as T;
            }

            return null;
        }

        public List<T> GetEntityComponents<T>(bool includeDisabled = false) where T : AEntityComponent
        {
            var result = new List<T>();

            foreach(var item in Components)
            {
                if (item is T && (item.enabled || includeDisabled)) result.Add(item as T);
            }

            return result;
        }

        private T GetEntityComponentRecursively<T>(GameObject gameObject, bool includeDisabled) where T : AEntityComponent
        {
            var entity = gameObject.GetComponent<Entity>();
            T component = null;

            if (entity == null || entity == this)
            {
                component = gameObject.GetComponent<T>();

                if (!component.enabled && !includeDisabled) component = null;

                if (component == null)
                {
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        var child = gameObject.transform.GetChild(i).gameObject;
                        component = GetEntityComponentRecursively<T>(child, includeDisabled);

                        if (component != null) return component;
                    }
                }
            }

            return component;
        }

        private void GetEntityComponentsRecursively(GameObject gameObject, List<AEntityComponent> components)
        {
            var entity = gameObject.GetComponent<Entity>();

            if (entity == null || entity == this)
            {
                var c = gameObject.GetComponents<AEntityComponent>();
                components.AddRange(c);

                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    var child = gameObject.transform.GetChild(i).gameObject;
                    GetEntityComponentsRecursively(child, components);
                }
            }
        }

        public virtual void OnAddedToLevel(Level level)
        {
            Level = level;

            foreach (var component in Components)
            {
                if (component.enabled)
                    component.OnLevelChanged();
            }
        }

        public void Tick()
        {
            foreach (var component in Components)
            {
                if (component.enabled)
                    component.OnTick();
            }
        }

        public void SearchEntityComponents()
        {
            if (components == null) components = new List<AEntityComponent>();

            components.Clear();
            GetEntityComponentsRecursively(gameObject, components);
        }
    }
}
