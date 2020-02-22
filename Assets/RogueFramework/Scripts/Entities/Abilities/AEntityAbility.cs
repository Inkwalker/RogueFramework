using UnityEngine;

namespace RogueFramework
{
    public abstract class AEntityAbility : MonoBehaviour
    {
        [SerializeField] int cost = 1;

        private Entity owner;

        public abstract AbilitySignature Signature { get; }
        public int Cost => cost;
        public Entity Owner
        {
            get { if (owner == null) owner = GetComponentInParent<Entity>(); return owner; }
            set { owner = value; }
        }
        public Actor Actor
        {
            get { return Owner?.GetEntityComponent<Actor>(); }
        }

        private void OnTransformParentChanged()
        {
            Owner = GetComponentInParent<Entity>();
            OnOwnerChanged();
        }

        protected virtual void OnOwnerChanged() { }

        public virtual bool CanPerform(Entity target)
        {
            return Owner != null && target != null;
        }
        public virtual bool CanPerform(Vector2Int tile)
        {
            return Owner != null;
        }

        public ActorActionResult Perform(Entity target)
        {
            if (CanPerform(target))
            {
                Actor.Energy -= Cost;
                return OnPerform(target, target.Cell);
            }

            return null;
        }
        public ActorActionResult Perform(Vector2Int tile)
        {
            if (CanPerform(tile))
            {
                Actor.Energy -= Cost;
                return OnPerform(null, tile);
            }

            return null;
        }

        protected abstract ActorActionResult OnPerform(Entity targetEntity, Vector2Int targetTile);
    }
}
