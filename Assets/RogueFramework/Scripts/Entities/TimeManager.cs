using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RogueFramework
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] Level level = default;
        [SerializeField] float tickTime = 1f;

        private long tickCount;

        public UnityEvent OnCycleStart;
        public UnityEvent OnCycleEnd;

        private void OnEnable()
        {
            StartCoroutine(Tick());
        }

        IEnumerator Tick()
        {
            yield return null;

            var actionResults = new List<ActorActionResult>();

            while (gameObject.activeInHierarchy)
            {
                OnCycleStart.Invoke();

                var entities = level.Entities.All;
                foreach (var entity in entities)
                {
                    entity.Tick();
                }

                //Add energy and take turns
                var actors = level.Entities.Actors;
                foreach (var actor in actors)
                {
                    if (actor.HasEnoughEnergy())
                    {
                        var result = actor.TakeTurn();
                        if (result != null)
                            actionResults.Add(result);
                    }
                }

                //Wait till actions are finished
                if (actionResults.Count > 0)
                {
                    bool allActionsFinished = false;
                    while (allActionsFinished == false)
                    {
                        foreach (var result in actionResults)
                        {
                            if (result.Finished == false)
                            {
                                allActionsFinished = false;
                                break;
                            }

                            allActionsFinished = true;
                        }

                        if (allActionsFinished == false)
                            yield return null;
                    }
                    actionResults.Clear();
                }

                //GC friendly wait
                float timer = tickTime;
                while (timer > 0)
                {
                    yield return null;
                    timer -= Time.deltaTime;

                }

                tickCount++;

                OnCycleEnd.Invoke();
            }
        }
    }
}
