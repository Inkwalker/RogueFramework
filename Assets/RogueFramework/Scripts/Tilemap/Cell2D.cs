using UnityEngine;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    [CreateAssetMenu]
    public class Cell2D : ACell
    {
        [SerializeField] Tile tile = default;

        public Tile Tile => tile;
    }
}
