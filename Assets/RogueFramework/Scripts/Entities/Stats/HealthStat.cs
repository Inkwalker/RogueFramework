using UnityEngine;

namespace RogueFramework
{
    public class HealthStat : AEntityStat
    {
        [SerializeField] int defaultValue = 100;
        [SerializeField] int maxValue = 100;

        private int value;

        public int Value
        {
            get => value;
            set
            {
                this.value = Mathf.Clamp(value, 0, maxValue);
            }
        }

        private void Awake()
        {
            Value = defaultValue;
        }
    }
}
