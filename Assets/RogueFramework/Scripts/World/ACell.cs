using UnityEngine;

namespace RogueFramework
{
    /// <summary>
    /// Map cell
    /// </summary>
    public abstract class ACell : ScriptableObject
    {
        [SerializeField] bool walkable = false;
        [SerializeField] bool transparent = false;

        public bool Walkable => walkable;
        public bool Transparent => transparent;
    }
}
