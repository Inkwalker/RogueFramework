using UnityEngine;

namespace RogueFramework
{
    public class StatRegenerationAbility : AEntityAbility
    {
        [SerializeField] StatType targetStatType = null;
        [SerializeField] int regenerationAmmount = 1;

        private void Awake()
        {
            if (targetStatType == null) Debug.LogError("Target stat type is not set", this);
        }

        public override AbilitySignature Signature => AbilitySignature.Undefined;

        public override bool CanPerform(Actor user, Entity target)
        {
            return false;
        }

        public override bool CanPerform(Actor user, Vector2Int tile)
        {
            return false;
        }

        protected override ActorActionResult OnPerform(Actor user, Entity targetEntity, Vector2Int targetTile)
        {
            return null;
        }

        public override void Tick(Entity target)
        {
            var targetStats = target.GetEntityComponent<EntityStats>();

            if (targetStats)
            {
                var stat = targetStats.GetRawStat(targetStatType);

                if (stat)
                {
                    stat.Value += regenerationAmmount;
                }
            }
        }
    }
}