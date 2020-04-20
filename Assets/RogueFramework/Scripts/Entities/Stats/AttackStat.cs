using UnityEngine;

namespace RogueFramework
{
    public class AttackStat : AEntityStat
    {
        [SerializeField] int defaultValue = 0;

        private int value;

        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                if (this.value < 0) this.value = 0;
            }
        }

        private void Awake()
        {
            Value = defaultValue;
        }
    }
}
