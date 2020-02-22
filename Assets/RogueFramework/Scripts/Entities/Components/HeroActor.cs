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


                ActorAction action = null;

                if (Input.GetKey(KeyCode.G))
                {
                    var item = Entity.Level.Entities.Get<Item>(Entity.Cell);

                    if (item != null)
                    {
                        action = new GrabAction(this, item);
                    }
                }
                else if (Input.GetKey(KeyCode.H))
                {
                    var inv = Entity.GetEntityComponent<Inventory>();
                    if (inv!= null && inv.Count > 0)
                    {
                        var item = inv.Items[0];

                        action = new ItemDropAction(this, item);
                    }
                }
                else if (delta.HasValue)
                {
                    var entity = Entity.Level.Entities.Get(Entity.Cell + delta.Value);
                    var interactable = entity?.GetEntityComponent<Interactable>();

                    if (interactable != null)
                    {
                        action = new InteractAction(this, interactable);
                    }
                    else
                    {
                        action = new WalkAction(this, delta.Value);
                    }
                }

                if (action != null)
                {
                    activeAction.SetResult(action.Perform());
                }
            }
        }
    }
}
