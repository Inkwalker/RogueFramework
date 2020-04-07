using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace RogueFramework
{
    public class Item : AEntityComponent
    {
        [SerializeField] ItemType type = default;

        [SerializeField] ItemEvent onAddedToInventory = new ItemEvent();
        [SerializeField] ItemEvent onRemovedFromInventory = new ItemEvent();

        public ItemType Type => type;
        public int Quantity { get; set; }

        public bool InInventory => Inventory != null;
        public Inventory Inventory { get; private set; }

        public ItemEvent OnAddedToInventory => onAddedToInventory;
        public ItemEvent OnRemovedFromInventory => onRemovedFromInventory;

        public void OnInventoryChanged(Inventory inv)
        {
            if (Inventory == inv) return;

            var oldInv = Inventory;
            Inventory = inv;

            if (oldInv != null) OnRemovedFromInventory.Invoke(this);
            if (inv != null) OnAddedToInventory.Invoke(this);
        }

        [System.Serializable] public class ItemEvent : UnityEvent<Item> { }
    }
}
