using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] MapTile wallTile  = default;
        [SerializeField] MapTile floorTile = default;

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
                    var position = new Vector2Int(x + 1, y + 1);

                    bool isWall = x == 0 || y == 0 || x == size.x - 1 || y == size.y - 1;

                    var cell = isWall ? wallTile : floorTile;

                    map.Set(position, cell);
                }
            }

            map.Set(new Vector2Int(4, 4), wallTile);
            map.Set(new Vector2Int(4, 5), wallTile);
            map.Set(new Vector2Int(5, 4), wallTile);
            map.Set(new Vector2Int(5, 5), wallTile);

            GetComponent<Level>()?.FoV.Clear();
        }
    }
}
