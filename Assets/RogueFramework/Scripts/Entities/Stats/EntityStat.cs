using UnityEngine;

namespace RogueFramework
{
    public class EntityStat : MonoBehaviour
    {
        [SerializeField] StatType type = null;

        [SerializeField] int defaultValue = 100;

        [SerializeField] bool hasCap = false;
        [SerializeField] int capValue = 100;

        private Entity owner;
        private int value;

        public StatType Type => type;

        public int Value
        {
            get => value;
            set
            {
                this.value = Mathf.Clamp(value, 0, hasCap ? capValue : int.MaxValue);
            }
        }

        public Entity Owner
        {
            get { if (owner == null) owner = GetComponentInParent<Entity>(); return owner; }
            set { owner = value; }
        }

        private void Awake()
        {
            if (type == null) Debug.LogError("Stat Type is not set", this);

            Value = defaultValue;
        }

        private void OnTransformParentChanged()
        {
            Owner = GetComponentInParent<Entity>();
        }
    }
}
