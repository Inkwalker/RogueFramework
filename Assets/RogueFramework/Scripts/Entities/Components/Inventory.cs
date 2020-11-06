using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RogueFramework
{
    public class Inventory : AEntityComponent
    {
        [SerializeField] int size = 12;

        [SerializeField] InventoryEvent onItemAdded = new InventoryEvent();
        [SerializeField] InventoryEvent onItemRemoved = new InventoryEvent();

        private List<Item> items = new List<Item>();
        private TransformChildrenTracker tracker;

        public int Size => size;
        public int Count => items.Count;
        public IReadOnlyList<Item> Items => items;

        public InventoryEvent OnItemAdded => onItemAdded;
        public InventoryEvent OnItemRemoved => onItemRemoved;

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
                    item.Entity.gameObject.SetActive(false);
                    item.Entity.transform.SetParent(transform);
                    item.Entity.transform.localPosition = Vector3.zero;
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
                items.Add(item);

                item.OnInventoryChanged(this);
                OnItemAdded.Invoke(item);

                Debug.Log($"{Entity.name} | Item added: {item.name}", this);
            }
        }

        private void OnChildRemoved(Transform child)
        {
            var item = child.GetComponent<Item>();

            if (item != null && items.Remove(item))
            {
                item.OnInventoryChanged(null);
                OnItemRemoved.Invoke(item);

                Debug.Log($"{Entity.name} | Item removed: {item.name}", this);
            }
        }

        [System.Serializable] public class InventoryEvent : UnityEvent<Item> { }
    }
}
