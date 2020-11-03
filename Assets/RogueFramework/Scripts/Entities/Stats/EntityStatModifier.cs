using UnityEngine;

namespace RogueFramework
{
    public class EntityStatModifier : MonoBehaviour
    {
        [SerializeField] StatType type = null;
        [SerializeField] int value = 0;

        public StatType Type => type;
        public int Value => value;

        private void Awake()
        {
            if (type == null) Debug.LogError("Stat Type is not set", this);
        }

        public int Modify(int baseValue)
        {
            return baseValue + value;
        }
    }
}