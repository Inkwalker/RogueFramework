using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        public UnityEvent OnEntitiesLoaded;
        public EntityEvent OnEntityAdded;
        public EntityEvent OnEntityRemoved;

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

            OnEntitiesLoaded?.Invoke();
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
                {
                    var entity = entities[i];
                    UnregisterEntity(entity);
                    OnEntityRemoved?.Invoke(entity);
                }
            }

            foreach (var entity in addedEntities)
            {
                entity.OnAddedToLevel(level);
                OnEntityAdded?.Invoke(entity);
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
            return entities.Find(entity => entity.IsEntityOnCell(position));
        }

        public T Get<T>(Vector2Int position) where T : AEntityComponent
        {
            var entities = GetAll(position);
            foreach (var entity in entities)
            {
                T c = entity.GetEntityComponent<T>();

                if (c != null) return c;
            }

            return null;
        }

        public List<Entity> GetAll(Vector2Int position)
        {
            return entities.FindAll(entity => entity.IsEntityOnCell(position));
        }

        public List<T> GetAll<T>(Vector2Int position) where T : AEntityComponent
        {
            var result = new List<T>();

            foreach (var entity in GetAll(position))
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

        [System.Serializable] public class EntityEvent : UnityEvent<Entity> { }
    }
}
