using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RogueFramework
{
    public class EntityAbilities : AEntityComponent
    {
        [SerializeField] List<Transform> containers = new List<Transform>();

        private AbilitiesTable abilities = new AbilitiesTable();
        private bool initialized = false;

        public IReadOnlyList<AEntityAbility> Abilities => abilities.Abilities;
        public IReadOnlyList<AEntityAbility> SharedAbilities => abilities.SharedAbilities;

        public UnityEvent OnAbilitiesChanged;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (!initialized)
            {
                if (containers.Contains(transform) == false) containers.Add(transform);

                foreach (var container in containers)
                {
                    AddAbilitiesFromContainer(container);

                    var containerTracker = TransformChildrenTracker.GetOrCreate(container.gameObject);

                    containerTracker.OnChildAdded.AddListener(OnChildAdded);
                    containerTracker.OnChildRemoved.AddListener(OnChildRemoved);
                }

                initialized = true;
            }
        }

        private void OnDestroy()
        {
            foreach (var container in containers)
            {
                if (container != null)
                {
                    var tracker = container.GetComponent<TransformChildrenTracker>();

                    if (tracker != null)
                    {
                        tracker.OnChildAdded.RemoveListener(OnChildAdded);
                        tracker.OnChildRemoved.RemoveListener(OnChildRemoved);
                    }
                }
            }
        }

        public AEntityAbility Get(AbilitySignature signature)
        {
            return abilities.Get(signature);
        }

        public List<AEntityAbility> GetAll(AbilitySignature signature)
        {
            return abilities.GetAll(signature);
        }

        private void AddAbilitiesFromContainer(Transform container)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                AddChildAbilities(child);
            }
        }

        private bool AddChildAbilities(Transform child)
        {
            if (child.gameObject.activeSelf == false) return false; //Skip disabled objects

            bool abilityAdded = false;

            var entity = child.GetComponent<Entity>();
            if (entity == null)
            {
                var ability = child.GetComponent<AEntityAbility>();
                if (ability != null)
                {
                    if (abilities.Add(ability)) abilityAdded = true;
                }
            }
            else
            {
                var entityAbilities = entity.GetEntityComponent<EntityAbilities>();
                if (entityAbilities != null)
                {
                    entityAbilities.Initialize();

                    int count = abilities.AddRange(entityAbilities.SharedAbilities);
                    if (count > 0) abilityAdded = true;
                }
            }

            return abilityAdded;
        }

        private void OnChildAdded(Transform child)
        {
            int count = Abilities.Count;

            if (AddChildAbilities(child))
            {
                count = Abilities.Count - count;
                Debug.Log($"{Entity.name} | Acquired {count} new abilities.", this);

                OnAbilitiesChanged.Invoke();
            }
        }

        private void OnChildRemoved(Transform child)
        {
            int abilitiesRemoved = 0;

            var entity = child.GetComponent<Entity>();

            if (entity != null)
            {
                var entityAbilities = entity.GetEntityComponent<EntityAbilities>();

                if (entityAbilities != null)
                {
                    foreach (var item in entityAbilities.SharedAbilities)
                    {
                        if (abilities.Remove(item)) abilitiesRemoved++;
                    }
                }
            }
            else
            {
                var ability = child.GetComponent<AEntityAbility>();

                if (ability != null)
                {
                    if (abilities.Remove(ability)) abilitiesRemoved++;
                }
            }

            if (abilitiesRemoved > 0)
            {
                Debug.Log($"{Entity.name} | Lost {abilitiesRemoved} abilities.", this);

                OnAbilitiesChanged.Invoke();
            }
        }

        private class AbilitiesTable
        {
            //TODO: Add signature lookup table for performance increase.

            private List<AEntityAbility> abilities = new List<AEntityAbility>();
            private List<AEntityAbility> sharedAbilities = new List<AEntityAbility>();

            public IReadOnlyList<AEntityAbility> Abilities => abilities;
            public IReadOnlyList<AEntityAbility> SharedAbilities => sharedAbilities;

            public bool Add(AEntityAbility ability)
            {
                if (abilities.Contains(ability) == false)
                {
                    abilities.Add(ability);
                    if (ability.Shared) sharedAbilities.Add(ability);

                    return true;
                }

                return false;
            }

            public int AddRange(IReadOnlyCollection<AEntityAbility> range)
            {
                int counter = 0;

                foreach (var item in range)
                {
                    if (Add(item)) counter++;
                }

                return counter;
            }

            public AEntityAbility Get(AbilitySignature signature)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    var ability = abilities[i];
                    if (ability.Signature == signature) 
                        return ability;
                }

                return null;
            }

            public List<AEntityAbility> GetAll(AbilitySignature signature)
            {
                var result = new List<AEntityAbility>();

                foreach (var item in abilities)
                {
                    if (item.Signature == signature)
                        result.Add(item);
                }

                return result;
            }

            public bool Remove(AEntityAbility ability)
            {
                  if (ability.Shared)
                    sharedAbilities.Remove(ability);

                return abilities.Remove(ability);
            }

            public int Remove(Entity owner)
            {
                sharedAbilities.RemoveAll(a => a.Owner == owner);

                return abilities.RemoveAll(a => a.Owner == owner);
            }
        }
    }
}
