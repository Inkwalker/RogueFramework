using UnityEngine;

namespace RogueFramework
{
    public abstract class AEntityStat : MonoBehaviour
    {
        private Entity owner;

        public Entity Owner
        {
            get { if (owner == null) owner = GetComponentInParent<Entity>(); return owner; }
            set { owner = value; }
        }

        private void OnTransformParentChanged()
        {
            Owner = GetComponentInParent<Entity>();
        }
    }
}
