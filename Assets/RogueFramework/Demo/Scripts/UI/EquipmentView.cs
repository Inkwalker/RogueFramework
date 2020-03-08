using UnityEngine;

namespace RogueFramework.Demo
{
    public class EquipmentView : MonoBehaviour
    {
        [SerializeField] ItemView itemViewPrefab = null;
        [SerializeField] Transform content = null;
        [SerializeField] ActionsView actionPanel = null;

        private Equipment target;
        private Item selectedItem;

        private void OnEnable()
        {
            actionPanel.Hide();
        }

        private void OnDisable()
        {
            if (target != null)
            {
                target.OnEquipped.RemoveListener(OnEquipmentChanged);
                target.OnUnequipped.RemoveListener(OnEquipmentChanged);
            }
        }

        public void SetTarget(Equipment equipment)
        {
            if (target != null)
            {
                target.OnEquipped.RemoveListener(OnEquipmentChanged);
                target.OnUnequipped.RemoveListener(OnEquipmentChanged);
            }

            target = equipment;

            target.OnEquipped.AddListener(OnEquipmentChanged);
            target.OnUnequipped.AddListener(OnEquipmentChanged);

            CreateItemViews();
        }

        private void CreateItemViews()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }

            if (target != null)
            {
                foreach (var item in target.Equipped)
                {
                    var itemView = Instantiate(itemViewPrefab, content);

                    itemView.Item = item;
                    itemView.onClick.AddListener(OnItemClicked);
                }
            }
        }

        private void OnItemClicked(ItemView sender)
        {
            var actor = target.Entity.GetEntityComponent<Actor>();

            if (actor)
            {
                var abilities = actor.GetApplicableAbilities(sender.Item.Entity);

                actionPanel.Show(abilities);
                actionPanel.onActionSelected.AddListener(OnActionSelected);

                selectedItem = sender.Item;
            }
        }

        private void OnActionSelected(AEntityAbility ability)
        {
            if (ability != null && selectedItem != null)
            {
                ability.Perform(target.Entity.GetEntityComponent<Actor>(), selectedItem.Entity);
            }

            selectedItem = null;

            actionPanel.Hide();
            actionPanel.onActionSelected.RemoveListener(OnActionSelected);
        }

        private void OnEquipmentChanged(Item item)
        {
            CreateItemViews();

            if (selectedItem == item)
            {
                selectedItem = null;
                actionPanel.Hide();
                actionPanel.onActionSelected.RemoveListener(OnActionSelected);
            }
        }
    }
}
