using UnityEngine;
using System.Collections.Generic;

namespace RogueFramework
{
    public class Inventory : AEntityComponent
    {
        [SerializeField] int size = 12;

        private List<Item> items = new List<Item>();

        public int Size => size;
        public int Count => items.Count;

        private void Awake()
        {
            var children = GetComponentsInChildren<Item>();
            items.AddRange(children);

            if (Count > Size) Debug.LogWarning("Inventory capacity exceeded");
        }

        public bool Add(Item item, bool force = false)
        {
            if (!items.Contains(item))
            {
                if (Count < Size || force)
                {
                    item.Entity.Level.Entities.Remove(item.Entity);

                    item.Entity.transform.SetParent(transform);
                    item.Entity.transform.localPosition = Vector3.zero;

                    item.Entity.gameObject.SetActive(false);
                }
                else return false;
            }

            return true;
        }

        public Item Get(ItemType type)
        {
            return items.Find(item => item.Type == type);
        }

        public bool Remove(Item item)
        {
            if (items.Remove(item))
            {
                item.Entity.transform.SetParent(null);
                return true;
            }

            return false;
        }

        public bool Drop(Item item)
        {
            if (items.Remove(item))
            {
                item.Entity.gameObject.SetActive(true);

                Entity.Level.Entities.Add(item.Entity);
                item.Entity.Position = Entity.Position;
                return true;
            }

            return false;
        }
    }
}
