using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RogueFramework
{
    public class Equipment : AEntityComponent
    {
        [SerializeField] EquipmentSlot[] slots;

        private List<Item> equipped = new List<Item>();

        public IReadOnlyList<Item> Equipped => equipped;

        private void Awake()
        {
            var children = GetComponentsInChildren<Item>();
            equipped.AddRange(children);
        }

        private void OnTransformChildrenChanged()
        {
            equipped.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var item = child.GetComponent<Item>();

                if (item != null)
                {
                    item.Entity.transform.localPosition = Vector3.zero;
                    item.Entity.gameObject.SetActive(true);

                    equipped.Add(item);
                }
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
    }
}
