using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public abstract class Actor : AEntityComponent
    {
        [SerializeField] int speed = 4;

        public int Speed => speed;
        public int Energy { get; set; }

        public void AddEnergy(int quantity)
        {
            Energy += quantity;

            if (Energy > 0) Energy = 0;
        }

        public bool HasEnoughEnergy()
        {
            return Energy >= 0;
        }

        public abstract ActorActionResult TakeTurn();

        public bool CanOccupy(Vector2Int cell)
        {
            return Entity.Level.IsWalkable(cell);
        }

        public override void OnTick()
        {
            AddEnergy(speed);
        }

        protected AEntityAbility GetAbility(AbilitySignature signature)
        {
            var abilities = Entity.GetEntityComponent<EntityAbilities>();
            var ability = abilities.Get(signature);

            if (ability != null) 
                return ability;
            else
                return null;
        }

        public List<AEntityAbility> GetApplicableAbilities(Entity target)
        {
            var result = new List<AEntityAbility>();
            var abilities = Entity.GetEntityComponent<EntityAbilities>();

            foreach (var ability in abilities.Abilities)
            {
                if (ability.CanPerform(this, target))
                {
                    result.Add(ability);
                }
            }

            return result;
        }
    }
}