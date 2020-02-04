using UnityEngine;

namespace RogueFramework
{
    public class FieldOfViewUpdater : AEntityComponent
    {
        [SerializeField] int viewDistance = 10;
        [SerializeField] bool explore = true;

        public override void OnTick()
        {
            Entity.Level.FoV.AppendFoV(Entity.Cell, viewDistance, explore);
        }
    }
}
