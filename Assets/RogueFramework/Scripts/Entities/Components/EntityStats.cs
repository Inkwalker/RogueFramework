using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RogueFramework
{
    public class EntityStats : AEntityComponent
    {
        private TransformChildrenTracker tracker;
        private List<AEntityStat> stats = new List<AEntityStat>();

        public List<AEntityStat> Stats => new List<AEntityStat>(stats);

        public UnityEvent OnStatsChanged = new UnityEvent();

        private void Awake()
        {
            tracker = TransformChildrenTracker.GetOrCreate(gameObject);

            tracker.OnChildAdded.AddListener(OnChildAdded);
            tracker.OnChildRemoved.AddListener(OnChildRemoved);
        }

        private void OnChildAdded(Transform child)
        {
            var stat = child.GetComponent<AEntityStat>();
            if (stat != null && !stats.Contains(stat))
            {
                stats.Add(stat);
                OnStatsChanged.Invoke();
            }
        }

        private void OnChildRemoved(Transform child)
        {
            var stat = child.GetComponent<AEntityStat>();
            if (stat != null)
            {
                stats.Remove(stat);
                OnStatsChanged.Invoke();
            }
        }
    }
}
