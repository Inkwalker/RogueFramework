using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class EntityIndex : MonoBehaviour
    {
        private List<Entity> entities = new List<Entity>();
        private List<Actor> actors    = new List<Actor>();
        private List<Item> items      = new List<Item>();
        private Level level;

        public IReadOnlyList<Entity> All => entities;
        public IReadOnlyList<Actor> Actors => actors;
        public IReadOnlyList<Item> Items => items;

        private void Awake()
        {
            level = GetComponentInParent<Level>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var entity = child.GetComponent<Entity>();

                if (entity != null)
                {
                    RegisterEntity(entity);
                }
            }
        }

        private void Start()
        {
            foreach (var entity in entities)
            {
                entity.OnAddedToLevel(level);
            }
        }

        private void OnTransformChildrenChanged()
        {
            var attachedEntities = new List<Entity>();

            var addedEntities = new List<Entity>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var entity = child.GetComponent<Entity>();

                if (entity != null)
                {
                    attachedEntities.Add(entity);

                    if (RegisterEntity(entity))
                    {
                        addedEntities.Add(entity);
                    }
                }
            }

            for (int i = entities.Count - 1; i >= 0; i--)
            {
                if (attachedEntities.Contains(entities[i]) == false)
                    UnregisterEntity(entities[i]);
            }

            foreach (var entity in addedEntities)
            {
                entity.OnAddedToLevel(level);
            }
        }

        private bool RegisterEntity(Entity entity)
        {
            if (entities.Contains(entity)) return false;

            entities.Add(entity);

            var actor = entity.GetEntityComponent<Actor>();
            if (actor) actors.Add(actor);

            var item = entity.GetEntityComponent<Item>();
            if (item) items.Add(item);

            return true;
        }

        private bool UnregisterEntity(Entity entity)
        {
            if (entities.Remove(entity) == false) return false;

            var actor = entity.GetEntityComponent<Actor>();
            if (actor) actors.Remove(actor);

            var item = entity.GetEntityComponent<Item>();
            if (item) items.Remove(item);

            entity.OnAddedToLevel(null);

            return true;
        }

        public void Add(Entity entity)
        {
            entity.transform.SetParent(transform);
        }

        public void Remove(Entity entity)
        {
            entity.transform.SetParent(null);
        }

        public Entity Get(Vector2Int position)
        {
            return entities.Find(entity => entity.Cell == position);
        }

        public T Get<T>(Vector2Int position) where T : AEntityComponent
        {
            foreach (var entity in entities)
            {
                T c = entity.GetEntityComponent<T>();

                if (c != null) return c;
            }

            return null;
        }

        public List<Entity> GetAll(Vector2Int position)
        {
            return entities.FindAll(entity => entity.Cell == position);
        }

        public List<T> GetAll<T>(Vector2Int position) where T : AEntityComponent
        {
            var result = new List<T>();

            foreach (var entity in entities)
            {
                T c = entity.GetEntityComponent<T>();

                if (c != null) result.Add(c);
            }

            return result;
        }

        public bool IsWalkable(Vector2Int position)
        {
            var entity = Get(position);

            return entity == null || !entity.BlocksMovement;
        }
    }
}
