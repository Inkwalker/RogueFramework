using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace RogueFramework.Demo
{
    public class ItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image iconImage = null;
        [SerializeField] Text  nameText  = null;

        private Item item;

        public ViewEvent onClick;

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

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke(this);
        }

        [System.Serializable]
        public class ViewEvent : UnityEvent<ItemView> { }
    }
}
