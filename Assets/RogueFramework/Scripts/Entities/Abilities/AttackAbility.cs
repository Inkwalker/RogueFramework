using UnityEngine;

namespace RogueFramework
{
    public class AttackAbility : AEntityAbility
    {
        [SerializeField] StatType attackStat  = null;
        [SerializeField] StatType defenceStat = null;
        [SerializeField] StatType healthStat  = null;

        public override AbilitySignature Signature => AbilitySignature.Attack;

        private void Awake()
        {
            if (attackStat == null || defenceStat == null || healthStat == null) Debug.LogError("Stat Types are not set", this);
        }

        public override bool CanPerform(Actor user, Vector2Int tile)
        {
            return base.CanPerform(user, tile) && user.Entity.Level?.Entities.Get(tile) != null;
        }

        protected override ActorActionResult OnPerform(Actor user, Entity targetEntity, Vector2Int targetTile)
        {
            if (targetEntity == null)
            {
                targetEntity = user.Entity.Level?.Entities.Get(targetTile);
            }

            if (targetEntity != null)
            {
                var stats = user.Entity.GetEntityComponent<EntityStats>();

                int attack = stats ? stats.GetModifiedStat(attackStat) : 0;

                var targetStats = targetEntity.GetEntityComponent<EntityStats>();
                var targetHealthStat = targetStats?.GetRawStat(healthStat);

                int targetDefence = targetStats ? targetStats.GetModifiedStat(defenceStat) : 0;

                int damage = attack - targetDefence;
                if (damage < 0) damage = 0;

                if (targetHealthStat)
                {
                    targetHealthStat.Value -= damage;
                    Log(user.Entity.name, targetEntity.name, damage);
                }
            }

            return null;
        }

        private void Log(string userName, string targetName, int damage)
        {
            LogView.Log($"{userName} deals {damage} damage to {targetName}");
        }
        
    }
}
