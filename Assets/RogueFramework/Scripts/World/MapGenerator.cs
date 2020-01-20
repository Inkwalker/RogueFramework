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
            var map = GetComponent<Level>().Tilemap;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var position = new Vector3Int(x, y, 0);

                    bool isWall = x == 0 || y == 0 || x == size.x - 1 || y == size.y - 1;

                    var cell = isWall ? wallTile : floorTile;

                    map.SetTile(position, cell);
                }
            }

            map.SetTile(new Vector3Int(4, 4, 0), wallTile);
            map.SetTile(new Vector3Int(4, 5, 0), wallTile);
            map.SetTile(new Vector3Int(5, 4, 0), wallTile);
            map.SetTile(new Vector3Int(5, 5, 0), wallTile);
            map.SetColor(new Vector3Int(5, 5, 0), Color.gray);
            map.SetColor(new Vector3Int(6, 6, 0), Color.gray);
        }
    }
}
