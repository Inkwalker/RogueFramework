using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RogueFramework
{
    public class TransformChildrenTracker : MonoBehaviour
    {
        List<Transform> children = new List<Transform>();
        List<Transform> buffer = new List<Transform>();

        public TrackerEvent OnChildAdded   = new TrackerEvent();
        public TrackerEvent OnChildRemoved = new TrackerEvent();

        public static TransformChildrenTracker GetOrCreate(GameObject gameObject)
        {
            var tracker = gameObject.GetComponent<TransformChildrenTracker>();
            if (tracker == null)
            {
                tracker = gameObject.AddComponent<TransformChildrenTracker>();
                tracker.hideFlags = HideFlags.HideInInspector;
            }

            return tracker;
        }

        private void OnEnable()
        {
            FetchChildren();
        }

        private void OnTransformChildrenChanged()
        {
            if (enabled)
            {
                buffer.AddRange(children);
                FetchChildren();

                foreach (var item in buffer)
                {
                    if (children.Contains(item) == false)
                    {
                        OnChildRemoved.Invoke(item);
                    }
                }

                foreach (var item in children)
                {
                    if (buffer.Contains(item) == false)
                    {
                        OnChildAdded.Invoke(item);
                    }
                }

                buffer.Clear();
            }
        }

        private void FetchChildren()
        {
            children.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
        }

        [System.Serializable] public class TrackerEvent : UnityEvent<Transform> { }
    }
}
