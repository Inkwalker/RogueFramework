using UnityEngine;
using System.Collections.Generic;

namespace RogueFramework
{
    public abstract class TransformBehaviour<T> : MonoBehaviour where T : Component
    {
        List<T> children = new List<T>();
        List<T> buffer = new List<T>();

        public IReadOnlyList<T> Children => children;

        protected virtual void Awake()
        {
            FetchChildren();
        }

        private void OnTransformChildrenChanged()
        {
            buffer.AddRange(children);
            FetchChildren();

            foreach (var item in buffer)
            {
                if (children.Contains(item) == false)
                {
                    OnChildRemoved(item);
                }
            }

            foreach (var item in children)
            {
                if (buffer.Contains(item) == false)
                {
                    OnChildAdded(item);
                }
            }

            buffer.Clear();          
        }

        private void FetchChildren()
        {
            children.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var comp = child.GetComponent<T>();

                if (comp != null)
                    children.Add(comp);
            }
        }

        protected abstract void OnChildAdded(T child);
        protected abstract void OnChildRemoved(T child);
    }
}
