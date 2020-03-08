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

        public virtual bool CanPerform(Actor user, Entity target)
        {
            return user != null && Owner != null && target != null;
        }
        public virtual bool CanPerform(Actor user, Vector2Int tile)
        {
            return user != null && Owner != null;
        }

        public ActorActionResult Perform(Actor user, Entity target)
        {
            if (CanPerform(user, target))
            {
                user.Energy -= Cost;
                return OnPerform(user, target, target.Cell);
            }

            return null;
        }
        public ActorActionResult Perform(Actor user, Vector2Int tile)
        {
            if (CanPerform(user, tile))
            {
                user.Energy -= Cost;
                return OnPerform(user, null, tile);
            }

            return null;
        }

        protected abstract ActorActionResult OnPerform(Actor user, Entity targetEntity, Vector2Int targetTile);
    }
}
