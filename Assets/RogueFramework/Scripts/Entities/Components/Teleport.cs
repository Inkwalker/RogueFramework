using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RogueFramework
{
    public class Teleport : Interactable
    {
        [Tooltip("Set to null if you want to send actors to a buffer.")]
        [SerializeField] Entity exit = default;

        public TeleportEvent onEntityTeleported;

        public override void Interact(Actor actor)
        {
            if (exit == null)
                EntityBuffer.Push(actor.Entity);
            else
                SendToExit(actor.Entity, exit);

            onEntityTeleported?.Invoke(actor.Entity);
        }

        private void SendToExit(Entity entity, Entity exit)
        {
            var targetEnitiyIndex = exit.Level?.Entities;

            if (targetEnitiyIndex != null)
            {
                targetEnitiyIndex.Add(entity);
            }
            else
            {
                if (exit.transform.parent != null)
                {
                    entity.transform.SetParent(exit.transform.parent);
                }
                else
                {
                    SceneManager.MoveGameObjectToScene(entity.gameObject, exit.gameObject.scene);
                }
            }

            Vector2Int position = exit.Cell;
            if (exit.BlocksMovement)
            {
                var neighbors = MapUtils.GetNeighborCells(position, true);

                position = neighbors[Random.Range(0, neighbors.Length)];
            }

            entity.Cell = position;
        }

        [System.Serializable]
        public class TeleportEvent : UnityEvent<Entity> { }
    }
}
