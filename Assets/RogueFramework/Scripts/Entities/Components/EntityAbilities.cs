using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RogueFramework
{
    public class EntityAbilities : AEntityComponent
    {
        [SerializeField] bool includeChildEntities = false;

        private TransformChildrenTracker tracker;
        private List<AEntityAbility> abilities = new List<AEntityAbility>();
        private List<EntityAbilities> containers = new List<EntityAbilities>();

        public List<AEntityAbility> Abilities
        {
            get
            {
                var result = new List<AEntityAbility>();

                result.AddRange(abilities);

                foreach (var container in containers)
                {
                    result.AddRange(container.Abilities);
                }

                return result;
            }
        }

        public UnityEvent OnAbilitiesChanged;

        private void Awake()
        {
            abilities = new List<AEntityAbility>();
            AddChildAbilities();

            tracker = GetComponent<TransformChildrenTracker>();
            if (tracker == null)
            {
                tracker = gameObject.AddComponent<TransformChildrenTracker>();
                tracker.hideFlags = HideFlags.HideInInspector;
            }

            tracker.OnChildAdded.AddListener(OnChildAdded);
            tracker.OnChildRemoved.AddListener(OnChildRemoved);
        }

        public AEntityAbility Get(AbilitySignature signature)
        {
            return Find(signature);
        }

        public List<AEntityAbility> GetAll(AbilitySignature signature)
        {
            var result = new List<AEntityAbility>();
            FindAll(result, signature);

            return result;
        }

        private AEntityAbility Find(AbilitySignature signature)
        {
            var ability = abilities.Find(a => a.Signature == signature);

            if (ability != null) return ability;

            foreach (var container in containers)
            {
                ability = container.Find(signature);
                if (ability != null) return ability;
            }

            return null;
        }

        private void FindAll(List<AEntityAbility> list, AbilitySignature signature)
        {
            list.AddRange(abilities.FindAll(a => a.Signature == signature));

            foreach (var container in containers)
            {
                container.FindAll(list, signature);
            }
        }

        private void AddChildAbilities()
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

            bool abilitiesChanged = false;

            var ability = child.GetComponent<AEntityAbility>();

            if (ability != null)
            {
                if (abilities.Contains(ability) == false)
                {
                    abilities.Add(ability);
                    abilitiesChanged = true;
                }
            }
            else if (includeChildEntities)
            {
                var entity = child.GetComponent<Entity>();

                if (entity != null)
                {
                    var ea = entity.GetEntityComponent<EntityAbilities>();
                    if (ea && containers.Contains(ea) == false)
                    {
                        containers.Add(ea);
                        abilitiesChanged = true;
                    }
                }
            }

            return abilitiesChanged;
        }

        private void OnChildAdded(Transform child)
        {
            if (AddChildAbilities(child))
                OnAbilitiesChanged.Invoke();
        }

        private void OnChildRemoved(Transform child)
        {
            bool abilitiesChanged = false;

            var ability = child.GetComponent<AEntityAbility>();

            if (ability != null)
            {
                abilities.Remove(ability);
                abilitiesChanged = true;
            }

            var entity = child.GetComponent<Entity>();

            if (entity != null)
            {
                var ea = entity.GetEntityComponent<EntityAbilities>();
                containers.Remove(ea);
                abilitiesChanged = true;
            }

            if (abilitiesChanged) OnAbilitiesChanged.Invoke();
        }
    }
}
