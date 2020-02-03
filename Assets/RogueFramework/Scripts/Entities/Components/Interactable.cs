using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public abstract class Interactable : AEntityComponent
    {
        public abstract void Interact(Actor actor);
    }
}
