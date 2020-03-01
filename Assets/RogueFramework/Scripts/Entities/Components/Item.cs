using UnityEngine;
using System.Collections;

namespace RogueFramework
{
    public class Item : AEntityComponent
    {
        [SerializeField] ItemType type = default;

        public ItemType Type => type;
        public int Quantity { get; set; }

        public bool InInventory => Entity.Level == null;
    }
}
