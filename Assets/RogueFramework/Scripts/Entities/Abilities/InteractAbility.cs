using UnityEngine;

namespace RogueFramework
{
    public class InteractAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Interaction;

        public override bool CanPerform(Entity target)
        {
            return base.CanPerform(target) && target.GetEntityComponent<Interactable>() != null;
        }

        public override bool CanPerform(Vector2Int tile)
        {
            return base.CanPerform(tile) && Owner.Level.Entities.Get<Interactable>(tile) != null;
        }

        protected override ActorActionResult OnPerform(Entity targetEntity, Vector2Int targetTile)
        {
            Interactable interactable = null;

            if (targetEntity == null)
            {
                interactable = Owner.Level.Entities.Get<Interactable>(targetTile);
            }
            else
            {
                interactable = targetEntity.GetEntityComponent<Interactable>();
            }

            if (interactable != null)
            {
                Debug.Log($"Interacting with {interactable.Entity.name}");
                interactable.Interact(Actor);
            }

            return null;
        }
    }
}
