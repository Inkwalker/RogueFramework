using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class EntityList
    {
        private List<Entity> entities = new List<Entity>();
        private List<Actor> actors    = new List<Actor>();
        private List<Item> items      = new List<Item>();
        private Level level;

        public IReadOnlyList<Entity> All => entities;
        public IReadOnlyList<Actor> Actors => actors;
        public IReadOnlyList<Item> Items => items;

        public EntityList(Level level)
        {
            this.level = level;
        }

        public void Add(Entity entity)
        {
            if (entities.Contains(entity) == false)
            {
                entities.Add(entity);

                entity.OnAddedToLevel(level);

                var actor = entity.GetEntityComponent<Actor>();
                if (actor) actors.Add(actor);

                var item = entity.GetEntityComponent<Item>();
                if (item) items.Add(item);
            }
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);

            entity.OnAddedToLevel(null);

            var actor = entity.GetEntityComponent<Actor>();
            if (actor) actors.Remove(actor);

            var item = entity.GetEntityComponent<Item>();
            if (item) items.Remove(item);
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

        public bool IsWalkable(Vector2Int position)
        {
            var entity = Get(position);

            return entity == null || !entity.BlocksMovement;
        }
    }
}
