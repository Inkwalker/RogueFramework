using UnityEngine;
using UnityEngine.UI;

namespace RogueFramework.Demo
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] Image iconImage = null;
        [SerializeField] Text  nameText  = null;

        private Item item;

        public Item Item
        {
            get => item;
            set
            {
                item = value;

                iconImage.enabled = item != null;
                iconImage.sprite = item?.Type.Icon;

                nameText.text = item?.Type.DisplayName;                
            }
        }
    }
}
