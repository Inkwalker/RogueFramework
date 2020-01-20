using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class EntityList
    {
        private List<Entity> entities = new List<Entity>();
        private Level level;

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
            }
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);
        }

        public Entity Get(Vector2Int position)
        {
            return entities.Find(entity => entity.Cell == position);
        }
    }
}
