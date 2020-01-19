using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class Level : MonoBehaviour
    {
        public Map Map { get; private set; }

        private void Awake()
        {
            Map = new Map();
        }
    }
}
