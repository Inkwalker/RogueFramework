using System.Collections;
using UnityEngine;

namespace RogueFramework.Demo
{
    public class PlayerSpawner : AEntityComponent
    {
        [SerializeField] Entity playerPrefab = default;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            if (SpawnFromBuffer() == false)
                SpawnPrefab();
        }

        private bool SpawnFromBuffer()
        {
            var entities = EntityBuffer.GetEntities();

            foreach (var entity in entities)
            {
                var player = entity.GetEntityComponent<HeroActor>();

                if (player != null)
                {
                    Vector2Int cell = FindWalkableCell();
                    EntityBuffer.Pop(entity, Entity.Level, cell);

                    return true;
                }
            }

            return false;
        }

        private void SpawnPrefab()
        {
            if (playerPrefab == null) return;

            var player = Instantiate(playerPrefab);
            Entity.Level.Entities.Add(player);
            player.Cell = FindWalkableCell();
        }

        private Vector2Int FindWalkableCell()
        {
            Vector2Int cell = Entity.Cell;

            if (Entity.BlocksMovement)
            {
                var neighbors = MapUtils.GetNeighborCells(cell, true);

                for (int i = 0; i < neighbors.Length; i++)
                {
                    if (Entity.Level.IsWalkable(neighbors[i]))
                    {
                        cell = neighbors[i];
                        break;
                    }
                }
            }

            return cell;
        }
    }
}
