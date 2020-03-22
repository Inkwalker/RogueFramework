using UnityEngine;
using System.Collections.Generic;
using System;

namespace RogueFramework
{
    public class EntityBuffer : MonoBehaviour
    {
        private static EntityBuffer instance;
        private static EntityBuffer Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = new GameObject("Entity Buffer");
                    obj.AddComponent<TransformChildrenTracker>();
                    instance = obj.AddComponent<EntityBuffer>();

                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }

        public static event Action<Entity> onEntityAdded;
        public static event Action<Entity> onEntityRemoved;

        private TransformChildrenTracker tracker;
        private List<Entity> entities = new List<Entity>();

        private void Awake()
        {
            tracker = GetComponent<TransformChildrenTracker>();
            tracker.OnChildAdded.AddListener(OnChildAdded);
            tracker.OnChildRemoved.AddListener(OnChildRemoved);
        }

        private void Start()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnChildAdded(Transform child)
        {
            var entity = child.GetComponent<Entity>();

            if (entity != null && !entities.Contains(entity))
            {
                entities.Add(entity);

                onEntityAdded?.Invoke(entity);
            }
        }

        private void OnChildRemoved(Transform child)
        {
            var entity = child.GetComponent<Entity>();

            if (entity != null && entities.Remove(entity))
            {
                onEntityRemoved?.Invoke(entity);
            }
        }

        public static bool Contains(Entity entity)
        {
            return Instance.entities.Contains(entity);
        }

        public static Entity[] GetEntities()
        {
            return Instance.entities.ToArray();
        }

        public static void Push(Entity entity)
        {
            if (Instance.entities.Contains(entity) == false)
                Instance.entities.Add(entity);

            entity.transform.SetParent(Instance.transform);
            entity.transform.position = Vector3.zero;
            entity.gameObject.SetActive(false);
        }

        public static void Pop(Entity entity, Level level, Vector2Int cell)
        {
            Instance.entities.Remove(entity);

            level.Entities.Add(entity);
            entity.Cell = cell;
            entity.gameObject.SetActive(true);
        }
    }
}
