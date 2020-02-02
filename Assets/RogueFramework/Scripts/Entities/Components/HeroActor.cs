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

        private void Update()
        {
            if (activeAction != null && activeAction.WaitsResult)
            {

                Vector2Int? delta = null;

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    delta = Vector2Int.left;
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    delta = Vector2Int.right;
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    delta = Vector2Int.up;
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    delta = Vector2Int.down;
                }

                if (delta.HasValue)
                {
                    var action = new WalkAction(this, delta.Value);
                    activeAction.SetResult(action.Perform());
                }
            }
        }
    }
}
