using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class DelayedActionResult : ActorActionResult
    {
        private bool waitsResult = true;
        private ActorActionResult result;

        public bool WaitsResult => waitsResult;

        public override bool Finished
        {
            get
            {
                if (waitsResult)
                    return false;
                else if (result == null)
                    return true;
                else
                    return result.Finished;
            }
        }

        public void SetResult(ActorActionResult result)
        {  
            this.result = result;

            waitsResult = false;
        }
    }
}
