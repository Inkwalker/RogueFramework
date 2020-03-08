using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework
{
    [CreateAssetMenu]
    public class ItemType : ScriptableObject
    {
        [SerializeField] Item prefab = default;
        [SerializeField] Sprite icon = null;
        [SerializeField] string displayName = "NONE";

        [SerializeField] List<EquipmentSlot> equippableInSlots = new List<EquipmentSlot>();

        public Item Prefab => prefab;
        public Sprite Icon => icon;
        public string DisplayName => displayName;
        public bool Equippable => equippableInSlots.Count > 0;
        public IReadOnlyList<EquipmentSlot> EquippableInSlots => equippableInSlots;

        public bool CanBeEquippedIn(EquipmentSlot slot)
        {
            return equippableInSlots.Contains(slot);
        }
    }
}