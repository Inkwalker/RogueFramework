using System;
using System.Collections.Generic;

namespace RogueFramework
{
    public class ItemDropAction : ActorAction
    {
        private Item itemToDrop;

        public ItemDropAction(Actor actor, Item item) : base(actor)
        {
            itemToDrop = item;
        }

        public override int Cost => 1;

        protected override ActorActionResult OnPerform()
        {
            var inventory = Actor.Entity.GetEntityComponent<Inventory>();
            if (inventory != null)
            {
                if (inventory.Drop(itemToDrop) == false)
                {
                    itemToDrop.Entity.gameObject.SetActive(true);

                    Actor.Entity.Level.Entities.Add(itemToDrop.Entity);
                    itemToDrop.Entity.Position = Actor.Entity.Position;
                }
            }

            return null;
        }
    }
}
