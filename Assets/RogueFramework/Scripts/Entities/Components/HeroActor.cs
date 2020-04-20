using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class HeroActor : Actor
    {
        private DelayedActionResult activeAction;

        public override ActorActionResult TakeTurn()
        {
            activeAction = new DelayedActionResult();

            return activeAction;
        }

        private void OnDestroy()
        {
            if (activeAction != null && activeAction.WaitsResult)
            {
                activeAction.SetResult(null);
            }
        }

        private void Update()
        {
            if (activeAction != null && activeAction.WaitsResult)
            {

                Vector2Int? delta = null;

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    delta = Vector2Int.left;
                }

                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    delta = Vector2Int.right;
                }

                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                {
                    delta = Vector2Int.up;
                }

                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    delta = Vector2Int.down;
                }

                if (Input.GetKey(KeyCode.G))
                {
                    var ability = GetAbility(AbilitySignature.ItemTake);

                    if (ability != null)
                    {
                        var item = Entity.Level.Entities.Get<Item>(Entity.Cell);

                        if (item != null && ability.CanPerform(this, item.Entity))
                        {
                            var result = ability.Perform(this, item.Entity);
                            activeAction.SetResult(result);
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.H))
                {
                    var ability = GetAbility(AbilitySignature.ItemDrop);

                    if (ability != null)
                    {
                        var inv = Entity.GetEntityComponent<Inventory>();
                        if (inv != null && inv.Count > 0)
                        {
                            var item = inv.Items[0];

                            var result = ability.Perform(this, item.Entity);
                            activeAction.SetResult(result);
                        }
                    }
                }
                else if (delta.HasValue)
                {
                    var entity = Entity.Level.Entities.Get(Entity.Cell + delta.Value);

                    var interact = GetAbility(AbilitySignature.Interaction);
                    var attack   = GetAbility(AbilitySignature.Attack);
                    var move     = GetAbility(AbilitySignature.Move);

                    if (interact != null && interact.CanPerform(this, entity))
                    {
                        var result = interact.Perform(this, entity);
                        activeAction.SetResult(result);
                    }
                    else if (attack != null && attack.CanPerform(this, entity))
                    {
                        var result = attack.Perform(this, entity);
                        activeAction.SetResult(result);
                    }
                    else if (move != null)
                    {
                        var result = move.Perform(this, Entity.Cell + delta.Value);
                        activeAction.SetResult(result);
                    }                    
                }
            }
        }
    }
}
