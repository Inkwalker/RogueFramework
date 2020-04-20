using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class AttackAbility : AEntityAbility
    {
        public override AbilitySignature Signature => AbilitySignature.Attack;

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
                var attack = stats?.Get<AttackStat>();

                var targetStats = targetEntity.GetEntityComponent<EntityStats>();
                var targetHP = targetStats?.Get<HealthStat>();

                if (attack && targetHP)
                {
                    targetHP.Value -= attack.Value;

                    LogView.Log($"{user.Entity.name} deals {attack.Value} damage to {targetEntity.name}");
                }
            }

            return null;
        }
        
    }
}
