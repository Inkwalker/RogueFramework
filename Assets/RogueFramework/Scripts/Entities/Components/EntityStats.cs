using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RogueFramework
{
    public class EntityStats : AEntityComponent
    {
        [SerializeField] Transform[] externalContainers = null;

        private TransformChildrenTracker tracker;
        private List<EntityStat> stats = new List<EntityStat>();
        private List<EntityStatModifier> modifiers = new List<EntityStatModifier>();
        private List<EntityStats> childEntities = new List<EntityStats>();

        public UnityEvent OnStatsChanged = new UnityEvent();

        private void Awake()
        {
            AddStatsAndMods(transform);

            tracker = TransformChildrenTracker.GetOrCreate(gameObject);

            tracker.OnChildAdded.AddListener(OnChildAdded);
            tracker.OnChildRemoved.AddListener(OnChildRemoved);

            foreach (var container  in externalContainers)
            {
                AddStatsAndMods(container);

                var externalTracker = TransformChildrenTracker.GetOrCreate(container.gameObject);

                externalTracker.OnChildAdded.AddListener(OnChildAdded);
                externalTracker.OnChildRemoved.AddListener(OnChildRemoved);
            }
        }

        private void OnDestroy()
        {
            foreach (var container in externalContainers)
            {
                if (container != null)
                {
                    var externalTracker = container.GetComponent<TransformChildrenTracker>();
                    if (externalTracker)
                    {
                        externalTracker.OnChildAdded.RemoveListener(OnChildAdded);
                        externalTracker.OnChildRemoved.RemoveListener(OnChildRemoved);
                    }
                }
            }
        }

        public EntityStat GetRawStat(StatType type)
        {
            return stats.Find(s => s.Type == type);
        }

        public int GetModifiedStat(StatType type)
        {
            var stat = GetRawStat(type);

            int value = stat ? stat.Value : 0;

            foreach (var mod in modifiers)
            {
                if (mod.Type == type)
                    value = mod.Modify(value);
            }

            foreach (var entity in childEntities)
            {
                foreach (var mod in entity.modifiers)
                {
                    if (mod.Type == type)
                        value = mod.Modify(value);
                }
            }

            return value;
        }

        private void OnChildAdded(Transform child)
        {
            bool statsChanged = false;
            var entity = child.GetComponent<Entity>();

            if (entity != null)
            {
                var childStats = entity.GetEntityComponent<EntityStats>();
                if (childStats)
                {
                    childEntities.Add(childStats);
                    statsChanged = true;
                }
            }
            else
            {
                var stat = child.GetComponent<EntityStat>();
                if (stat != null && !stats.Contains(stat))
                {
                    Debug.Log(Entity.name + " | Stat added: " + stat.Type.name, stat);
                    stats.Add(stat);
                    statsChanged = true;
                }

                var mod = child.GetComponent<EntityStatModifier>();
                if (mod != null && !modifiers.Contains(mod))
                {
                    modifiers.Add(mod);
                    statsChanged = true;
                }
            }

            if (statsChanged)
                OnStatsChanged.Invoke();
        }

        private void OnChildRemoved(Transform child)
        {
            bool statsChanged = false;
            var entity = child.GetComponent<Entity>();

            if (entity != null)
            {
                var childStats = entity.GetEntityComponent<EntityStats>();
                if (childEntities.Remove(childStats)) 
                    statsChanged = true;
            }
            else
            {
                var stat = child.GetComponent<EntityStat>();
                if (stat != null)
                {
                    if (stats.Remove(stat))
                        statsChanged = true;
                }

                var mod = child.GetComponent<EntityStatModifier>();
                if (mod != null)
                {
                    if (modifiers.Remove(mod)) 
                        statsChanged = true;
                }
            }

            if (statsChanged)
                OnStatsChanged.Invoke();
        }

        private void AddStatsAndMods(Transform container)
        {
            for (int i = 0; i < container.childCount; i++)
            {
                var child = container.GetChild(i);

                OnChildAdded(child);
            }
        }
    }
}
