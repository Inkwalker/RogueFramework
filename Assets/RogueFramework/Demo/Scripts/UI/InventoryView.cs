using UnityEngine;

namespace RogueFramework.Demo
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] ItemView itemViewPrefab = null;
        [SerializeField] Transform content = null;

        private Inventory target;

        public void SetTarget(Inventory inventory)
        {
            target = inventory;

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
                }
            }
        }
    }
}
