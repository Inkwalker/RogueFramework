using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class Item : AEntityComponent
    {
        [SerializeField] ItemType type;

        public ItemType Type => type;
        public int Quantity { get; set; }
    }
}
