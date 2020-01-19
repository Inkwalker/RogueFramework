using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

namespace RogueFramework
{
    public class MapRenderer : MonoBehaviour
    {
        [SerializeField] Level level = default;

        private Tilemap tilemap;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        private void OnEnable()
        {
            level.Map.onCellChanged += OnCellChanged;
        }

        private void OnDisable()
        {
            if (level != null)
                level.Map.onCellChanged -= OnCellChanged;
        }

        private void OnCellChanged(Vector3Int position)
        {
            var cell = level.Map.Get(position);
            var cell2D = cell as Cell2D;

            var tilePosition = new Vector3Int(position.x, position.y, -position.z);

            tilemap.SetTile(tilePosition, cell2D.Tile);
        }
    }
}
