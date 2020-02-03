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
            return Entity.Level.Map.IsWalkable(cell) && Entity.Level.Entities.IsWalkable(cell);
        }

        public override void OnTick()
        {
            AddEnergy(speed);
        }
    }
}