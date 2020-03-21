using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RogueFramework
{
    public class Teleport : Interactable
    {
        [Tooltip("Set to null if you want to send actors to a buffer.")]
        [SerializeField] Entity exit;

        private static Transform buffer;
        private static Transform Buffer
        {
            get
            {
                if (buffer == null)
                {
                    buffer = new GameObject("Teleport Buffer").transform;
                    DontDestroyOnLoad(buffer);
                }

                return buffer;
            }
        }

        public TeleportEvent onEntityTeleported;
        public TeleportEvent onEntityAddedToBuffer;

        public override void Interact(Actor actor)
        {
            if (exit == null)
                SendToBuffer(actor.Entity);
            else
                SendToExit(actor.Entity, exit);
        }

        private void SendToBuffer(Entity entity)
        {
            entity.transform.SetParent(Buffer);
            entity.transform.position = Vector3.zero;
            entity.gameObject.SetActive(false);

            onEntityAddedToBuffer?.Invoke(entity);
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

            onEntityTeleported?.Invoke(entity);
        }

        [System.Serializable]
        public class TeleportEvent : UnityEvent<Entity> { }
    }
}
