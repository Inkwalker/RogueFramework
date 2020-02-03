using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class EntityList
    {
        private List<Entity> entities = new List<Entity>();
        private List<Actor> actors    = new List<Actor>();
        private Level level;

        public IReadOnlyList<Entity> All => entities;
        public IReadOnlyList<Actor> Actors => actors;

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
            }
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);

            var actor = entity.GetEntityComponent<Actor>();
            if (actor) actors.Remove(actor);
        }

        public Entity Get(Vector2Int position)
        {
            return entities.Find(entity => entity.Cell == position);
        }

        public bool IsWalkable(Vector2Int position)
        {
            var entity = Get(position);

            return entity == null || !entity.BlocksMovement;
        }
    }
}
