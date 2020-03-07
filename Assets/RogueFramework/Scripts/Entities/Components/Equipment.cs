using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RogueFramework
{
    public class Equipment : AEntityComponent
    {
        [SerializeField] EquipmentSlot[] slots;

        private List<Item> equipped = new List<Item>();
        private TransformChildrenTracker tracker;

        public IReadOnlyList<Item> Equipped => equipped;

        public EquipmentEvent OnEquipped;
        public EquipmentEvent OnUnequipped;

        private void Awake()
        {
            var children = GetComponentsInChildren<Item>();
            equipped.AddRange(children);

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
            if (equipped.Contains(item))
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


        private void OnChildAdded(Transform child)
        {
            var item = child.GetComponent<Item>();

            if (item != null && !equipped.Contains(item))
            {
                item.Entity.transform.localPosition = Vector3.zero;
                item.Entity.gameObject.SetActive(true);

                equipped.Add(item);

                OnEquipped.Invoke(item);
            }
        }

        private void OnChildRemoved(Transform child)
        {
            var item = child.GetComponent<Item>();
            if (item != null && equipped.Remove(item))
            {
                OnUnequipped.Invoke(item);
            }
        }

        [System.Serializable] public class EquipmentEvent : UnityEvent<Item> { }
    }
}
