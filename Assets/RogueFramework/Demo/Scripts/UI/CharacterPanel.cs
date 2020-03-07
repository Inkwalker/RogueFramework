using UnityEngine;
using System.Collections;

namespace RogueFramework.Demo
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] InventoryView inventoryView;
        [SerializeField] EquipmentView equipmentView;

        public void Show(Entity character)
        {
            inventoryView.SetTarget(character.GetEntityComponent<Inventory>());
            equipmentView.SetTarget(character.GetEntityComponent<Equipment>());

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
