using System.Collections.Generic;

namespace RogueFramework
{
    public class AbilitiesContainer : AEntityComponent
    {
        private List<AEntityAbility> abilities;

        public IReadOnlyList<AEntityAbility> Abilities
        {
            get
            {
                if (abilities == null)
                {
                    abilities = new List<AEntityAbility>();
                    SearchAbilities();
                }

                return abilities;
            }
        }

        private void OnTransformChildrenChanged()
        {
            SearchAbilities();
        }

        public AEntityAbility Get(AbilitySignature signature)
        {
            if (abilities == null) SearchAbilities();
            return abilities.Find(a => a.Signature == signature);
        }

        public List<AEntityAbility> GetAll(AbilitySignature signature)
        {
            if (abilities == null) SearchAbilities();
            return abilities.FindAll(a => a.Signature == signature);
        }

        public void SearchAbilities()
        {
            if (abilities == null) abilities = new List<AEntityAbility>();

            abilities.Clear();
            abilities.AddRange(GetComponentsInChildren<AEntityAbility>(false));
        }
    }
}
