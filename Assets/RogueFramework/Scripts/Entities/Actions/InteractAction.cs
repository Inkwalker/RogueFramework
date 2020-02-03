using UnityEngine;

namespace RogueFramework
{
    public class InteractAction : ActorAction
    {
        private Interactable interactable;

        public override int Cost => 1;

        public InteractAction(Actor actor, Interactable interactable) : base(actor)
        {
            this.interactable = interactable;
        }

        protected override ActorActionResult OnPerform()
        {
            interactable.Interact(Actor);

            return null;
        }
    }
}
