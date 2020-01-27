using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public abstract class AEntityComponent : MonoBehaviour
    {
        private Entity entity;
        public Entity Entity { get { if (entity == null) entity = GetComponentInParent<Entity>(); return entity; } }
    }
}
