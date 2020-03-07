using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RogueFramework
{
    public class Inventory : AEntityComponent
    {
        [SerializeField] int size = 12;

        private List<Item> items = new List<Item>();
        private TransformChildrenTracker tracker;

        public int Size => size;
        public int Count => items.Count;
        public IReadOnlyList<Item> Items => items;

        public InventoryEvent OnItemAdded;
        public InventoryEvent OnItemRemoved;

        private void Awake()
        {
            var children = GetComponentsInChildren<Item>();
            items.AddRange(children);

            if (Count > Size) Debug.LogWarning("Inventory capacity exceeded");

            tracker = GetComponent<TransformChildrenTracker>();
            if (tracker == null)
            {
                tracker = gameObject.AddComponent<TransformChildrenTracker>();
                tracker.hideFlags = HideFlags.HideInInspector;
            }

            tracker.OnChildAdded.AddListener(OnChildAdded);
            tracker.OnChildRemoved.AddListener(OnChildRemoved);
        }

        private void OnDestroy()
        {
            if (tracker)
            {
                tracker.OnChildAdded.RemoveListener(OnChildAdded);
                tracker.OnChildRemoved.RemoveListener(OnChildRemoved);
            }
        }

        public bool Add(Item item, bool force = false)
        {
            if (!items.Contains(item))
            {
                if (Count < Size || force)
                {
                    item.Entity.transform.SetParent(transform);
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

        public bool Contains(Item item)
        {
            return items.Contains(item);
        }

        public bool Drop(Item item)
        {
            if (items.Contains(item))
            {
                item.Entity.transform.SetParent(null);
                item.Entity.gameObject.SetActive(true);

                Entity.Level.Entities.Add(item.Entity);
                item.Entity.Position = Entity.Position;
                return true;
            }

            return false;
        }

        private void OnChildAdded(Transform child)
        {
            var item = child.GetComponent<Item>();

            if (item != null && items.Contains(item) == false)
            {
                item.Entity.transform.localPosition = Vector3.zero;
                item.Entity.gameObject.SetActive(false);

                items.Add(item);

                OnItemAdded.Invoke(item);

                Debug.Log($"Item added {item.name}");
            }
        }

        private void OnChildRemoved(Transform child)
        {
            var item = child.GetComponent<Item>();

            if (item != null && items.Remove(item))
            {
                OnItemRemoved.Invoke(item);

                Debug.Log($"Item removed {item.name}");
            }
        }

        [System.Serializable] public class InventoryEvent : UnityEvent<Item> { }
    }
}
