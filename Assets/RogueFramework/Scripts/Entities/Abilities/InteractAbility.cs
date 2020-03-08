using UnityEngine;

namespace RogueFramework
{
    public class InteractAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Interaction;

        public override bool CanPerform(Actor user, Entity target)
        {
            return base.CanPerform(user, target) && target.GetEntityComponent<Interactable>() != null;
        }

        public override bool CanPerform(Actor user, Vector2Int tile)
        {
            return base.CanPerform(user, tile) && user.Entity.Level.Entities.Get<Interactable>(tile) != null;
        }

        protected override ActorActionResult OnPerform(Actor user, Entity targetEntity, Vector2Int targetTile)
        {
            Interactable interactable = null;

            if (targetEntity == null)
            {
                interactable = user.Entity.Level.Entities.Get<Interactable>(targetTile);
            }
            else
            {
                interactable = targetEntity.GetEntityComponent<Interactable>();
            }

            if (interactable != null)
            {
                Debug.Log($"Interacting with {interactable.Entity.name}");
                interactable.Interact(user);
            }

            return null;
        }
    }
}
