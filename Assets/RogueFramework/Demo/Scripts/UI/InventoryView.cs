using UnityEngine;

namespace RogueFramework.Demo
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] ItemView itemViewPrefab = null;
        [SerializeField] Transform content = null;
        [SerializeField] ActionsView actionPanel = null;

        private Inventory target;
        private Item selectedItem;

        private void OnEnable()
        {
            actionPanel.Hide();
        }

        private void OnDisable()
        {
            if (target != null)
            {
                target.OnItemAdded.RemoveListener(OnInventoryChanged);
                target.OnItemRemoved.RemoveListener(OnInventoryChanged);
            }
        }

        public void SetTarget(Inventory inventory)
        {
            if (target != null)
            {
                target.OnItemAdded.RemoveListener(OnInventoryChanged);
                target.OnItemRemoved.RemoveListener(OnInventoryChanged);
            }

            target = inventory;

            target.OnItemAdded.AddListener(OnInventoryChanged);
            target.OnItemRemoved.AddListener(OnInventoryChanged);

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
                foreach (var item in target.Items)
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

        private void OnInventoryChanged(Item item)
        {
            CreateItemViews();
        }
    }
}
