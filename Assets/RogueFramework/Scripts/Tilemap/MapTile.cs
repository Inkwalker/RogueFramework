using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    [CreateAssetMenu]
    public class MapTile : Tile
    {
        [SerializeField] bool walkable = false;
        [SerializeField] bool transparent = false;

        public bool Walkable => walkable;
        public bool Transparent => transparent;
    }
}
