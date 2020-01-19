using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] ACell wallCell  = default;
        [SerializeField] ACell floorCell = default;

        private void Start()
        {
            Generate(new Vector2Int(10, 10));   
        }

        private void Generate(Vector2Int size)
        {
            var map = GetComponent<Level>().Map;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var position = new Vector3Int(x, y, 0);

                    bool isWall = x == 0 || y == 0 || x == size.x - 1 || y == size.y - 1;

                    var cell = isWall ? wallCell : floorCell;

                    map.Set(position, cell);
                }
            }
        }
    }
}
