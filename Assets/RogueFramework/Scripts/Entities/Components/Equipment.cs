using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RogueFramework
{
    public class Equipment : AEntityComponent
    {
        [SerializeField] EquipmentSlot[] slots;

        private List<Item> equipped = new List<Item>();

        public IReadOnlyList<Item> Equipped => equipped;

        public EquipmentEvent OnEquipped;
        public EquipmentEvent OnUnequipped;

        private void Awake()
        {
            var children = GetComponentsInChildren<Item>();
            equipped.AddRange(children);
        }

        private void OnTransformChildrenChanged()
        {
            var newEquippedList = new List<Item>();
            var addedEquipment = new List<Item>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var item = child.GetComponent<Item>();

                if (item != null)
                {
                    if (!equipped.Remove(item))
                    {
                        item.Entity.transform.localPosition = Vector3.zero;
                        item.Entity.gameObject.SetActive(true);

                        addedEquipment.Add(item);
                    }
                    newEquippedList.Add(item);
                }
            }

            equipped = newEquippedList;

            foreach (var item in equipped)
            {
                OnUnequipped.Invoke(item);
            }
            foreach (var item in addedEquipment)
            {
                OnEquipped.Invoke(item);
            }
        }

        public bool Add(Item item, bool force = false)
        {
            if (!equipped.Contains(item))
            {
                item.Entity.transform.SetParent(transform);
                
                return true;
            }

            return false;
        }

        public Item Get(ItemType type)
        {
            return equipped.Find(item => item.Type == type);
        }

        public bool Remove(Item item)
        {
            if (equipped.Remove(item))
            {
                item.Entity.transform.SetParent(null);
                return true;
            }

            return false;
        }

        public bool Contains(Item item)
        {
            return equipped.Contains(item);
        }

        [System.Serializable] public class EquipmentEvent : UnityEvent<Item> { }
    }
}
