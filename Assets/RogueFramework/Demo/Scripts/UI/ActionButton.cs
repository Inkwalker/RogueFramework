using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RogueFramework.Demo
{
    [RequireComponent(typeof(Button))]
    public class ActionButton : MonoBehaviour
    {
        private AEntityAbility ability;

        public AEntityAbility Ability
        {
            get => ability;
            set
            {
                ability = value;

                var text = GetComponentInChildren<Text>();

                if (text) text.text = ability != null ? ability.name : "None";
            }
        }

        public ClickEvent onClick;

        private void Awake()
        {
            GetComponent<Button>()?.onClick.AddListener(
                () => 
            {
                onClick.Invoke(this);
            });
        }

        [System.Serializable] public class ClickEvent : UnityEvent<ActionButton> { }
    }
}
